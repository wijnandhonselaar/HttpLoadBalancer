using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace HttpLoadBalancer.Models.Methods
{
    public class RoundRobinMethod : Method
    {
        public RoundRobinMethod()
        {
            Name = "Round Robin";
        }

        private int _index = 0;

        /// <summary>
        /// Get the next server in line
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override Server GetServer(List<Server> servers)
        {
            var server = servers[_index];
            _index++;
            if (_index == servers.Count) _index = 0;
            return server;
        }
    }   
}
