using System.Net.Sockets;

namespace HttpLoadBalancer.Models
{
    public abstract class Method
    {
        //protected Method(string name)
        //{
        //    Name = name;
        //}
        public string Name { get; set; }
        public virtual HttpMessage ProcessRequest(NetworkStream message, HttpMessage httpMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}