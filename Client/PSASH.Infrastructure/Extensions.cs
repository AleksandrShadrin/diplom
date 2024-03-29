﻿using Grpc.Client;
using Microsoft.Extensions.DependencyInjection;
using PSASH.Application.Services;
using PSASH.Core.Entities;
using PSASH.Infrastructure.Services;
using PSASH.Infrastructure.Services.FileBased;
using PSASH.Infrastructure.Services.FileBased.Converter;
using PSASH.Infrastructure.Services.GrpcBased;

namespace PSASH.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services
                .AddTransient<IFileBasedMonoTimeSeriesConverter, FileBasedMonoTimeSeriesConverter>()
                .AddSingleton<IFileBasedMonoDatasetService, FileBasedMonoDatasetService>()
                .AddSingleton<IDatasetService<MonoTimeSeries>>(o
                    => o.GetRequiredService<IFileBasedMonoDatasetService>())
                .RegisterGrpcClients()
                .AddSingleton<IDatasetSender, MonoDatasetSender>()
                .AddTransient<ITrainedModelsLoader, TrainedModelsLoader>()
                .AddSingleton<IMonoTimeSeriesModelTrainer, MonoTimeSeriesModelTrainer>()
                .AddSingleton<ILearningService, MonoTimeSeriesLearningService>()
                .AddSingleton<ITrainedModelService<MonoTimeSeries>, TrainedModelService>();

            return services;
        }
    }
}
