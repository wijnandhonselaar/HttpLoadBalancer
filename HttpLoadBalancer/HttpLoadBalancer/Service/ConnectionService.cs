using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Models.HealthMonitors;
using Cookie = HttpLoadBalancer.Models.Cookie;

namespace HttpLoadBalancer.Service
{
    public class ConnectionService
    {
        const int BufferSize = 65536;

        public List<Server> Servers = new List<Server>();
        public Server SelectedServer;
        public Dictionary<string, Cookie> Sessions = new Dictionary<string, Cookie>();

        public async Task HandleRequest(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var request = await ReceiveRequest(stream);
                if (request != null && IsValidRequest(request))
                {
                    var responseStream = await SendeRequest(request);
                    if (request.Properties["Url"] == "{[Url, /favicon.ico]}") return;
                    var i = 0;

                    while (!responseStream.DataAvailable)
                    {
                        i++;
                        if (i > 5) break;
                        Thread.Sleep(50);
                    }
                    if (responseStream.DataAvailable)
                    {
                        var response = await GetResponse(responseStream);
                        SetCookies(response);
                        if (response != null) SendResponse(stream, response);
                    }
                }
            }
        }

        // TODO create session persistence method
        private void SetCookies(HttpMessage response)
        {
            if (!response.Properties.ContainsKey("set-cookie")) return;
            var value = response.Properties["set-cookie"];
            var split = value.Split(';');
            string key = null;
            string expires = null;
            foreach (var item in split)
            {
                if (item.Contains("connect.sid"))
                {
                    key = item.Split('=')[1];
                }
                if (item.Contains("Expires"))
                {
                    expires = item.Split('=')[1];
                }
            }
            if (key == null) return;
            if (!Sessions.ContainsKey(key)) Sessions.Add(key, new Cookie(SelectedServer, expires));
        }

        public Server GetServerFromCookie(HttpMessage request)
        {
            if (!request.Properties.ContainsKey("Cookie")) return null;
            var value = request.Properties["Cookie"];
            var split = value.Split(';');
            string key = null;
            foreach (var item in split)
            {
                if (item.Contains("connect.sid"))
                {
                    key = item.Split('=')[1];
                }
            }
            if (key == null) return null;
            return Sessions.ContainsKey(key) ? Sessions[key].Server : null;
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
            SelectedServer = GetServerFromCookie(request);
            if(SelectedServer == null) SelectedServer = MethodService.CurrentMethod.GetServer(Servers);
            HttpMapper.SetUrl(request, SelectedServer);
            var serverClient = new TcpClient();
            serverClient.Connect(SelectedServer.Address, SelectedServer.Port);
            var serverStream = serverClient.GetStream();
            var requestArray = HttpMapper.ToRequest(request);
            await serverStream.WriteAsync(requestArray, 0, requestArray.Length);
            return serverStream;
        }

        private async Task<HttpMessage> GetResponse (NetworkStream stream)
        {
            byte[] buffer = new byte[BufferSize];
            Thread.Sleep(100);
            try
            {
                await stream.ReadAsync(buffer, 0, BufferSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw(e);
            }
            return new HttpMessage(buffer, true);
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