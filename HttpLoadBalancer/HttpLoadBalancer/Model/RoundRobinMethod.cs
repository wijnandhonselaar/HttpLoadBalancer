using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoadBalancer.Model
{
    public class RoundRobinMethod : Method
    {
        public override HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            return base.ProcessRequest(message, httpMessage);
        }
    }
}
