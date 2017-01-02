using System;
using System.Net.NetworkInformation;

namespace WinSPCheck.Internal
{
    /// <summary>
    /// </summary>
    public class PingDevice : IPingDevice
    {
        /// <inheritdoc />
        public IPStatus ValueFor(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            try
            {
                var reply = new Ping().Send(address);
                if (reply != null)
                {
                    return reply.Status;
                }
            }
            catch (PingException)
            {
                Console.Write(@"Destination Host or Network Unreachable");
            }
            return IPStatus.Unknown;
        }
    }
}