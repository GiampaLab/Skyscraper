﻿using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace Skyscraper
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Add all SignalR related services to IoC.
            services.AddSignalR();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSignalR();
            app.UseMvc();
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                //map.UseCors(CorsOptions.AllowAll);
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR();
            });
        }
    }
}