using AquaCulture.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AquaCulture.Gateway
{
    public class MessageEventArgs:EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
    }
    public class MqttService
    {
        public EventHandler<MessageEventArgs> MessageReceived { set; get; }
        public MqttService()
        {
            SetupMqtt();
        }
        MqttClient MqttClient;
        readonly string? DataTopic = ConfigurationManager.AppSettings["DataTopic"];
        readonly string? ControlTopic = ConfigurationManager.AppSettings["ControlTopic"];

        bool _subscribed;
        public bool IsSubscribed
        {
            get
            {
                return _subscribed;
            }
            set
            {
                _subscribed = value;
                if (_subscribed)
                {
                    MqttClient.Subscribe(new string[] { ControlTopic, DataTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                }
                else
                {
                    MqttClient.Unsubscribe(new string[] { ControlTopic, DataTopic });
                }
            }
        }
        public void PublishMessage(string Message)
        {
            MqttClient.Publish(DataTopic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void SendCommand(string Message)
        {
            MqttClient.Publish(ControlTopic, Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        void SetupMqtt()
        {
            string? IPBrokerAddress = ConfigurationManager.AppSettings["MqttHost"];
            string? ClientUser = ConfigurationManager.AppSettings["MqttUser"];
            string? ClientPass = ConfigurationManager.AppSettings["MqttPass"];


            MqttClient = new MqttClient(IPBrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            MqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            
            // use a unique id as client id, each time we start the application
            var clientId = Guid.NewGuid().ToString();
            
            MqttClient.Connect(clientId, ClientUser, ClientPass);
            Console.WriteLine("MQTT is connected");
        } // this code runs when a message was received
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            MessageReceived?.Invoke(this, new MessageEventArgs { Topic = e.Topic, Message = ReceivedMessage });
            //if (e.Topic == ControlTopic)
            //{
            //    Console.WriteLine(ReceivedMessage);
            //}
        }
        // Invoke the direct method on the device, passing the payload
        public Task SendTelemetry(string DeviceId,string GatewayId,  bool[] Coils, int[] Registers)
        {
            Console.WriteLine($"ID -> {DeviceId}");
            return Task.Factory.StartNew(() => {
                var action = new DeviceTwin() { DeviceId = DeviceId, GatewayId=GatewayId,  Coils= Coils, Registers = Registers};

                SendCommand(JsonSerializer.Serialize(action));
            });

            //Console.WriteLine("Response status: {0}, payload:", response.Status);
            //Console.WriteLine(response.GetPayloadAsJson());
        }


    }

  
}
