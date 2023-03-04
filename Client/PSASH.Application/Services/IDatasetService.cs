using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface IDatasetService<T>
        where T : BaseTimeSeries
    {
        /// <summary>
        /// ��������� �������
        /// </summary>
        /// <returns>���������� Dataset</returns>
        Dataset LoadDataset();
        /// <summary>
        /// ��������� ��������� ��� �� ��������
        /// </summary>
        /// <param name="info">���������� �� ��������� ����</param>
        /// <returns>���������� ��������� ���</returns>
        T LoadTimeSeries(TimeSeriesInfo info);
    }
}