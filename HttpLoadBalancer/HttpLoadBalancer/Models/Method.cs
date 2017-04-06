using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HttpLoadBalancer.Models
{
    public abstract class Method
    {
        public string Name { get; set; }
        public virtual async Task<Server> GetServer(List<Server> servers)
        {
            throw new System.NotImplementedException();
        }
    }
}