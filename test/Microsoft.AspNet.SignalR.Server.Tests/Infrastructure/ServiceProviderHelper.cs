using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Hosting.Server;
using Microsoft.AspNet.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNet.SignalR.Tests
{
    /// <summary>
    /// Summary description for ServiceProviderHelper
    /// </summary>
    public class ServiceProviderHelper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            return CreateServiceProvider(_ => { });
        }

        public static IServiceProvider CreateServiceProvider(Action<IServiceCollection> configure)
        {
            var host = new WebHostBuilder()
                .UseServer(new ServerFactory())
                .UseStartup(
                    _ => { },
                    services =>
                    {
                        services.AddSignalR();
                        configure(services);
                        return services.BuildServiceProvider();
                    });
            return host.Build().ApplicationServices;
        }

        private class Server : IServer
        {
            private RequestDelegate _requestDelegate;

            public IFeatureCollection Features { get; }
            
            public void Start(RequestDelegate requestDelegate)
            {
                _requestDelegate = requestDelegate;
            }

            public void Dispose()
            {

            }
        }

        private class ServerFactory : IServerFactory
        {
            public IServer CreateServer(IConfiguration configuration)
            {
                return new Server();
            }
        }
    }
}