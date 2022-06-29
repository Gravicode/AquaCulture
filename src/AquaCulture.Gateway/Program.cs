using AquaCulture.Gateway;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        int DelayTime = 1000;
        var mqtt = new MqttService();
        mqtt.MessageReceived += (e, o) => {
            Console.WriteLine(o.Topic +" => "+ o.Message);
        };
        mqtt.IsSubscribed = true;
        int.TryParse(ConfigurationManager.AppSettings["Delay"],out DelayTime);
        var DeviceID = ConfigurationManager.AppSettings["DeviceID"];
        var IPModbusDevice = ConfigurationManager.AppSettings["IPModbusDevice"];
        EasyModbus.ModbusClient modbusClient = new EasyModbus.ModbusClient(IPModbusDevice, 502);
        //Increase the Connection Timeout to 5 seconds
        modbusClient.ConnectionTimeout = 5000;
        //We create a Log File. This will also active the Event logging 
        modbusClient.LogFileFilename = "test.txt";
        //The Messages sent to the MQTT-Broker will be retained in the Broker. After subscribtion, the client will automatically
        //receive the last retained message. By default the Retain-Flag is FALSE -> Messages will not be retained
        //modbusClient.MqttRetainMessages = true;
        //Connect to the ModbusTCP Server
        modbusClient.Connect();
        while (true)
        {
            // We read two registers from the Server, and Publish them to the MQTT-Broker. By default Values will be published
            // on change By default we publish to the Topic 'easymodbusclient/' and the the address e.g. ''easymodbusclient/holdingregister1'
            // The propery "Password" and "Username" can be used to connect to a Broker which require User and Password. By default the 
            //MQTT-Broker port is 1883
            Console.WriteLine("read holding registers");
            int[] holdingRegister = modbusClient.ReadHoldingRegisters(60, 10);
            foreach(var num in holdingRegister)
                Console.WriteLine($"read = {num}");
            Console.WriteLine("read coils");
            bool[] coils = modbusClient.ReadCoils(10, 10);
            foreach(var num in coils)
                Console.WriteLine($"read = {num}");
            mqtt.SendTelemetry(DeviceID, coils, holdingRegister);
            System.Threading.Thread.Sleep(DelayTime);
        }
        modbusClient.Disconnect();
        Console.ReadKey();
        

    }
}