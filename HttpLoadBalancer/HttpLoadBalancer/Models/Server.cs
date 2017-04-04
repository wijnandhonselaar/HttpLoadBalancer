using System.Net.Sockets;

namespace HttpLoadBalancer.Models
{
    public class Server
    {
        public Server(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public string Address { get; set; }

        public int Port { get; set; }

        public Status Status { get; set; }
    }
}