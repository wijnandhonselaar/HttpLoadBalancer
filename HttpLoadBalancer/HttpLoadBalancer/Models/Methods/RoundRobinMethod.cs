using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using HttpLoadBalancer.Service;

namespace HttpLoadBalancer.Models.Methods
{
    public class RoundRobinMethod : Method
    {
        public RoundRobinMethod()
        {
            Name = "Round Robin";
        }

        private int _index;

        /// <summary>
        /// Get the next (online) server in line
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override async Task<Server> GetServer(List<Server> servers)
        {
            while (! await MethodService.Monitor.IsHealthy(servers[_index]))
            {
                _index++;
                if (_index == servers.Count) _index = 0;
            }
            var server = servers[_index];
            _index++;
            if (_index == servers.Count) _index = 0;
            return server;
        }
    }   
}
