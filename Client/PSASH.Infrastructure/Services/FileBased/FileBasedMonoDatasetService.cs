using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Services.FileBased.Converter;

namespace PSASH.Infrastructure.Services.FileBased
{
    public class FileBasedMonoDatasetService : IFileBasedMonoDatasetService
    {
        private string _path = String.Empty;
        private readonly ITimeSeriesConverter<string, MonoTimeSeries> _timeSeriesConverter;

        public FileBasedMonoDatasetService(string path, ITimeSeriesConverter<string, MonoTimeSeries> timeSeriesConverter)
        {
            _path = path;
            _timeSeriesConverter = timeSeriesConverter;
        }

        /// <summary>
        /// �������� ��������
        /// </summary>
        /// <returns>���������� Dataset</returns>
        public Dataset LoadDataset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ��������� ��������� ��� �� ��� ����������
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MonoTimeSeries LoadTimeSeries(TimeSeriesInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ���������� ���� � ��������
        /// </summary>
        /// <param name="path">
        /// ���� � ��������
        /// </param>
        /// <exception cref="PathDontExistException">
        /// ���� �� ���������� ����
        /// </exception>
        public void SetPath(string path)
        {
            if (Path.Exists(path))
            {
                _path = path;
            }

            throw new PathDontExistException(path);
        }

        /// <summary>
        /// �������� ��������� �������� 
        /// </summary>
        /// <exception cref="DatasetStructureInvalidException">
        /// ��������� �������� ��������
        /// </exception>
        private void ValidateDatasetStructure(string path)
        {

        }
    }
}