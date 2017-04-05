using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Interfaces
{
    public interface IPersistenceMethod
    {
        HttpMessage SaveSession(HttpMessage response, Server currentServer);
        bool HasSession(HttpMessage message);
        bool HasValidSession(HttpMessage message);
        Server GetServerFromSession(HttpMessage request);
    }
}
