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
        public Server PickServer(List<Server> servers)
        {
            var times = servers.Select(async server => await Ping(server)).ToList();
            return servers[times.IndexOf(times.Min())];
        }

        public async void UpdateServerStatus(List<Server> servers)
        {
            foreach (var server in servers)
            {
                server.Status = IsHealthy(server) ? Status.Online : Status.Offline;
            }
        }

        public async Task<long> Ping(Server server)
        {
            var message = "/ HTTP/1.1\r\n" +
                          $"Host: {server.Address}\r\n" +
                          "Connection: keep-alive\r\n" +
                          "Content-Length: 0\r\n";
            return 1;
        }

        public static byte[] ToRequest(HttpMessage message)
        {
            var request = "";
            var statusLine = new List<string> { "Method", "Url", "HttpVersion" };
            foreach (var prop in message.Properties)
            {
                if (statusLine.Contains(prop.Key))
                {

                    if (prop.Key == "HttpVersion")
                        request += $"{prop.Value}\r\n";
                    else
                        request += $"{prop.Value} ";
                }
                else if (prop.Key == "Body")
                {
                    request += "\r\n";
                    request += prop.Value.Trim().Replace("\0", "");
                }
                else
                {
                    request += $"{prop.Key}: {prop.Value.Trim()}\r\n";
                }
            }
            return Encoding.ASCII.GetBytes(request);
        }

        public bool IsHealthy(Server server)
        {
            try
            {
                var x = new TcpClient();
                x.Connect(server.Address, server.Port);
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}