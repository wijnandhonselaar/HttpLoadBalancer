using System.Net.Sockets;
using System.Threading.Tasks;
using HttpLoadBalancer.Service;

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

        public async Task<Status> UpdateStatus()
        {
            Status = await MethodService.Monitor.IsHealthy(this) ? Status.Online : Status.Offline;
            return Status;
        }
    }
}