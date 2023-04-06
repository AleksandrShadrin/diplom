using Grpc.Client.ClientForLearning;
using Grpc.Client.ClientForPrediction;
using Grpc.Client.ClientForSending;
using Grpc.Client.HealthCheck;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grpc.Client
{
    public static class Extensions
    {
        public static IServiceCollection RegisterGrpcClients(this IServiceCollection services)
        {
            services.AddTransient<ISendingClient>(o =>
            {
                var config = o.GetService<IConfiguration>();

                var url = config.GetSection("Server")["url"];
                var channel = GrpcChannel.ForAddress(url);

                return new SendingClient(channel);
            });

            services.AddTransient<IHealthClient>(o =>
            {
                var config = o.GetService<IConfiguration>();

                var url = config.GetSection("Server")["url"];
                var channel = GrpcChannel.ForAddress(url);

                return new HealthClient(channel);
            });

            services.AddTransient<ILearningClient>(o =>
            {
                var config = o.GetService<IConfiguration>();

                var url = config.GetSection("Server")["url"];
                var channel = GrpcChannel.ForAddress(url);

                return new LearningClient(channel);
            });

            services.AddTransient<IPredictionClient>(o =>
            {
                var config = o.GetService<IConfiguration>();

                var url = config.GetSection("Server")["url"];
                var channel = GrpcChannel.ForAddress(url);

                return new PredictionClient(channel);
            });

            services.AddSingleton<ServerStatusService>();

            return services;
        }
    }
}
