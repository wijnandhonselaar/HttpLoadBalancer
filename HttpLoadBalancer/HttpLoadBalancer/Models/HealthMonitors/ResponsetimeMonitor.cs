using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;

namespace HttpLoadBalancer.Models.HealthMonitors
{
    public class ResponsetimeMonitor : IHealthMonitor
    {
        public bool IsHealthy(Server server)
        {
            throw new NotImplementedException();
        }
    }
}
