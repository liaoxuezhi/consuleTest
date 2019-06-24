using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SchoolAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var date = DateTime.Parse("2019-06-24T17:23:43.584+08:00");
            //var abc = TimeZoneInfo.ConvertTimeToUtc(date);

            var ss = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var aa = "{ age : 20,name : \"aaa\"}";
            var bb = JsonConvert.DeserializeObject<Test>(aa);

            var cc = new Test() { Age = 30,Name="bbb" };
            var dd = JsonConvert.SerializeObject(cc);

            var freePort = FreeTcpPort();

            var host = WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://*:8899")
                .UseStartup<Startup>()
                .Build();

            var loggingFactory = host.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var logger = loggingFactory.CreateLogger(nameof(Program));
            logger.LogInformation($"{Process.GetCurrentProcess().Id}");

            host.Run();
        }

        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }

    class Test
    {
        //[JsonProperty("age")]
        public int Age { get; set; }
        //[JsonProperty("name")]
        public string Name { get; set; }
    }

}
