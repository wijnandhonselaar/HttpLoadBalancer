using System.Collections.Generic;
using System.Net.Sockets;

namespace HttpLoadBalancer.Models
{
    public abstract class Method
    {
        public string Name { get; set; }
        public virtual Server GetServer(List<Server> servers)
        {
            throw new System.NotImplementedException();
        }
    }
}