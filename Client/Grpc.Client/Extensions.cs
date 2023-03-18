using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grpc.Client
{
    public static class Extensions
    {
        public static IServiceCollection RegisterGrpcClients(this IServiceCollection services)
        {
            services.AddSingleton<ISendingClient, SendingClient>(o =>
            {
                var config = o.GetService<IConfiguration>();

                var url = config.GetSection("Server")["url"];
                var channel = GrpcChannel.ForAddress(url);

                return new SendingClient(channel);
            });

            return services;
        }

    }
}
