using System.Net.Sockets;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Model.Methods
{
    public class RoundRobinMethod : Method
    {
        public RoundRobinMethod(string name) : base(name)
        {
        }

        public override HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            return new HttpMessage("");
        }
    }

    
}
