using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace HttpLoadBalancer.Models.Methods
{
    public class RandomMethod : Method
    {
        public RandomMethod()
        {
            Name = "Random";
        }

        /// <summary>
        /// Gets a random server
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public override Server GetServer(List<Server> servers)
        {
            var index = new Random().Next(0, servers.Count);
            return servers[index];
        }
    }
}
