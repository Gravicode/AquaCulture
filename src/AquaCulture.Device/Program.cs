using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Modbus;
using GHIElectronics.TinyCLR.Devices.Modbus.Interface;
using GHIElectronics.TinyCLR.Devices.Network;
using GHIElectronics.TinyCLR.Pins;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace AquaCulture.Device
{
    internal class Program
    {
        static void Main()
        {
            StartEthernet();
            ModbusDevice ModbusTCP_Device;
            ModbusTCP_Device = new ModbusClient(248);
            ModbusTcpListener mbListner;
            mbListner = new ModbusTcpListener(ModbusTCP_Device, 502, 5, 1000);
            Thread.Sleep(100);
            ModbusTCP_Device.Start();
            Thread.Sleep(Timeout.Infinite);
        }
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
         new IPAddress(new byte[] { 75, 75, 75, 76 })};

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


    }
    public class ModbusClient : ModbusDevice
    {
        public ModbusClient(byte deviceAddress, object syncObject = null)
           : base(deviceAddress, syncObject) { }

        protected override string OnGetDeviceIdentification(ModbusObjectId objectId)
        {
            switch (objectId)
            {
                case ModbusObjectId.VendorName:
                    return "GHI Electronics";
                case ModbusObjectId.ProductCode:
                    return "101";
                case ModbusObjectId.MajorMinorRevision:
                    return "1.0";
                case ModbusObjectId.VendorUrl:
                    return "ghielectronics.com";
                case ModbusObjectId.ProductName:
                    return "SitCore";
                case ModbusObjectId.ModelName:
                    return "SCM20260D";
                case ModbusObjectId.UserApplicationName:
                    return "Modbus Slave Test";
            }
            return null;
        }

        protected override ModbusConformityLevel GetConformityLevel()
        {
            return ModbusConformityLevel.Regular;
        }

        protected override ModbusErrorCode OnReadCoils(bool isBroadcast, ushort startAddress, ushort coilCount, byte[] coils)
        {
            try
            {
                
                for (int n = 0; n < coils.Length; ++n)
                {
                    coils[n] = 1;
                }
                Debug.WriteLine("Master read coils");
                return ModbusErrorCode.NoError;
            }
            catch
            {
                Debug.WriteLine("error in on read coils registers");
                return base.OnReadCoils(isBroadcast, startAddress, coilCount, coils);
            }
        }

        protected override ModbusErrorCode OnReadHoldingRegisters(bool isBroadcast, ushort startAddress, ushort[] registers)
        {
            try
            {
                ushort counter = 1;
                for (int n = 0; n < registers.Length; ++n)
                {
                    registers[n] = counter++; // set number in each register for testing               
                }
                Debug.WriteLine("Master Read Holding Registers - " + registers[0].ToString());
                return ModbusErrorCode.NoError;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error in on read holding registers");
                return base.OnReadHoldingRegisters(isBroadcast, startAddress, registers);
            }
        }

    }
}
