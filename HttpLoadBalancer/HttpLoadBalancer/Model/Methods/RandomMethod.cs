using System.Net.Sockets;

namespace HttpLoadBalancer.Model.Methods
{
    public class RandomMethod : Method
    {
        public RandomMethod(string name) : base(name)
        {
        }

        public override HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            return new HttpMessage("");
        }
    }
}
