using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service.HealthMonitors
{
    public class ResponsetimeMonitor : IHealthMonitor
    {
        public Server PickServer(List<Server> servers)
        {
            var times = servers.Select(server => Ping(server.Address)).ToList();
            return servers[times.IndexOf(times.Min())];
        }

        public void UpdateServerStatus(List<Server> servers)
        {
            foreach (var server in servers)
            {
                server.Status = Ping(server.Address) > 0 ? Status.Online : Status.Offline;
            }
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

        public bool IsHealthy(Server server)
        {
            var time = Ping(server.Address);
            server.Status = time > 0 ? Status.Online : Status.Offline;
            return time > 0;
        }
    }
}