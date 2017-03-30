using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;

namespace HttpLoadBalancer.Models.HealthMonitors
{
    public class ResponsetimeMonitor : IHealthMonitor
    {
        public bool IsHealthy(Server server)
        {
            var tcpClient = new TcpClient(server.Address, server.Port);
            return true;
        }
    }
}
