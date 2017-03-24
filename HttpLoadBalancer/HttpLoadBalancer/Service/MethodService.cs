using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HttpLoadBalancer.Model;
using HttpLoadBalancer.Model.Methods;

namespace HttpLoadBalancer.Service
{
    public static class MethodService
    {
        /// <summary>
        /// Dictionary string methodName, Method method
        /// </summary>
        public static List<Method> Methods = new List<Method>
        {
            new RoundRobinMethod("Round Robin")
        };

        public static Method CurrentMethod { get; private set; }

        public static void SetMethod(string methodName)
        {
            CurrentMethod = Methods.FirstOrDefault(x => x.Name == methodName);
        }
    }
}