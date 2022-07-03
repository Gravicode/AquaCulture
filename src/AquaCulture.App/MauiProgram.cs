using Blazored.LocalStorage;
using Blazored.Toast;
using AquaCulture.Tools;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using AquaCulture.App.Data;
using AquaCulture.App.Helpers;
using AquaCulture.App.Properties;
using AquaCulture.App.Services;
using System.Text;

namespace AquaCulture.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.appsettings));

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();


            builder.Configuration.AddConfiguration(config);

            ConfigureServices(builder.Services, builder.Configuration);
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            //builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }

        static void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager Configuration)
        {


            
            AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];
            AppConstants.RedisCon = Configuration["RedisCon"];
            AppConstants.BlobConn = Configuration["BlobConn"];
            services.AddBlazoredLocalStorage();
            services.AddBlazoredToast();

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

            AppConstants.GrpcUrl = Configuration["App:GrpcUrl"];
            AppConstants.LaporanStatistikUrl = Configuration["Reports:LaporanStatistikUrl"];
            AppConstants.DefaultPass = Configuration["App:DefaultPass"];

            services.AddMauiBlazorWebView();
            services.AddBlazoredToast();
            services.AddBlazoredLocalStorage();

            services.AddSingleton<GrpcChannel>(GrpcChannel.ForAddress(
                AppConstants.GrpcUrl, new GrpcChannelOptions
                {
                    MaxReceiveMessageSize = 8 * 1024 * 1024, // 5 MB
                    MaxSendMessageSize = 8 * 1024 * 1024, // 2 MB                
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                }));
            services.AddScoped<ISyncMemoryStorageService, LocalMemoryStorage>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            
            services.AddSingleton<TempStorageService>();

            
            //data service
            services.AddTransient<GatewayService>();
            services.AddTransient<DeviceService>();
            services.AddTransient<AuthService>();
            services.AddTransient<StreamRawDataService>();
            services.AddTransient<UserProfileService>();
            //helpers
            services.AddSingleton<FaceService>();
            services.AddSingleton<AzureBlobHelper>();

        }
    }
}