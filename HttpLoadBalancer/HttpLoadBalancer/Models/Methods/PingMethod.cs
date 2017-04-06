using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Service;

namespace HttpLoadBalancer.Models.Methods
{
    public class PingMethod : Method
    {
        public PingMethod()
        {
            Name = "Ping";
        }

        /// <summary>
        /// Get the next server in line
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override async Task<Server> GetServer(List<Server> servers)
        {
            var times = new List<long>();
            foreach (var s in servers)
            {
                times.Add(await Ping(s));
            }
            var server = servers[times.IndexOf(times.Min())];
            return server;
        }

        /// <summary>
        /// Gets the time it takes between sending the request and receiving the last byte
        /// </summary>
        /// <param name="server"></param>
        /// <returns>int:ping in MS</returns>
        public async Task<long> Ping(Server server)
        {
            var message = "HEAD / HTTP/1.1\r\n" +
                          $"Host: {server.Address}\r\n" +
                          "Connection: keep-alive\r\n" +
                          "Content-Length: 0\r\n\r\n";
            var serverClient = new TcpClient(server.Address, server.Port);
            var bytes = Encoding.ASCII.GetBytes(message);

            var sentTime = DateTime.Now;
            serverClient.GetStream().Write(bytes, 0, bytes.Length);

            Array.Clear(bytes, 0, bytes.Length);
            bytes = new byte[2048];
            await serverClient.GetStream().ReadAsync(bytes, 0, bytes.Length);
            var receiveTime = DateTime.Now;
            TimeSpan span = receiveTime - sentTime;
            return (int)span.TotalMilliseconds;
        }
    }
}
