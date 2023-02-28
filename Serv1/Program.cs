using System;
using System.Collections;
using System.IO;
using System.Net;

namespace Serv_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:8080/");
            listener.Start();

            while (true)
            {
                var context = listener.GetContext();
                Console.WriteLine(context.Request.Url);
                Console.WriteLine(context.Request.RemoteEndPoint);

                string filePath = context.Request.Url?.LocalPath.Trim('/');






                //filePath = "Sample Web Page.html";
                Console.WriteLine(filePath);

                if (!File.Exists(filePath))
                {
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    response.StatusCode = 404;
                    response.OutputStream.Close();
                }
                else
                {
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    System.IO.Stream input = File.OpenRead(filePath);
                    byte[] buffer = new byte[1024];
                    int bytesRead = input.Read(buffer, 0, buffer.Length);
                    while (bytesRead > 0)
                    {
                        response.OutputStream.Write(buffer, 0, bytesRead);
                        bytesRead = input.Read(buffer, 0, buffer.Length);
                    }
                    response.StatusCode = 200;
                    input.Close();
                    response.OutputStream.Close();
                }
            }

            listener.Stop();
        }
    }
}