using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using HttpLoadBalancer.Service;

namespace HttpLoadBalancer.Models.Methods
{
    public class PingMethod : Method
    {
        public PingMethod()
        {
            Name = "Ping";
        }

        /// <summary>
        /// Get the next server in line
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override Server GetServer(List<Server> servers)
        {
            var times = servers.Select(s => Ping(s.Address)).ToList();
            var server = servers[times.IndexOf(times.Min())];
            return server;
        }

        public long Ping(string address)
        {
            var x = new Ping();
            var reply = x.Send(address);
            long pingTime = 0;
            if (reply?.Status == IPStatus.Success)
            {
                pingTime = reply.RoundtripTime;
            }
            return pingTime;
        }
    }   
}
