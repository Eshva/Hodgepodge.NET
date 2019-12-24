#region Usings

using System;
using System.Linq;
using System.Net.NetworkInformation;

#endregion


namespace Eshva.Common.UnitTesting
{
    public static class FreeTcpPorts
    {
        public static int[] GetPorts(int portsCount = 1)
        {
            if (portsCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(portsCount));
            }

            const int FirstDynamicPortNumber = 49152;
            const int MaxPortNumber = 65535;
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            var tcpConnectionPorts = ipGlobalProperties.GetActiveTcpConnections()
                                                       .Where(n => n.LocalEndPoint.Port >= FirstDynamicPortNumber)
                                                       .Select(n => n.LocalEndPoint.Port);

            var tcpListenerPorts = ipGlobalProperties.GetActiveTcpListeners()
                                                     .Where(n => n.Port >= FirstDynamicPortNumber)
                                                     .Select(n => n.Port);

            var udpListenerPorts = ipGlobalProperties.GetActiveUdpListeners()
                                                     .Where(n => n.Port >= FirstDynamicPortNumber)
                                                     .Select(n => n.Port);

            return Enumerable.Range(FirstDynamicPortNumber, MaxPortNumber)
                             .Where(
                                 port => !tcpConnectionPorts.Contains(port) &&
                                         !tcpListenerPorts.Contains(port) &&
                                         !udpListenerPorts.Contains(port))
                             .Take(portsCount)
                             .ToArray();
        }
    }
}
