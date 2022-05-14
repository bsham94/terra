/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace terra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>().Build().Run();

            //string appsettings = "";
            //if (File.Exists("./appsettings.json"))
            //{
            //    appsettings = "./appsettings.json";
            //}

            //if (!String.IsNullOrEmpty(appsettings))
            //{
            //    var configurations = new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .AddJsonFile(appsettings)
            //    .AddEnvironmentVariables("ASPNETCORE_ENVIROMENT")
            //    .Build();

            //    new WebHostBuilder()
            //        .UseConfiguration(configurations)
            //        .UseKestrel()
            //        .UseContentRoot(Directory.GetCurrentDirectory())
            //        .UseStartup<Startup>()
            //        .Build()
            //        .Run();
            //}
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
