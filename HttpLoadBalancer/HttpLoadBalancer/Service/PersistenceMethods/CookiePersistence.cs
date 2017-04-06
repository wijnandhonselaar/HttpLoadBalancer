using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service.PersistenceMethods
{
    public class CookiePersistence : IPersistenceMethod
    {
        public HttpMessage SaveSession(HttpMessage response, Server currentServer)
        {
            if (!response.Properties.ContainsKey("set-cookie")) return response;
            response.Properties["set-cookie"] = $"serverID={currentServer.Address}-{currentServer.Port}";
            return response;
        }

        public Server GetServerFromSession(HttpMessage message)
        {
            var serverFromCookie = GetServerFromMessage(message);
            return SessionService.Servers.FirstOrDefault(x => x.Address == serverFromCookie.Address && x.Port == serverFromCookie.Port);
        }

        public bool HasSession(HttpMessage message)
        {
            var data = GetCookieArray(message);
            if (data == null) return false;
            if (data.Length < 2) return false;
            try
            {
                var server = new Server(data[0], int.Parse(data[1]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool HasValidSession(HttpMessage message)
        {
            var server = GetServerFromMessage(message);
            return
                SessionService.Servers.Any(
                    x => x.Address == server.Address && x.Port == server.Port && x.Status == Status.Online);
        }

        private Server GetServerFromMessage(HttpMessage message)
        {
            var data = GetCookieArray(message);
            if (data == null) return null;
            var address = data[0];
            var port = int.Parse(data[1]);
            return SessionService.Servers.FirstOrDefault(x => x.Address == address && x.Port == port);
        }

        private string[] GetCookieArray(HttpMessage message)
        {
            if (!message.Properties.ContainsKey("Cookie")) return null;
            var value = message.Properties["Cookie"];
            if (value.Contains(';'))
            {
                var cookies = value.Split(';');
                foreach (var c in cookies)
                {
                    if (c.Contains("serverID"))
                    {
                        value = c;
                    }
                }
            }
            return value.Contains("serverID") ? value.Split('=')[1].Split('-') : null;
        }
    }
}
