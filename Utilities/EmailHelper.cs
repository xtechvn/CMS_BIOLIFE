using System;
using System.Net.Mail;
using System.ComponentModel;
using Utilities.Contants;
using Utilities.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp;

namespace Utilities
{
    public static class EmailHelper
    {
        static int mailSent = (int)ResponseType.SUCCESS;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                mailSent = (int)ResponseType.EXISTS;
            }
            if (e.Error != null)
            {
                mailSent = (int)ResponseType.ERROR;
            }
            else
            {

            }
            mailSent = (int)ResponseType.SUCCESS;
        }
        public static int SendEmail(EmailAccountModel sender, string receive_email, string email_title, string email_body, string[] cc_email = null, string[] bcc_email = null)
        {
            try
            {
                // Command-line argument must be the SMTP host.
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(sender.Email, sender.Password);
                client.Port = sender.Port;
                client.Host = sender.Host;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                string msg_id = new Random().Next(0, 99999999).ToString();
                // Specify the email sender.
                // Create a mailing address that includes a UTF8 character
                // in the display name.
                // Set destinations for the email message.
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender.Email, sender.Display_name, System.Text.Encoding.UTF8);
                foreach (var address in receive_email.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }
                if (cc_email != null)
                {
                    foreach (string item in cc_email) message.CC.Add(item);
                }
                if (bcc_email != null)
                {
                    foreach (string item in bcc_email) message.Bcc.Add(item);
                }
                // Specify Email body is HTML.
                message.IsBodyHtml = true;
                // Specify the message content.
                message.Body = email_body;
                // Additional Message:
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = email_title;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // Set the method that is called back when the send operation ends.
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                // The userState can be any object that allows your callback
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                string userState = "msg: " + msg_id;
                client.SendAsync(message, userState);
                return mailSent;
            }
            catch (SmtpException ex)
            {
                return (int)ResponseType.FAILED;
            }
            catch (Exception ex)
            {
                return (int)ResponseType.ERROR;
            }
        }
        /*
        public static async Task SendMailAsync(string receive_email, string email_title, string email_body, string cc_email, string bcc_email)
        {
            string token = string.Empty;
            try
            {
                var configApiCms = FileHelpers<ApiCmsConfig>.LoadConfig("config.json");
                var apiPrefix = configApiCms.API_CMS_URL + configApiCms.API_SEND_MAIL;
                var key_token_api = configApiCms.KEY_TOKEN_API;
                var j_param = new Dictionary<string, string> {
                    { "receive_email", receive_email },
                    { "email_title", email_title },
                    { "email_body", email_body },
                    { "cc_email", cc_email },
                    { "bcc_email", bcc_email }};

                using (HttpClient httpClient = new HttpClient())
                {
                    token = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), key_token_api);
                    var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", token) });
                    await httpClient.PostAsync(apiPrefix, content);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SendMail - " + ex.Message.ToString() + " Token:" + token);
            }
        }*/
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    fs.Dispose();
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ByteArrayToFile(fileName = " + fileName + ") - EmailHelper: " + ex);
                return false;
            }
        }
        public static byte[] PdfSharpConvert(String html)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            try
            {
                using (var outputStream = new MemoryStream())
                {
                    PdfGenerateConfig pdfGenerateConfig = new PdfGenerateConfig();
                    pdfGenerateConfig.PageSize = PageSize.A4;
                    var pdf = PdfGenerator.GeneratePdf(html, pdfGenerateConfig, null, null);
                    pdf.Save(outputStream);
                    var result = outputStream.ToArray();
                    pdf.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}