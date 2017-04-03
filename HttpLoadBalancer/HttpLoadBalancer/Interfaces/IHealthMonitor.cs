using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Model;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Interfaces
{
    public interface IHealthMonitor
    {
        void UpdateServerStatus(List<Server> servers);
        bool IsHealthy(Server server);
        Server PickServer(List<Server> servers);
    }
}
