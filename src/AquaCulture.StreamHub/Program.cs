using AquaCulture.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace AquaCulture.StreamHub
{
    public class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }
        static IClusterClient client;
        private static async Task<int> RunMainAsync()
        {
            try
            {
                
                var GatewayIDs = ConfigurationManager.AppSettings["GatewayIDs"].Split(";");

                client = await ConnectClient();

                var mqtt = new MqttService();
                mqtt.MessageReceived += async(e, o) => {
                    Console.WriteLine(o.Topic + " => " + o.Message);
                    var item = JsonSerializer.Deserialize<DeviceTwin>(o.Message);
                    var data = new StreamRawData() { CreatedDate = DateTime.Now, DeviceId = item.DeviceId, GatewayId = item.GatewayId, RawData = o.Message };
                    await SendData(client, data);
                };
                mqtt.IsSubscribed = true;
                
                Console.ReadKey();
                
                client.Dispose();

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "AquaCulture";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task SendData(IClusterClient client, StreamRawData item)
        {
            // example of calling grains from the initialized client
            var friend = client.GetGrain<IGatewayGrain>(item.GatewayId);
            var response = await friend.SendRawData(item);
            Console.WriteLine($"\n\nResult:{response}\n\n");
        }
    }
}
