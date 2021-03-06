﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using SocketChatServer.Handlers;
using SocketChatServer.Managers;
using IO.Encoders;

namespace SocketChatServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();

            var socketHandler = new IntroducingSocketHandler(new ConnectionManager(), new Utf8MessageEncoder());
            services.AddSingleton(typeof(IntroducingSocketHandler), socketHandler);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseWebSockets();

            string path = "/ws";
            var socketHandler = serviceProvider.GetService<IntroducingSocketHandler>();
            app.Map(path, builder => builder.UseMiddleware<SocketMiddleware>(socketHandler));

            app.UseStaticFiles();

            app.UseRewriter(new RewriteOptions().AddRedirect("^.*$", "index.html"));
        }
    }
}