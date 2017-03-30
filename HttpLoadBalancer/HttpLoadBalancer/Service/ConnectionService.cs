using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service
{
    public class ConnectionService
    {
        public event EventHandler ServerOffline;
        public event EventHandler ServerOnline;
        const int BufferSize = 2048;

        public List<Server> Servers = new List<Server>();

        public async Task HandleRequest(TcpClient client)
        {
            var stream = client.GetStream();
            var request = await ReceiveRequest(stream);
            var responseStream = SendeRequest(request);
            var response = await GetResponse(responseStream);
            SendResponse(stream, response);
        }

        private async Task<HttpMessage> ReceiveRequest(NetworkStream stream)
        {
            var buffer = new byte[BufferSize];
            await stream.ReadAsync(buffer, 0, BufferSize);
            var context = Encoding.UTF8.GetString(buffer);
            var request = new HttpMessage(context);
            return request;
        }

        private NetworkStream SendeRequest(HttpMessage request)
        {
            // httpMessage for session persistence (cookie)
            var server = MethodService.CurrentMethod.GetServer(Servers);
            var serverClient = new TcpClient();
            serverClient.Connect(server.Address, server.Port);
            var serverStream = serverClient.GetStream();
            var requestArray = HttpMapper.ToRequest(request);
            serverStream.Write(requestArray, 0, requestArray.Length);
            return serverStream;
        }

        private async Task<HttpMessage> GetResponse (NetworkStream stream)
        {
            var buffer = new byte[BufferSize];
            try
            {
                await stream.ReadAsync(buffer, 0, BufferSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var context = Encoding.UTF8.GetString(buffer);
            var response = new HttpMessage(context);
            return response;
        }

        private void SendResponse(NetworkStream stream, HttpMessage message)
        {
            var response = HttpMapper.ToResponse(message);
            stream.Write(response, 0 , response.Length);
        }

        public Server AddServer(string address, int port)
        {
            var server = new Server(address, port);
            Servers.Add(server);
            return server;
        }

        public bool RemoveServer(Server server)
        {
            return Servers.Remove(server);
        }
    }
}