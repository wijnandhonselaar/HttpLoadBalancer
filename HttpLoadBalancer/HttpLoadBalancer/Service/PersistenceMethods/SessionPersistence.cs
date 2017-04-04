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

        public Server GetServerFromSession(HttpMessage request)
        {
            var sessions = SessionService.Sessions;
            if (!request.Properties.ContainsKey("Cookie")) return null;
            var value = request.Properties["Cookie"];
            var split = value.Split(';');
            string key = null;
            foreach (var item in split)
            {
                if (item.Contains("connect.sid"))
                {
                    key = item.Split('=')[1];
                }
            }
            // TODO RETURNED NU OOK SERVERS DIE NIET MEER IN DE SERVERS LIJST STAAN
            return key != null && sessions.ContainsKey(key) ? sessions[key].Server : null;
        }
    }
}
