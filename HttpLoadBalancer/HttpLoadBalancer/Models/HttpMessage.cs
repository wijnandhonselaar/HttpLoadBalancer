using System.Collections.Generic;
using System.Text;
using HttpLoadBalancer.Service;

namespace HttpLoadBalancer.Models
{
    public class HttpMessage
    {
        public HttpMessage()
        {
        }

        public HttpMessage(byte[] httpMessage, bool isReponse = false)
        {
            Original = httpMessage;
            var context = Encoding.UTF8.GetString(httpMessage);
            context = context.Replace("\0", "");
            if (!isReponse && context.Length > 0)
                Properties = HttpMapper.ToRequestHead(context);
            else if(isReponse && context.Length > 0)
                Properties = HttpMapper.ToResponseHead(context);

        }

        public byte[] Original { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}