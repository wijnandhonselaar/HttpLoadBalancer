using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace HttpLoadBalancer.Models
{
    public class HttpMessage
    {
        public HttpMessage(string httpMessage)
        {
            Properties = HttpMapper.ToHead(httpMessage);
        }

        public Dictionary<string, string> Properties { get; set; }
    }

    public static class HttpMapper
    {
        public static Dictionary<string, string> ToHead(string context)
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
        public static List<string> RequestToList(string text)
        {
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        public static byte[] ToRequest(HttpMessage message)
        {
            var request = "";
            var statusLine = new List<string> {"Method", "Url", "HttpVersion"};
            foreach (var prop in message.Properties)
            {
                if (statusLine.Contains(prop.Key))
                {
                    request += $"{prop.Value} ";
                    if (prop.Key == "HttpVersion")
                        request += "\r\n";
                }
                else
                {
                    request += $"{prop.Key}: {prop.Value}\r\n";
                }
            }
            return Encoding.ASCII.GetBytes(request);
        }

        public static string GetRequestHead(HttpMessage message)
        {
            var head = "";
            // status line
            //head += "HTTP/" + message.Properties["HttpVersion"] + " " + httpRes.StatusCode.GetHashCode() + " " +
            //        httpRes.StatusDescription + "\r\n";
            //// Date
            //head += ToHeaderProperty("Date", httpRes.Headers["Date"]);
            //// Server
            //head += ToHeaderProperty("Server", httpRes.Server);
            //// Content-Type
            //head += ToHeaderProperty("Content-Type", httpRes.ContentType);
            //// Content-Length
            //head += ToHeaderProperty("Content-Length", httpRes.ContentLength.ToString());

            return head;
        }

        public static byte[] ToResponse(HttpMessage message)
        {
            // Creating a list because this is easier to manupulate in comparrison to the array it is.
            var head = GetReponseHead(message);
            var response = new List<byte>(Encoding.UTF8.GetBytes(head));
            response.AddRange(Encoding.UTF8.GetBytes("\r\n"));
            string body;
            message.Properties.TryGetValue("Body", out body);
            response.AddRange(Encoding.ASCII.GetBytes(body));
            return response.ToArray();
        }

        public static string GetReponseHead(HttpMessage message)
        {
            var head = "";
            // status line
            //head += "HTTP/" + message.Properties["HttpVersion"] + " " + httpRes.StatusCode.GetHashCode() + " " +
            //        httpRes.StatusDescription + "\r\n";
            // Date
            //head += ToHeaderProperty("Date", httpRes.Headers["Date"]);
            // Server
            //head += ToHeaderProperty("Server", httpRes.Server);
            // Content-Type
            //head += ToHeaderProperty("Content-Type", httpRes.ContentType);
            // Content-Length
            //head += ToHeaderProperty("Content-Length", httpRes.ContentLength.ToString());

            return head;
        }

    }
}