using System.Collections.Generic;
using System.Linq;
using HttpLoadBalancer.Model;
using HttpLoadBalancer.Model.Methods;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Service
{
    public static class MethodService
    {
        public static Method CurrentMethod { get; private set; }

        public static List<Method> Methods = new List<Method>
        {
            new RandomMethod(),
            new RoundRobinMethod("RoundRobin")
        };

        public static bool SetMethod(string methodName)
        {
            var newMethod = Methods.FirstOrDefault(x => x.Name == methodName);
            if (newMethod == null) return false;
            CurrentMethod = newMethod;
            return true;
        }
    }
}