using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrowser.Library
{
    public class Network
    {
        public static bool IsNetworkAvaliable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public static bool IsVPNActive()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface Interface in interfaces)
                {
                    if (Interface.OperationalStatus == OperationalStatus.Up)
                    {
                        if ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
