/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  Startup.cs                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file sets up the webapplication                                           |
*************************************************************************************************/
using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace terra
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static string ConnectionString
        {
            get;
            private set;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Set the firebase service.
            services.AddFirebaseAuthentication("https://securetoken.google.com/terra-1547225268592", "terra-1547225268592");
            //Set json serializer settings
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "./terra/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Make sure server will run .unityweb files.
            StaticFileOptions option = new StaticFileOptions();
            FileExtensionContentTypeProvider contentTypeProvider = (FileExtensionContentTypeProvider)option.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".unityweb", "application/octet-stream");
            option.ContentTypeProvider = contentTypeProvider;
            //Set cors option
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            //Use authentication
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles(option);
            app.UseSpaStaticFiles(option);
            app.UseMvc();
            //User single page application middleware
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "./terra";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            //Set Database Connection String
            //If aspnetcore enviroment variable is set to development, use sams rasberry pi database
            if (env.IsDevelopment())
            {
                ConnectionString = Configuration["ConnectionStrings:SamsPie"];
            }
            else
            {
                ConnectionString = Configuration["ConnectionStrings:Azure"];
            }
                                
        }
    }
}
