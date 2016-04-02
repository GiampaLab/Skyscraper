using Microsoft.Owin;
using Owin;
using Autofac;
using Microsoft.AspNet.SignalR;
using SkyscraperCore;
using Autofac.Integration.SignalR;
using System.Reflection;

[assembly: OwinStartup(typeof(Skyscraper.Web.Startup))]

namespace Skyscraper.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            // Register dependencies, then...

            // Get your HubConfiguration. In OWIN, you'll create one
            // rather than using GlobalHost.
            var config = new HubConfiguration();

            builder.RegisterType<GameFactory>().As<IGameFactory>();
            builder.RegisterType<Game>().As<IGame>().InstancePerLifetimeScope();
           
            // Register your SignalR hubs.
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            config.Resolver = new AutofacDependencyResolver(container);
            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            app.UseAutofacMiddleware(container);

            app.Use(async (context, next) => {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:8000");
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, x-xsrf-token" });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                await next();
            });
            app.MapSignalR("/signalr", config);
        }
    }
}
