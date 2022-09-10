using System.Threading;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TlsClient
{
    internal class Program
    {
        static bool _linkReady = false;

        public static NetworkController Controller { get; private set; }

        static void Main()
        {
            var resetPin = GpioController.GetDefault().OpenPin(SC20260.GpioPin.PG3);
            resetPin.SetDriveMode(GpioPinDriveMode.Output);
            resetPin.Write(GpioPinValue.High);

            Controller = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);
            Controller.SetCommunicationInterfaceSettings(new BuiltInNetworkCommunicationInterfaceSettings());
            Controller.SetInterfaceSettings(new EthernetNetworkInterfaceSettings()
            {
                MacAddress = new byte[6] { 0x00, 0x8D, 0xB4, 0x49, 0xAD, 0xBD }
            });
            Controller.SetAsDefaultController();

            Controller.NetworkAddressChanged += NetworkController_NetworkAddressChanged;
            Controller.Enable();

            while (_linkReady == false);

            var tlsClient = new TlsClient();
            tlsClient.Connect("https://api.coindesk.com/v1/bpi/currentprice.json");
        }

        private static void NetworkController_NetworkAddressChanged
            (NetworkController sender, NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            _linkReady = address[0] != 0;
            Debug.WriteLine("IP: " + address[0] + "." + address[1] + "." + address[2] + "." + address[3]);
        }
    }
}
