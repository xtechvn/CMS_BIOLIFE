using System;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using OtpNet;
using Entities.ViewModels;
using Utilities.Common;
using System.Text.RegularExpressions;
using Entities.ViewModels.Login;
using Newtonsoft.Json;
using WEB.CMS.Models;

namespace Utilities
{
    public static class MFAService
    {
        public static byte[] Get_AESKey(byte[] data)
        {
            
            if(data.Length>=16 && data.Length < 24)
            {
                return data.Take(16).ToArray();
            }
            if (data.Length >= 24 && data.Length < 32)
            {
                return data.Take(24).ToArray();
            }
            if (data.Length >= 32)
            {
                return data.Take(32).ToArray();
            }

            return data;

        }
        public static byte[] Get_AESIV(byte[] data)
        {

            if (data.Length >= 16)
            {
                return data.Take(16).ToArray();
            }

            return data;

        }
        public static byte[] ConvertBase64StringToByte(string text)
        {
            try
            {
                return Convert.FromBase64String(text);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ConvertBase64StringToByte - MFAService" + ex);
            }
            return new byte[0];
        }
        public static string ConvertByteToBase64String(byte[] data)
        {
            try
            {
                return Convert.ToBase64String(data);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ConvertBase64StringToByte - MFAService" + ex);
            }
            return null;
        }
        public static byte[] AES_EncryptToByte(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string AES_DecryptToString(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
        public static bool CompareOTP(Mfauser record, string otp)
        {
            try
            {
                var bytes = Base32OTPEncoding.ToBytes(record.SecretKey.Trim());
                var totp = new Totp(bytes);
                var otp_code = totp.ComputeTotp();
                if (otp == otp_code)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CompareOTP - MFAService" + ex);
                return false;
            }

        }
        public static string BackupCodeMD5FromInput(string input, Mfauser mfa_record)
        {
            try
            {
                MD5 md5_generator = MD5.Create();
                string hash_str = input.Trim() + "_" + mfa_record.UserId.ToString().Trim() + "_" + mfa_record.Username.ToString().Trim() + "_" + mfa_record.UserCreatedYear.ToString().Trim();
                byte[] hash_byte = System.Text.Encoding.ASCII.GetBytes(hash_str);
                string backupcode_input = Base32Encoding.ToString(md5_generator.ComputeHash(hash_byte));
                return backupcode_input.Trim();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("BackupCodeMD5FromInput - MFAService" + ex);
                return null;
            }
        }
        public static async Task<Mfauser> Get2FAModel(UserDetailViewModel client_detail)
        {
            try
            {
                Mfauser new_mfa_record = new Mfauser()
                {
                    UserId = client_detail.Entity.Id,
                    Email = client_detail.Entity.Email.Trim(),
                    Username = client_detail.Entity.UserName.Trim(),
                    SecretKey = "",
                    Status = 0,
                    BackupCode = "",
                    UserCreatedYear = client_detail.Entity.CreatedOn.Value.Year.ToString()
                };
                string secret_key = GenerateSecretKey(client_detail);
                if (secret_key == null)
                {
                    return null;
                }
                new_mfa_record.SecretKey = secret_key.Trim();
                string backupcode = new Random().Next(0, 99999999).ToString(new string('0', 8));
                string backup_code_md5 = BackupCodeMD5FromInput(backupcode, new_mfa_record);
                if (backup_code_md5 == null)
                {
                    return null;

                }
                new_mfa_record.BackupCode = backup_code_md5;
                return new_mfa_record;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Get2FAModel - MFAService" + ex);
                return null;
            }
        }
        public static string GenerateSecretKey(UserDetailViewModel client_detail)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                /*Secret Key Generate*/
                string SecretKey = "";
                string random_int_begin = new Random().Next(0, 99999999).ToString(new string('0', 8));
                string random_int_last = new Random().Next(0, 99999999).ToString(new string('0', 8));
                // 12345678_55_minh.nq_11111111_minhnguyen@Adavigo.vn
                string base_text = random_int_begin.Trim() + "_" + client_detail.Entity.Id + "_" + client_detail.Entity.UserName.Trim() + "_" + random_int_last.Trim() + "_" + client_detail.Entity.Email.Trim();
                byte[] base_text_in_bytes = System.Text.Encoding.ASCII.GetBytes(base_text);
                byte[] hash_text_sha256 = sHA256.ComputeHash(base_text_in_bytes);
                SecretKey = Base32OTPEncoding.ToString(hash_text_sha256);
                //Get 32 first char from base32 string, as google authenticator secret key length
                SecretKey = SecretKey.Substring(0, 32).Trim();
                return SecretKey.Trim();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GenerateSecretKeyAsync - AccountController" + ex);
                return null;
            }
        }
        public static string GenerateQRCode(Mfauser result,string otpEnviroment) 
        {
            try
            {
                if (result != null)
                {
                    string enviroment = otpEnviroment;
                    if (enviroment == null) enviroment = "";
                    string label_name = "AdavigoCMS_" + enviroment + "-" + result.Username.Trim();
                    //string label_name = "qc-be-Adavigo.com:" + enviroment + "-" + result.Username.Trim();
                    string secret_key = result.SecretKey.Trim();
                    string issuer = "Adavigo";
                    string otp_auth_url = @"" + "otpauth://totp/" + issuer + ":" + label_name + "?secret=" + secret_key + "&issuer=" + issuer + "";
                    return otp_auth_url;
                }
                return null;

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GenerateQRCode - AccountController" + ex);
                return null;
            }
        }
        public static string FormatKey(string unformattedKey)
        {
            try
            {
                return Regex.Replace(unformattedKey.Trim(), ".{4}", "$0 ");
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FormatKey - AccountController" + ex);
                return null;
            }
        }
        public static UserLoginModel DecryptLoginModelFromToken(string token)
        {
            UserLoginModel login_model = new UserLoginModel();
            try
            {
                var key = Get_AESKey(ConvertBase64StringToByte(ReadFile.LoadConfig().AES_KEY));
                var iv = Get_AESIV(ConvertBase64StringToByte(ReadFile.LoadConfig().AES_IV));
                var decrypt_byte = ConvertBase64StringToByte(token.Trim().Replace(" ", ""));
                var decrypt_text = AES_DecryptToString(decrypt_byte, key, iv);
                if (decrypt_text != null && decrypt_text.Trim() != "")
                {
                    login_model = JsonConvert.DeserializeObject<UserLoginModel>(decrypt_text);
                }
            }
            catch
            {

            }
            return login_model;

        }
        public static string Base64StringToURLParam(string text)
        {
            var p = text.Replace(@"/", "-").Replace("+", "_");
            return p;
        }
        public static string URLParamToBase64String(string text)
        {
            var p= text.Replace("-", @"/").Replace("_", "+");
            return p;
        }
    }
    
}