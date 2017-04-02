using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;

namespace HttpLoadBalancer.Models.HealthMonitors
{
    public class ResponsetimeMonitor : IHealthMonitor
    {
        public Server PickServer(List<Server> servers)
        {
            var times = servers.Select(server => ping(server.Address)).ToList();
            return servers[times.IndexOf(times.Min())];
        }

        public void UpdateServerStatus(List<Server> servers)
        {
            foreach (var server in servers)
            {
                server.Status = ping(server.Address) > 0 ? Status.Online : Status.Offline;
            }
        }

        private long ping(string address)
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

        public bool IsHealthy(string address)
        {
            return ping(address) > 0;
        }
    }
}