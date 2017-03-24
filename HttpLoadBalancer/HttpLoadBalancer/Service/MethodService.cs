using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HttpLoadBalancer.Model;

namespace HttpLoadBalancer.Service
{
    public static class MethodService
    {
        /// <summary>
        /// Dictionary string methodName, Method method
        /// </summary>
        public static Dictionary<string, Method> Methods = new Dictionary<string, Method>
        {
            {"Round Robin", new RoundRobinMethod() }
        };

        public static Method CurrentMethod { get; private set; }

        public static void SetMethod(string methodName)
        {
            CurrentMethod = Methods[methodName];
        }
    }
}