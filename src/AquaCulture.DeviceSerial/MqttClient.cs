using GHIElectronics.TinyCLR.Data.Json;
using GHIElectronics.TinyCLR.Networking.Mqtt;
using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace AquaCulture.DeviceSerial
{
    public class MqttMessageReceivedEventArgs : EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
    }
    public class MqttClient
    {
        public delegate void MessageReceivedHandler(object sender, MqttMessageReceivedEventArgs e);
        public event MessageReceivedHandler MessageReceived;

        string topic = "aqua/data";
        Mqtt client;
        public MqttClient()
        {
            

        }

        public void Connect()
        {
            var caCertificate = new X509Certificate(UTF8Encoding.UTF8.GetBytes("Your certificate"));

            var mqttHost = "broker.emqx.io";
            var mqttPort = 8883; //default 8883
            var deviceId = "sensor-device-001";

            var username = "device-001";
            var password = "mqtt_pass";
           

            try
            {
                var clientSetting = new MqttClientSetting
                {
                    BrokerName = mqttHost,
                    BrokerPort = mqttPort,
                    ClientCertificate = null,
                    CaCertificate = caCertificate,
                    SslProtocol = System.Security.Authentication.SslProtocols.Tls12
                };

                client = new Mqtt(clientSetting);

                var connectSetting = new MqttConnectionSetting
                {
                    ClientId = deviceId,
                    UserName = username,
                    Password = password
                };

                // Connect to host
                var returnCode = client.Connect(connectSetting);

                var packetId = 1;

                // Subscribe to a topic
                client.Subscribe(new string[] { topic }, new QoSLevel[] { QoSLevel.ExactlyOnce },
                    (ushort)packetId++);

               

                // Publish recieved change from a specific topic
                client.PublishReceivedChanged += (object sender, string topic, byte[] data, bool duplicate, QoSLevel qosLevel, bool retain) => {
                    var message = Encoding.UTF8.GetString(data);
                    if (MessageReceived != null)
                    {
                        MessageReceived.Invoke(this, new MqttMessageReceivedEventArgs() { Message = message, Topic = topic });
                    }
                };

                client.PublishedChanged += (a, b, c) => { Debug.WriteLine("Published Changed."); };
                client.SubscribedChanged += (a, b, c) => { Debug.WriteLine("Subscribed Changed."); };
                client.ConnectedChanged += (a) => { Debug.WriteLine("Connected Changed."); };
                client.UnsubscribedChanged += (a, b) => { Debug.WriteLine("Unsubscribed Changed."); };


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public void SendTelemetry(string DeviceId, byte[] Coils, ushort[] Registers)
        {
            var action = new DeviceTwin() { DeviceID = DeviceId, Coils = Coils, Registers = Registers };
            var message = JsonConverter.Serialize(action);
            var packetId = 1;
            client.Publish(topic, Encoding.UTF8.GetBytes(message.ToString()), QoSLevel.MostOnce, false, (ushort)packetId++);
        }
       
    }

    public class DeviceTwin
    {
        public string DeviceID { get; set; }
        public byte[] Coils { get; set; }
        public ushort[] Registers { get; set; }
    }
}
