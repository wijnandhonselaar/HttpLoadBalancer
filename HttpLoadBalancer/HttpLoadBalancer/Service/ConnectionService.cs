using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Models.HealthMonitors;

namespace HttpLoadBalancer.Service
{
    public class ConnectionService
    {
        const int BufferSize = 2048;

        public List<Server> Servers = new List<Server>();
        public Server SelectedServer;

        public async Task HandleRequest(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var request = await ReceiveRequest(stream);
                if (request != null && IsValidRequest(request))
                {
                    var responseStream = await SendeRequest(request);
                    var i = 0;

                    while (!responseStream.DataAvailable)
                    {
                        i++;
                        if (i > 5) break;
                        Thread.Sleep(50);
                    }
                    var response = await GetResponse(responseStream);
                    SendResponse(stream, response);
                }
            }
        }

        public List<Server> GetDefaultServers()
        {
            return new List<Server>
            {
                new Server("server4.tezzt.nl", 8081),
                new Server("server4.tezzt.nl", 8082),
                new Server("server4.tezzt.nl", 8083),
                new Server("server4.tezzt.nl", 8084)
            };
        }
    
        private bool IsValidRequest(HttpMessage request)
        {
            var message = Encoding.ASCII.GetString(request.Original).Replace("\0", "");
            return message.Length > 0;
        }

        private async Task<HttpMessage> ReceiveRequest(NetworkStream stream)
        {
            if (!stream.DataAvailable) return null;
            var buffer = new byte[BufferSize];
            await stream.ReadAsync(buffer, 0, BufferSize);
            var request = new HttpMessage(buffer);
            return request;
        }

        private async Task<NetworkStream> SendeRequest(HttpMessage request)
        {
            // httpMessage for session persistence (cookie)
            SelectedServer = MethodService.CurrentMethod.GetServer(Servers);
            HttpMapper.SetUrl(request, SelectedServer);
            var serverClient = new TcpClient();
            serverClient.Connect(SelectedServer.Address, SelectedServer.Port);
            var serverStream = serverClient.GetStream();
            var requestArray = HttpMapper.ToRequest(request);
            await serverStream.WriteAsync(requestArray, 0, requestArray.Length, CancellationToken.None);
            return serverStream;
        }

        private async Task<HttpMessage> GetResponse (NetworkStream stream)
        {
            var buffer = new byte[BufferSize];
            HttpMessage response = null;
            if (!stream.DataAvailable) return response;
            try
            {
                await stream.ReadAsync(buffer, 0, BufferSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw(e);
            }
            response = new HttpMessage(buffer, true);
            return response;
        }

        private void SendResponse(NetworkStream stream, HttpMessage message)
        {
            stream.Write(message.Original, 0 , message.Original.Length);
        }

        public Server AddServer(string address, int port)
        {
            var server = new Server(address, port);
            Servers.Add(server);
            return server;
        }

        public bool RemoveServer(Server server) => Servers.Remove(server);
    }
}