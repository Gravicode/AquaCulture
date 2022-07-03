using AquaCulture.Tools;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AquaCulture.Data;
using AquaCulture.Services.Grpc;
using ProtoBuf.Grpc.Server;
using System.Text;
using AquaCulture.Services.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices(builder.Services, builder.Configuration);
// Configure Kestrel to listen on a specific HTTP port 
/*
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    options.ListenAnyIP(8585, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
*/
var app = builder.Build();
Configure(app, app.Environment);
ConfigureRouting(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
{
    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //services.AddCors(options =>
    //{
    //    options.AddPolicy("AllowAllOrigins",
    //        builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS"));
    //});

    services.AddSingleton<GatewayService>();
    services.AddSingleton<DeviceService>();
    services.AddSingleton<StreamRawDataService>();
    services.AddTransient<AzureBlobHelper>();
    AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];
    AppConstants.RedisCon = Configuration["RedisCon"];
    AppConstants.BlobConn = Configuration["BlobConn"];
    


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


    /*
    //ML
    services.AddPredictionEnginePool<ImageInputData, TinyYoloPrediction>().
        FromFile(_mlnetModelFilePath);

    //services.AddTransient<IImageFileWriter, ImageFileWriter>();
    services.AddTransient<IObjectDetectionService, ObjectDetectionService>();
    */
    //services.AddSignalR(hubOptions =>
    //{
    //    hubOptions.MaximumReceiveMessageSize = 128 * 1024; // 1MB
    //});

    AquaCultureDB db = new AquaCultureDB();
    db.Database.EnsureCreated();
   

    //GRPC
    services.AddCodeFirstGrpc(options =>
    {
        options.EnableDetailedErrors = true;
        options.MaxReceiveMessageSize = 8 * 1024 * 1024; // 2 MB
        options.MaxSendMessageSize = 8 * 1024 * 1024; // 5 MB
    });
    services.AddGrpcHealthChecks()
                    .AddCheck("Sample", () => HealthCheckResult.Healthy());
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //only for GRPC WEB
    services.AddCors(o => o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    }));




}
void Configure(WebApplication app, IWebHostEnvironment env)
{
    //if use GRPC Native
    // app.UseCors(x => x
    //.AllowAnyMethod()
    //.AllowAnyHeader()
    //.SetIsOriginAllowed(origin => true) // allow any origin  
    //.AllowCredentials());               // allow credentials 
    // Configure the HTTP request pipeline.
    //if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    // grpc web
    // Configure the HTTP request pipeline.
    app.UseRouting();
    app.UseGrpcWeb();
    app.UseCors();

    //GRPC Native
    //app.MapGrpcService<GrpcNgajiService>();
    //app.MapGrpcService<GrpcAuthService>();
    //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    //app.MapGrpcHealthChecksService();

    //GRPC WEB
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<GrpcGatewayService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcDeviceService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcStreamRawDataService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcUserProfileService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcAuthService>().EnableGrpcWeb().RequireCors("AllowAll");
      
       




        endpoints.MapGrpcHealthChecksService().EnableGrpcWeb().RequireCors("AllowAll");

        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        });
    });


}

void ConfigureRouting(WebApplication app)
{

    var summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

}
internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}