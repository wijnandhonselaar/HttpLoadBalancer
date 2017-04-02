﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
        /// Get the next server in line
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override Server GetServer(List<Server> servers)
        {
            while (!MethodService.Monitor.IsHealthy(servers[_index].Address))
            {
                _index++;
            }
            var server = servers[_index];
            if (_index == servers.Count) _index = 0;
            return server;
        }
    }   
}
