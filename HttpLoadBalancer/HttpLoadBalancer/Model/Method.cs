using System.Net.Sockets;

namespace HttpLoadBalancer.Model
{
    public abstract class Method
    {
        public string Name { get; set; }
        public virtual HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}