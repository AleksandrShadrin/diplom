using Grpc.Client.ClientForLearning;
using PSASH.Core.Constants;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public class MonoTimeSeriesModelTrainer : IMonoTimeSeriesModelTrainer
    {
        private TrainOptions _options = new TrainOptions(100,
            Core.Constants.CutOption.CUT_BOTH,
            Core.Constants.FillOption.FILL_BOTH);

        private readonly ILearningClient _learningClient;

        public MonoTimeSeriesModelTrainer(ILearningClient learningClient)
        {
            _learningClient = learningClient;
        }

        public async Task<List<string>> GetDatasetNames()
        {
            return await _learningClient.GetModelsNamesForLearning();
        }

        public async Task<List<UntrainedModel>> GetUntrainedModels()
        {
            var modelsNames = await _learningClient
                .GetModelsNamesForLearning();

            return modelsNames
                .Select(name => new UntrainedModel(name, ""))
                .ToList();
        }

        public void SetTrainOptions(TrainOptions options)
        {
            _options = options;
        }

        public async Task<bool> TrainModel(UntrainedModel model)
        {
            var modelParameters = ConvertLearningOptionsToModelParameters();
            var modelToSend = new Grpc.Client.Models.Model(model.DatasetName,
                model.Name,
                modelParameters);

            var result = await _learningClient.TrainModel(modelToSend);

            return result switch
            {
                Grpc.Client.Models.TrainStatus.SUCCESS => true,
                _ => false,
            };
        }

        private Grpc.Client.Models.ModelParameters ConvertLearningOptionsToModelParameters()
        {
            var fillType = _options.FillOption switch
            {
                FillOption.FILL_BOTH => Grpc.Client.Models.FillType.FILL_ZEROES_BOTH,
                FillOption.FILL_LEFT => Grpc.Client.Models.FillType.FILL_ZEROES_LEFT,
                FillOption.FILL_RIGHT => Grpc.Client.Models.FillType.FILL_ZEROES_RIGHT
            };

            var cutType = _options.CutOption switch
            {
                CutOption.CUT_BOTH => Grpc.Client.Models.CutType.CUT_BOTH,
                CutOption.CUT_RIGHT => Grpc.Client.Models.CutType.CUT_RIGHT,
                CutOption.CUT_LEFT => Grpc.Client.Models.CutType.CUT_LEFT
            };

            var modelParameters = new Grpc.Client.Models.ModelParameters(fillType,
                cutType,
                _options.TimeSeriesLength);

            return modelParameters;
        }
    }
}
