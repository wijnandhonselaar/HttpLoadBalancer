using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HttpLoadBalancer.Models;
using HttpLoadBalancer.Service;

namespace HttpLoadBalancer.Service
{
    public static class HttpMapper
    {
        public static Dictionary<string, string> ToRequestHead(string context)
        {
            var request = new Dictionary<string, string>();
            var lines = RequestToList(context);
            for (var i = 0; i < lines.Count; i++)
            {
                // First line is the status line
                // erveything until the empty line is a header
                // the forloop after the empty line is in case the content is split in more than one line because of the split on Environment.NewLine
                if (i > 0)
                {
                    if (lines[i].Contains(':'))
                    {
                        request.Add(lines[i].Split(':')[0], lines[i].Split(':')[1].Trim());
                    }
                    else if (lines[i] == "")
                    {
                        var body = "";
                        for (var y = i + 1; y < lines.Count; y++)
                        {
                            body += lines[y];
                        }
                        request.Add("Body", body.Trim());
                        i = lines.Count;
                    }
                }
                else
                {
                    var statusLine = lines[i].Split(' ');
                    request.Add("Method", statusLine[0]);
                    request.Add("Url", statusLine[1]);
                    request.Add("HttpVersion", statusLine[2]);
                }
            }
            return request;
        }

        public static Dictionary<string, string> ToResponseHead(string context)
        {
            var response = new Dictionary<string, string>();
            var lines = RequestToList(context);
            for (var i = 0; i < lines.Count; i++)
            {
                // First line is the status line
                // erveything until the empty line is a header
                // the forloop after the empty line is in case the content is split in more than one line because of the split on Environment.NewLine
                if (i > 0)
                {
                    if (lines[i].Contains(':'))
                    {
                        response.Add(lines[i].Split(':')[0], lines[i].Split(':')[1].Trim());
                    }
                    else if (lines[i] == "")
                    {
                        var body = "";
                        for (var y = i + 1; y < lines.Count; y++)
                        {
                            body += lines[y];
                        }
                        response.Add("Body", body.Trim().Replace("\0", ""));
                        i = lines.Count;
                    }
                }
                else
                {
                    var statusLine = lines[i].Split(' ');
                    response.Add("ProtocolVersion", statusLine[0]);
                    response.Add("StatusCode", statusLine[1]);
                    response.Add("StatusDescription", statusLine[2]);
                }
            }
            return response;
        }
        public static string GetHead(HttpMessage message)
        {
            var head = "";
            // status line
            head += "HTTP/" + message.Properties["ProtocolVersion"] + " " + message.Properties["StatusCode"] + " " +
                    message.Properties["StatusDescription"] + "\r\n";
            // Date
            head += ToHeaderProperty("Date", message.Properties["Date"]);
            // Server
            head += ToHeaderProperty("Server", message.Properties["Server"]);
            // Content-Type
            head += ToHeaderProperty("Content-Type", message.Properties["ContentType"]);
            // Content-Length
            head += ToHeaderProperty("Content-Length", message.Properties["ContentLength"]);

            return head;
        }

        /// <summary>
        /// returns key and value as one string in the format used in a http header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToHeaderProperty(string key, string value)
        {
            return key + ": " + value + "\r\n";
        }

        public static List<string> RequestToList(string text)
        {
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        public static byte[] ToRequest(HttpMessage message)
        {
            var request = "";
            var statusLine = new List<string> { "Method", "Url", "HttpVersion" };
            foreach (var prop in message.Properties)
            {
                if (statusLine.Contains(prop.Key))
                {

                    if (prop.Key == "HttpVersion")
                        request += $"{prop.Value}\r\n";
                    else
                        request += $"{prop.Value} ";
                }
                else if (prop.Key == "Body")
                {
                    request += "\r\n";
                    request += prop.Value.Trim().Replace("\0", "");
                }
                else
                {
                    request += $"{prop.Key}: {prop.Value.Trim()}\r\n";
                }
            }
            return Encoding.ASCII.GetBytes(request);
        }

        public static void SetUrl(HttpMessage request, Server server)
        {
            request.Properties["Url"] = $"http://{server.Address}/";
            request.Properties["Host"] = server.Address;
        }
    }
}

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