using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoadBalancer.Models
{
    public class Cookie
    {
        public Cookie(Server selectedServer, string expiresValue)
        {
            Server = selectedServer;
            Expires = expiresValue;
        }

        public Server Server { get; set; }
        public string Expires { get; set; }
    }
}
