using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using SkyscraperCore;
using System;

namespace Skyscraper
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Add all SignalR related services to IoC.
            services.AddSignalR();
            services.AddMvc();
            services.AddScoped<IGameFactory, GameFactory>();
            services.AddScoped<IGame, SkyscraperCore.Game>();
            services.AddScoped<ISymbolsProvider, SymbolsProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) => {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "http://192.168.0.3:8088");
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, x-xsrf-token" });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                await next();
            });
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
