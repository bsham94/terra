/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace terra
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;    
           
        }

        public static string ConnectionString
        {
            get;
            private set;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void ConfigureServices(IServiceCollection services)
        {
            //Setup Google's Firebase Authentication
            services.AddFirebaseAuthentication("https://securetoken.google.com/terra-1547225268592", "terra-1547225268592");
           // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Setup CORS
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

            //Set up JSON serializer/deserializer
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            /////
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = @"./terra_frontend";
            //});
            /////
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            /////////
            //ppa.UseStaticFiles();
            //app.UseSpaStaticFiles();


            ////////
            //Use CORS
            app.UseCors("EnableCORS");
            //app.UseHttpsRedirection();
            //Use Authentication
            app.UseAuthentication();
            app.UseMvc();
            app.UseMvcWithDefaultRoute();
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = @"./terra_frontend";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});

            //Set Database Connection String
            ConnectionString = Configuration["ConnectionStrings:SamsPie"];
        }
    }
}

//app.UseCors(builder => builder
//.AllowAnyOrigin()
//.AllowAnyMethod()
//.AllowAnyHeader()
//.AllowCredentials());


//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,

//            ValidIssuer = "http://localhost:54357",
//            ValidAudience = "http://localhost",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
//        };
//    });
//services
//.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.Authority = "https://securetoken.google.com/terra-1547225268592";
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = "https://securetoken.google.com/terra-1547225268592",
//        ValidateAudience = true,
//        ValidAudience = "terra-1547225268592",
//        ValidateLifetime = true
//    };
//});