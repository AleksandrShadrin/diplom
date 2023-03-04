using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface ILearningService
    {
        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="model">��� ������</param>
        /// <param name="options">��������� ��������</param>
        /// <returns>���������� ��������� ������</returns>
        TrainedModel TrainModel(UntrainedModel model, TrainOptions options);

        /// <summary>
        /// �������� ��������� ������
        /// </summary>
        /// <returns>���������� ������ ������� �������</returns>
        List<TrainedModel> GetTrainedModels();

        /// <summary>
        /// ���������� ������������ ������ ��� ��������
        /// </summary>
        /// <returns>���������� ������ ����������� �������</returns>
        List<UntrainedModel> GetUntrainedModels();
    }
}