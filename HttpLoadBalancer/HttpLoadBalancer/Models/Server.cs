using System.Net.Sockets;
using HttpLoadBalancer.Model;

namespace HttpLoadBalancer.Models
{
    public class Server
    {
        public Server(TcpClient client, string address, Status status)
        {
            throw new System.NotImplementedException();
        }

        public TcpClient Client
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public string Address
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public Status Status
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}