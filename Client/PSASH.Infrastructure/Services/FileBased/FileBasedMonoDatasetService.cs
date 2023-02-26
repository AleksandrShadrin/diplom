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
        /// Загрузка датасета
        /// </summary>
        /// <returns>Возвращает Dataset</returns>
        public Dataset LoadDataset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузить временной ряд по его информации
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MonoTimeSeries LoadTimeSeries(TimeSeriesInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Назначение пути к датасету
        /// </summary>
        /// <param name="path">
        /// Путь к датасету
        /// </param>
        /// <exception cref="PathDontExistException">
        /// Если не существует пути
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
        /// Проверка структуры датасета 
        /// </summary>
        /// <exception cref="DatasetStructureInvalidException">
        /// Структура датасета нарушена
        /// </exception>
        private void ValidateDatasetStructure(string path)
        {

        }
    }
}