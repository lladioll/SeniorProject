using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; 
using System.Net;
using System.Net.Sockets;

namespace SeniorProject
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
               .AddCommandLine(args)
               .AddEnvironmentVariables(prefix: "ASPNETCORE_")
               .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
           /*     .UseWebListener(options =>
                {
                    options.ListenerSettings.Authentication.Schemes = AuthenticationSchemes.NTLM;
                    options.ListenerSettings.Authentication.AllowAnonymous = false;
                }) */
                .UseStartup<Startup>()
                .Build();

            host.Run();    
        }
    }
}