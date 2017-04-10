using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpLoadBalancer.Models;
using Cookie = HttpLoadBalancer.Models.Cookie;

namespace HttpLoadBalancer.Service
{
    public class ConnectionService
    {
        const int BufferSize = 65536;

        public Server SelectedServer;
        public Dictionary<string, Cookie> Sessions = new Dictionary<string, Cookie>();
        public bool SessionsEnabled = true;

        public async Task HandleRequest(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var request = await ReceiveRequest(stream);
                if (request != null && IsValidRequest(request))
                {
                    var responseStream = await SendeRequest(request);
                    if (responseStream == null)
                    {
                        ServerUnavailable(stream);
                        return;
                    }
                    var i = 0;
                    // If needed, wait for the response
                    while (!responseStream.DataAvailable)
                    {
                        i++;
                        if (i > 10) break;
                        Thread.Sleep(100);
                    }
                    if (responseStream.CanRead && responseStream.DataAvailable)
                    {
                        var response = await GetResponse(responseStream);
                        SessionService.SaveSession(response, SelectedServer);
                        if (response != null) SendResponse(stream, response);
                    }
                }
            }
        }
    
        /// <summary>
        /// Returns false if the request is empty
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsValidRequest(HttpMessage request)
        {
            var message = Encoding.ASCII.GetString(request.Original).Replace("\0", "");
            return message.Length > 0;
        }

        private async Task<HttpMessage> ReceiveRequest(NetworkStream stream)
        {
            var i = 0;
            // If needed, wait for the response
            while (!stream.DataAvailable)
            {
                i++;
                if (i > 10) break;
                Thread.Sleep(100);
            }
            if (!stream.DataAvailable) return null;
            var buffer = new byte[BufferSize];
            await stream.ReadAsync(buffer, 0, BufferSize);
            var request = new HttpMessage(buffer);
            return request;
        }

        private async Task<NetworkStream> SendeRequest(HttpMessage request)
        {
            // httpMessage for session persistence (cookie)
            // Has session
            if (SessionsEnabled && SessionService.SessionManager.HasSession(request))
            {
                // has Valid session
                if (SessionService.SessionManager.HasValidSession(request))
                {
                    SelectedServer = SessionService.GetServerFromSession(request);
                }
                else // has no valid session, return 503 Service Unavailable 
                {
                    return null;
                }
            }
            else // has no session, use load-balancer to pick a server
            {
                SelectedServer = await MethodService.CurrentMethod.GetServer(SessionService.Servers);
            }
            var serverClient = new TcpClient();
            serverClient.Connect(SelectedServer.Address, SelectedServer.Port);
            var serverStream = serverClient.GetStream();
            HttpMapper.SetUrl(request, SelectedServer);
            var requestArray = HttpMapper.ToRequest(request);
            await serverStream.WriteAsync(requestArray, 0, requestArray.Length);
            return serverStream;
        }

        /// <summary>
        /// Writes 503 to stream if server from cookie could not be reached
        /// </summary>
        /// <param name="stream"></param>
        private void ServerUnavailable(NetworkStream stream)
        {
            var head = "HTTP/1.1 503 server unavailable\r\nContent-Length: 0\r\n\r\n";
            var errorBuffer = Encoding.ASCII.GetBytes(head);
            stream.Write(errorBuffer, 0, errorBuffer.Length);
        }

        private async Task<HttpMessage> GetResponse (Stream stream)
        {
            byte[] buffer = new byte[BufferSize];
            await stream.ReadAsync(buffer, 0, BufferSize);
            return new HttpMessage(buffer, true);
        }

        private void SendResponse(NetworkStream stream, HttpMessage message)
        {
            var result = HttpMapper.GetHead(message);
            var resultBuffer = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBuffer, 0 , resultBuffer.Length);
        }

        public Server AddServer(string address, int port)
        {
            var server = new Server(address, port);
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    tcpClient.Connect(server.Address, server.Port);
                }
            }
            catch
            {
                return null;
            }
            SessionService.AddServer(server);
            return server;
        }

        public List<Server> GetDefaultServers()
        {
            return new List<Server>
            {
                new Server("joelchrist.nl", 8081),
                new Server("joelchrist.nl", 8082),
                new Server("joelchrist.nl", 8083),
                new Server("joelchrist.nl", 8084)
            };
        }

        public bool RemoveServer(Server server) => SessionService.RemoveServer(server);
        public bool RemoveServer(string address, int port) => SessionService.RemoveServer(address, port);
    }
}