using System.Net.Sockets;
using HttpLoadBalancer.Model;

namespace HttpLoadBalancer.Models
{
    public class Server
    {
        public Server(TcpClient client, string name, string address, Status status)
        {
            Client = client;
            Name = name;
            Address = address;
            Status = status;
        }

        public TcpClient Client { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Status Status { get; set; }
}
}