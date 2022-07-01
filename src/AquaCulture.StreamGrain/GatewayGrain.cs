using AquaCulture.Models;
using AquaCulture.StreamGrain.Data;
using AquaCulture.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AquaCulture.StreamGrain
{
    public class GatewayGrain : Orleans.Grain, IGatewayGrain
    {
        private readonly ILogger logger;
        StreamRawDataService streamService;
        GatewayService gatewayService;

        public GatewayGrain(ILogger<GatewayGrain> logger)
        {
            ReadConfig();
            this.streamService = new StreamRawDataService();
            this.gatewayService = new GatewayService();
            this.logger = logger;
        }

        public Task<Gateway> GetGatewayInfo(string GatewayId)
        {
            var res = gatewayService.GetDataByGatewayId(GatewayId);
            return Task.FromResult(res);
        }

        public Task<bool> SendRawData(StreamRawData data)
        {
            var res = streamService.InsertData(data);
            return Task.FromResult( res);
        }

        
        void ReadConfig()
        {
            try
            {
                var builder = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("config.json", optional: false);

                IConfiguration Configuration = builder.Build();

                MailService.MailUser = Configuration["MailSettings:MailUser"];
                MailService.MailPassword = Configuration["MailSettings:MailPassword"];
                MailService.MailServer = Configuration["MailSettings:MailServer"];
                MailService.MailPort = int.Parse(Configuration["MailSettings:MailPort"]);
                MailService.SetTemplate(Configuration["MailSettings:TemplatePath"]);
                MailService.SendGridKey = Configuration["MailSettings:SendGridKey"];
                MailService.UseSendGrid = true;


                SmsService.UserKey = Configuration["SmsSettings:ZenzivaUserKey"];
                SmsService.PassKey = Configuration["SmsSettings:ZenzivaPassKey"];
                SmsService.TokenKey = Configuration["SmsSettings:TokenKey"];

                AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];


            }
            catch (Exception ex)
            {
                Console.WriteLine("read config failed:" + ex);
            }
            Console.WriteLine("Read config successfully.");
        }

    }

}