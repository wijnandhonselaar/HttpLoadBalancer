using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using HttpLoadBalancer.Model;

namespace HttpLoadBalancer.Service
{
    public class ConnectionService
    {
        public event EventHandler ServerOffline;
        public event EventHandler ServerOnline;

        public ConnectionService()
        {
            
        }

        public ConcurrentBag<Server> Servers = new ConcurrentBag<Server>();

        public void HandleRequest()
        {
            throw new System.NotImplementedException();
        }

        private HttpMessage ReceiveRequest()
        {
            throw new System.NotImplementedException();
        }

        private HttpMessage SendeRequest(NetworkStream stream, HttpMessage httpMessage)
        {
            return MethodService.CurrentMethod.ProcessRequest(stream, httpMessage);
        }

        private void SendResponse(NetworkStream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}