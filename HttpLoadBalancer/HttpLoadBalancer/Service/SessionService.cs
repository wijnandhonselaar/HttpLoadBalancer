using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service
{
    public static class SessionService
    {
        public static Dictionary<string, Cookie> Sessions = new Dictionary<string, Cookie>();
        public static IPersistenceMethod SessionManager { get; set; }
        public static List<Server> Servers = new List<Server>();

        public static void SaveSession(HttpMessage message, Server currentServer)
        {
            SessionManager.SaveSession(message, currentServer);
        }

        public static Server GetServerFromSession(HttpMessage message)
        {
            return SessionManager.GetServerFromSession(message);
        }

        public static void AddServer(Server server)
        {
            Servers.Add(server);
        }

        public static void AddServer(string address, int port)
        {
            Servers.Add(new Server(address, port));
        }

        public static bool RemoveServer(Server server)
        {
            return Servers.Remove(server);
        }

        public static bool RemoveServer(string address, int port)
        {
            var server = Servers.FirstOrDefault(x => x.Address == address && x.Port == port);
            if (server == null) return false;
            return Servers.Remove(server);
        }
    }
}
