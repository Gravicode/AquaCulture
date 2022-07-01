using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace AquaCulture.Tools
{
    public class SmsService
    {
        public static string TokenKey { get; set; }

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class response
        {

            private responseMessage messageField;

            /// <remarks/>
            public responseMessage message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class responseMessage
        {

            private byte statusField;

            private string textField;

            /// <remarks/>
            public byte status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }

            /// <remarks/>
            public string text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }
        }

        public static string PassKey { get; set; }
        public static string UserKey { get; set; }

        public enum SMSStatus { Success = 0, NumberNotValid = 1, UserPassKeyNotValid = 5, ContentRejected = 6, SMSSpams = 89, CreditNotSufficient = 99 };
        static HttpClient client;
        public static async Task<bool> SendSms(string Message, string PhoneTo, string PhoneFrom = "08174810345")
        {
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                // DANGER! This is insecure. See http://twil.io/secure
                //string UserKey = ConfigurationManager.AppSettings["ZenzivaUserKey"];
                //string PassKey = ConfigurationManager.AppSettings["ZenzivaPassKey"];

                if (client == null)
                {
                    client = new HttpClient();
                }

                string Url = $"https://websms.co.id/api/smsgateway?token={TokenKey}&to={PhoneTo}&msg={Message}";

                var res = await client.GetAsync(Url);
                if (res.IsSuccessStatusCode)
                {
                    var respStr = await res.Content.ReadAsStringAsync();
                    var hasil = JsonConvert.DeserializeObject<SmsResult>(respStr);

                    if (hasil.status == "success")
                    {
                        Console.WriteLine("sms notification result:" + hasil.message);
                        //LogHelpers.source = typeof(SmsService).ToString();
                        //LogHelpers.message = "failed to send sms with the following error:" + resObj.message;
                        //LogHelpers.user = CommonWeb.GetCurrentUser();
                        //LogHelpers.WriteLog();
                        Logs.WriteLog("sms notification result:" + hasil.message);
                        return true;
                    }
                    else
                        return false;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                //LogHelpers.source = typeof(SmsService).ToString();
                //LogHelpers.message = "failed to send sms with the following error:" + ex.Message;
                //LogHelpers.user = CommonWeb.GetCurrentUser();
                //LogHelpers.WriteLog();
                Logs.WriteLog($"failed to send email with the following error:{ex}");
            }
            return false;

        }
        /*
        public static async Task<bool> SendSms(string Message, string PhoneTo, string PhoneFrom = "+628174810345")
        {
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                // DANGER! This is insecure. See http://twil.io/secure
                //string UserKey = ConfigurationManager.AppSettings["ZenzivaUserKey"];
                //string PassKey = ConfigurationManager.AppSettings["ZenzivaPassKey"];
                if (PhoneTo.StartsWith("08"))
                {
                    PhoneTo = "+628" + PhoneTo.Substring(2);
                }
                if (client == null)
                {
                    client = new HttpClient();
                }

                string Url = $"https://reguler.zenziva.net/apps/smsapi.php?userkey={UserKey}&passkey={PassKey}&nohp={PhoneTo}&pesan={Message}";

                var res = await client.GetAsync(Url);
                if (res.IsSuccessStatusCode)
                {
                    var serializer = new XmlSerializer(typeof(responseMessage));
                    var respStr = await res.Content.ReadAsStringAsync();

                    var buffer = Encoding.UTF8.GetBytes(respStr);
                    using (var stream = new MemoryStream(buffer))
                    {
                        XElement purchaseOrder = XElement.Load(stream);
                        var resObj = (from item in purchaseOrder.Descendants("message")
                                      select new { code = int.Parse(item.Element("status").Value), message = item.Element("text").Value }).FirstOrDefault();
                        //var resObj = (responseMessage)serializer.Deserialize(stream);
                        var hasil = (SMSStatus)resObj.code;
                        if (hasil != SMSStatus.Success)
                        {
                            Console.WriteLine("sms notification result:" + resObj.message);
                            //LogHelpers.source = typeof(SmsService).ToString();
                            //LogHelpers.message = "failed to send sms with the following error:" + resObj.message;
                            //LogHelpers.user = CommonWeb.GetCurrentUser();
                            //LogHelpers.WriteLog();
                            Logs.WriteLog("sms notification result:" + resObj.message);
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                //LogHelpers.source = typeof(SmsService).ToString();
                //LogHelpers.message = "failed to send sms with the following error:" + ex.Message;
                //LogHelpers.user = CommonWeb.GetCurrentUser();
                //LogHelpers.WriteLog();
                Logs.WriteLog($"failed to send email with the following error:{ex}");
                return false;
            }


        }
        
        public static bool SendSmsWithTwilio(string Message,string PhoneTo, string PhoneFrom= "+17204667090")
        {
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                // DANGER! This is insecure. See http://twil.io/secure
                string accountSid = ConfigurationManager.AppSettings["TwilioSID"];
                string authToken = ConfigurationManager.AppSettings["TwilioToken"];

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber(PhoneFrom),
                    to: new Twilio.Types.PhoneNumber(PhoneTo.StartsWith("+62") ? PhoneTo : $"+62{PhoneTo}")
                );

                Console.WriteLine(message.Sid);
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                LogHelpers.source = typeof(EmailService).ToString();
                LogHelpers.message = "failed to send sms with the following error:" + ex.Message;
                LogHelpers.user = CommonWeb.GetCurrentUser();
                LogHelpers.WriteLog();
                return false;
            }
           

        }*/
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SmsResult
    {
        public string status { get; set; }
        public string message { get; set; }
    }


}
