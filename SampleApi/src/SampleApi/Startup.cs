﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Formatting;
using SampleApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.AspNetCore.Mvc.Formatters;
using SampleApi.Formatters;

namespace SampleApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContactRepository, InMemoryContactRepository>();
            services.AddSingleton<IContentNegotiator>(new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));
            services.AddSingleton<LinkProvider>();
            services.AddSingleton<ContactSelfLinkFilter>();

            services.AddMvc().Configure<MvcOptions>(options =>
            {
                options.OutputFormatters.Insert(0, new HttpResponseMessageOutputFormatter());
                options.OutputFormatters.Insert(0, new HttpNotAcceptableOutputFormatter());
                options.OutputFormatters.Insert(0, new CsvMediaTypeFormatter());
                options.AddXmlDataContractSerializerFormatter();
                options.RespectBrowserAcceptHeader = true;

            }).Configure<WebApiCompatShimOptions>(opt =>
            {
                opt.Formatters = new MediaTypeFormatterCollection();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var metadata = new ErrorData
                    {
                        Message = "An unexpected error occurred! The error ID will be helpful to debug the problem",
                        DateTime = DateTimeOffset.Now,
                        RequestUri = new Uri(context.Request.Host.ToString() + context.Request.Path.ToString() + context.Request.QueryString),
                        ErrorId = Guid.NewGuid()
                    };

                    var connection = context.Connection;
                    if (connection.LocalIpAddress == connection.RemoteIpAddress)
                    {
                        if (errorFeature.Error != null)
                        {
                            metadata.Exception = errorFeature.Error;
                        }
                    }

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
                });

            });

            app.UseMiddleware<TimerMiddleware>();

            app.UseMvc();
        }
    }
}