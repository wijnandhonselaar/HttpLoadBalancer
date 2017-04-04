using System;
using System.Collections.Generic;
using System.Linq;
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
            response.Properties["set-cookie"] = $"{currentServer.Address}:{currentServer.Port}";
            return response;
        }

        public Server GetServerFromSession(HttpMessage request)
        {
            // TODO OMZETTEN NAAR COOKIE
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
            if (key == null) return null;
            // TODO RETURNED NU OOK SERVERS DIE NIET MEER IN DE SERVERS LIJST STAAN
            return SessionService.Sessions.ContainsKey(key) ? SessionService.Sessions[key].Server : null;
        }
    }
}
