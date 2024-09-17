using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class EncodeHelpers
    {

        /// <summary>
        /// Converting a string to MD5 code to encrypt password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public static string Encode(string strString, string strKeyPhrase)
        {

            strString = KeyED(strString, strKeyPhrase);
            Byte[] byt = System.Text.Encoding.UTF8.GetBytes(strString);
            strString = Convert.ToBase64String(byt);
            return strString;
        }
        public static string KeyED(string strString, string strKeyphrase)
        {
            int strStringLength = strString.Length;
            int strKeyPhraseLength = strKeyphrase.Length;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(strString);

            for (int i = 0; i < strStringLength; i++)
            {
                int pos = i % strKeyPhraseLength;
                int xorCurrPos = (int)(strString[i]) ^ (int)(strKeyphrase[pos]);
                builder[i] = Convert.ToChar(xorCurrPos);
            }

            return builder.ToString();
        }
    }
}
