using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service.HealthMonitors
{
    public class ResponsetimeMonitor : IHealthMonitor
    {
        public async void UpdateServerStatus(List<Server> servers)
        {
            foreach (var server in servers)
            {
                server.Status = await IsHealthy(server) ? Status.Online : Status.Offline;
            }
        }

        /// <summary>
        /// Gets the time it takes between sending the request and receiving the last byte
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<long> Ping(Server server)
        {
            var message = "HEAD / HTTP/1.1\r\n" +
                            $"Host: {server.Address}\r\n" +
                            "Connection: keep-alive\r\n" +
                            "Content-Length: 0\r\n\r\n";
            var serverClient = new TcpClient(server.Address, server.Port);
            var bytes = Encoding.ASCII.GetBytes(message);
            try
            {
                var sentTime = DateTime.Now;
                serverClient.GetStream().Write(bytes, 0, bytes.Length);

                Array.Clear(bytes, 0, bytes.Length);
                bytes = new byte[2048];
                await serverClient.GetStream().ReadAsync(bytes, 0, bytes.Length);
                var receiveTime = DateTime.Now;
                TimeSpan span = receiveTime - sentTime;
                return (int)span.TotalMilliseconds;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> IsHealthy(Server server)
        {
            try
            {
                return await Ping(server) > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}