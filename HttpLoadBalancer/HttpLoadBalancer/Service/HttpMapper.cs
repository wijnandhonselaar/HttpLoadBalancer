using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLoadBalancer.Models;

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
                var line = lines[i];
                // First line is the status line
                // erveything until the empty line is a header
                // the forloop after the empty line is in case the content is split in more than one line because of the split on Environment.NewLine
                if (i > 0)
                {
                    if (line.Contains(':'))
                    {
                        request.Add(line.Split(':')[0], line.Split(':')[1].Trim());
                    }
                    else if (line == "")
                    {
                        var body = "";
                        for (var y = i + 1; y < lines.Count; y++)
                        {
                            body += lines[y];
                        }
                        request.Add("Body", body);
                        i = lines.Count;
                    }
                }
                else
                {
                    var statusLine = line.Split(' ');
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
                var line = lines[i];
                // First line is the status line
                // erveything until the empty line is a header
                // the forloop after the empty line is in case the content is split in more than one line because of the split on Environment.NewLine
                if (i > 0)
                {
                    if (line.Contains(':'))
                    {
                        if (line.Contains("Date") || line.Contains("set-cookie"))
                        {
                            var x = line.Split(':')[0];
                            var c = line.Substring(x.Length + 1).Trim();
                            response.Add(x, c);
                        }
                        else
                        {
                            response.Add(line.Split(':')[0], line.Split(':')[1].Trim());
                        }
                    }
                    else if (line == "")
                    {
                        var body = "";
                        for (var y = i + 1; y < lines.Count; y++)
                        {
                            if (lines[y] == "")
                                body += "\r\n";
                            else if (lines[y].Contains("<p>"))
                                body += $"\r\n{lines[y]}\r\n";
                            else
                                body += lines[y];
                        }
                        response.Add("Body", body.Replace("\0", ""));
                        i = lines.Count;
                    }
                }
                else
                {
                    var statusLine = line.Split(' ');
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
            head += $"{message.Properties["ProtocolVersion"]} {message.Properties["StatusCode"]} {message.Properties["StatusDescription"]}\r\n";
            foreach (var prop in message.Properties)
            {
                if (prop.Key == "ProtocolVersion" || prop.Key == "StatusCode" || prop.Key == "StatusDescription")
                    continue;
                if (prop.Key == "Body")
                {
                    head += $"\r\n{prop.Value}";
                }
                else
                    head += ToHeaderProperty(prop.Key, prop.Value);

            }
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
            return $"{key}: {value}\r\n";
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
                    request += prop.Value.Replace("\0", "");
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