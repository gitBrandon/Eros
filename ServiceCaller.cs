using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class ServiceCaller
    {
        public string Post(string operationEndpoint, string requestJson)
        {
            try
            {
                string serviceEndpoint = Config.GetServiceEndpoint();
                WebClient clientProxy = new WebClient();

                var http = (HttpWebRequest)WebRequest.Create(new Uri(serviceEndpoint + operationEndpoint));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                //String username = uid;
                //String password = pass;
                //String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                //http.Headers.Add("Authorization", "Basic " + encoded);


                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(requestJson);

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                return content;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
            }

            return null;
        }
    }
}
