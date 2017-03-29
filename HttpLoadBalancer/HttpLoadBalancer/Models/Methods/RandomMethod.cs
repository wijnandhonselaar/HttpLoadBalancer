using System.Net.Sockets;
using HttpLoadBalancer.Models;

namespace HttpLoadBalancer.Model.Methods
{
    public class RandomMethod : Method
    {
        public RandomMethod()
        {
            Name = "Random";
        }

        public override HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            return new HttpMessage("");
        }
    }
}
