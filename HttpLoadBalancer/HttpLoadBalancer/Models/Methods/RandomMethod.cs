﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using HttpLoadBalancer.Service;
using System.Threading.Tasks;

namespace HttpLoadBalancer.Models.Methods
{
    public class RandomMethod : Method
    {
        public RandomMethod()
        {
            Name = "Random";
        }

        /// <summary>
        /// Gets a random (online) server
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override async Task<Server> GetServer(List<Server> servers)
        {
            var index = new Random().Next(0, servers.Count);
            while (servers[index].Status != Status.Online && !await MethodService.Monitor.IsHealthy(servers[index]))
            {
                index = new Random().Next(0, servers.Count);
            }
            return servers[index];
        }
    }
}
