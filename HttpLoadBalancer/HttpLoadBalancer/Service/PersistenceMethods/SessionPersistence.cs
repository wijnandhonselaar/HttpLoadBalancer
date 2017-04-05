using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Interfaces;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service.PersistenceMethods
{
    public class SessionPersistence : IPersistenceMethod
    {
        public HttpMessage SaveSession(HttpMessage response, Server currentServer)
        {
            var sessions = SessionService.Sessions;
            if (!response.Properties.ContainsKey("set-cookie")) return response;
            var value = response.Properties["set-cookie"];
            var split = value.Split(';');
            string key = null;
            string expires = null;
            foreach (var item in split)
            {
                if (item.Contains("connect.sid"))
                {
                    key = item.Split('=')[1];
                }
                if (item.Contains("Expires"))
                {
                    expires = item.Split('=')[1];
                }
            }
            if (key == null) return response;
            if (!sessions.ContainsKey(key)) sessions.Add(key, new Cookie(currentServer, expires));
            return response;
        }

        public Server GetServerFromSession(HttpMessage message)
        {
            var key = GetKey(message);
            if (key == null) return null;
            var serverFromSession = GetSessionServer(key);
            if (serverFromSession != null && SessionService.Servers.Any(x => x.Address == serverFromSession.Address && x.Port == serverFromSession.Port && x.Status == Status.Online))
                return serverFromSession;
            return null;
        }

        public bool HasSession(HttpMessage message)
        {
            var key = GetKey(message);
            if (key == null) return false;
            return GetSessionServer(key) != null;
        }

        private Server GetSessionServer(string key)
        {
            var sessions = SessionService.Sessions;
            return sessions.ContainsKey(key) ? sessions[key].Server : null;
        }

        public bool HasValidSession(HttpMessage message)
        {
            var sessions = SessionService.Sessions;
            var key = GetKey(message);
            var serverFromSession = key != null && sessions.ContainsKey(key) ? sessions[key].Server : null;
            return serverFromSession != null && SessionService.Servers.Contains(serverFromSession);
        }

        private string GetKey(HttpMessage message)
        {
            if (!message.Properties.ContainsKey("Cookie")) return null;
            var value = message.Properties["Cookie"];
            var split = value.Split(';');
            string key = null;
            foreach (var item in split)
            {
                if (item.Contains("connect.sid"))
                {
                    key = item.Split('=')[1];
                }
            }
            return key;
        }
    }
}
