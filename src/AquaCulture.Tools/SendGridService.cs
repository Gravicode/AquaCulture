using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AquaCulture.Tools
{
    public class SendGridService
    {
        public static async System.Threading.Tasks.Task<bool> SendEmail(string key, string subject, string emailfrom, string emailto, string message)
        {
            var apiKey = key;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailfrom),
                Subject = subject,
                PlainTextContent = "",
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(emailto));
            var response = await client.SendEmailAsync(msg);
            if ((int)response.StatusCode == 202)
            {
                //Logs.WriteLog("email notification result: email was sent successfully");
                return true;
            }
            else
            {
                //Logs.WriteLog("email notification result: failed sending email");
                return false;
            }
        }
    }
}
