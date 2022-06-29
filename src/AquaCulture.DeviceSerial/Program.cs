using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Modbus;
using GHIElectronics.TinyCLR.Devices.Modbus.Interface;
using GHIElectronics.TinyCLR.Devices.Network;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Pins;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace AquaCulture.DeviceSerial
{
    internal class Program
    {
        static int DelayTime = 1000;
        static string DeviceId = "device-001";
        static MqttClient client;
        static void Main()
        {
            StartEthernet();
            client = new MqttClient();
            client.Connect();
            Thread th1 = new Thread(new ThreadStart(StartReading));
            th1.Start();
            Thread.Sleep(Timeout.Infinite);

        }
        static void StartReading()
        {

            var serial = UartController.FromName(SC20260.UartPort.Uart5);

            var uartSetting = new UartSetting()
            {
                BaudRate = 19200,
                DataBits = 8,
                Parity = UartParity.None,
                StopBits = UartStopBitCount.One,
                Handshaking = UartHandshake.None,
            };

            serial.SetActiveSettings(uartSetting);

            serial.Enable();

            IModbusInterface mbInterface;
            mbInterface = new ModbusRtuInterface(
                serial,
                19200,
                8,
                UartStopBitCount.One,
                UartParity.None);

            ModbusMaster mbMaster;
            mbMaster = new ModbusMaster(mbInterface);

            var mbTimeout = false;

            ushort[] registers = null;
            //int count = 0;
            byte[] coils;
            while (true)
            {
                try
                {
                    mbTimeout = false;

                    registers = mbMaster.ReadHoldingRegisters(10, 0, 1, 3333);
                    coils = mbMaster.ReadCoils(10, 0, 2, 3333);

                    client.SendTelemetry(DeviceId, coils, registers);
                    //count++;

                    //if (count == 5)
                    //    break;
                }
                catch (System.Exception error)
                {
                    Debug.WriteLine("Modbus Timeout");
                    mbTimeout = true;
                }

                if (!mbTimeout)
                {
                    Debug.WriteLine("Modbus : " + (object)registers[0].ToString());
                }

                Thread.Sleep(DelayTime);
            }
        }
        #region network
        static bool linkReady = false;

        static void StartEthernet()
        {
            //Reset external phy.
            var gpioController = GpioController.GetDefault();
            var resetPin = gpioController.OpenPin(SC20260.GpioPin.PG3);

            resetPin.SetDriveMode(GpioPinDriveMode.Output);

            resetPin.Write(GpioPinValue.Low);
            Thread.Sleep(100);

            resetPin.Write(GpioPinValue.High);
            Thread.Sleep(100);

            var networkController = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings();

            var networkCommunicationInterfaceSettings = new BuiltInNetworkCommunicationInterfaceSettings();

            networkInterfaceSetting.Address = new IPAddress(new byte[] { 192, 168, 1, 122 });
            networkInterfaceSetting.SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 });
            networkInterfaceSetting.GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 });

            networkInterfaceSetting.DnsAddresses = new IPAddress[]
                {new IPAddress(new byte[] { 75, 75, 75, 75 }),
         new IPAddress(new byte[] { 8, 8, 8, 8 })};

            networkInterfaceSetting.MacAddress = new byte[] { 0x00, 0x4, 0x00, 0x00, 0x00, 0x00 };
            networkInterfaceSetting.DhcpEnable = true;
            networkInterfaceSetting.DynamicDnsEnable = true;

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.SetCommunicationInterfaceSettings(networkCommunicationInterfaceSettings);

            networkController.SetAsDefaultController();

            networkController.NetworkAddressChanged += NetworkController_NetworkAddressChanged;
            networkController.NetworkLinkConnectedChanged += NetworkController_NetworkLinkConnectedChanged;

            networkController.Enable();

            while (linkReady == false) ;
            Debug.WriteLine("Network is ready to use");
            //Thread.Sleep(Timeout.Infinite);
        }

        private static void NetworkController_NetworkLinkConnectedChanged
            (NetworkController sender, NetworkLinkConnectedChangedEventArgs e)
        {
            //Raise connect/disconnect event.
        }

        private static void NetworkController_NetworkAddressChanged
            (NetworkController sender, NetworkAddressChangedEventArgs e)
        {

            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            linkReady = address[0] != 0;
            Debug.WriteLine("IP: " + address[0] + "." + address[1] + "." + address[2] + "." + address[3]);
        }
        #endregion
    }


}
