using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Services.FileBased.Converter;

namespace PSASH.Infrastructure.Services.FileBased
{
    public class FileBasedMonoDatasetService : IFileBasedMonoDatasetService
    {
        private string _path = String.Empty;
        private Dataset? _dataset;
        private readonly ITimeSeriesConverter<string, MonoTimeSeries> _timeSeriesConverter;

        public FileBasedMonoDatasetService(ITimeSeriesConverter<string, MonoTimeSeries> timeSeriesConverter)
        {
            _timeSeriesConverter = timeSeriesConverter is null ?
                throw new ArgumentNullException(
                    nameof(timeSeriesConverter),
                    "Converter can't be null") :
                timeSeriesConverter;
        }

        /// <summary>
        /// Загрузка датасета
        /// </summary>
        /// <returns>Возвращает Dataset</returns>
        public Dataset LoadDataset()
        {
            ValidateDatasetStructure(_path);

            var datasetName = new DirectoryInfo(_path).Name;
            var timeSeriesInfoList = new List<TimeSeriesInfo>();

            foreach (var directory in Directory.GetDirectories(_path))
            {
                var directoryName = new DirectoryInfo(directory).Name;

                var timeSeriesInfoForDirectory =
                    Directory
                        .GetFiles(directory)
                        .Select(Path.GetFileNameWithoutExtension)
                        .Select(fn => new TimeSeriesInfo(directoryName, fn));

                timeSeriesInfoList.AddRange(timeSeriesInfoForDirectory);
            }

            var dataset = new Dataset(timeSeriesInfoList, datasetName);
            _dataset = dataset;

            return dataset;
        }

        /// <summary>
        /// Загрузить временной ряд по его информации
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DatasetNotLoadedException"></exception>
        public MonoTimeSeries LoadTimeSeries(TimeSeriesInfo info)
        {
            if (_dataset is null)
                throw new DatasetNotLoadedException();

            if (_dataset.GetValues().Any(tsi => tsi == info))
            {
                var filePath = GetFilePath(info);
                return _timeSeriesConverter.Convert(filePath);
            }

            throw new TimeSeriesDontExistException(info);
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
            if (Directory.Exists(path) == false)
            {
                throw new PathDontExistException(path);
            }

            _path = path;
        }

        #region Private Methods

        /// <summary>
        /// Проверка структуры датасета 
        /// </summary>
        /// <exception cref="DatasetStructureInvalidException">
        /// Структура датасета нарушена
        /// </exception>
        private void ValidateDatasetStructure(string path)
        {
            var directories = Directory.GetDirectories(path);

            // Проверка на наличие ненужных папок в датасете
            if (directories
                .Select(Directory.GetDirectories)
                .Any(dirs => dirs.Length != 0))
            {
                throw new DatasetStructureInvalidException(path);
            }

            var sameExtensions = directories
                .Select(FilesExtensionsTheSame)
                .ToList();

            if (sameExtensions
                    .Any(ext => ext.IsSame == false) ||
                sameExtensions
                    .DistinctBy(ext => ext.Ext)
                    .Count() > 1)
            {
                throw new DatasetStructureInvalidException(path);
            }
        }

        private string GetFilePath(TimeSeriesInfo info)
        {
            var path = Path.Combine(_path, info.Class);
            var fileExt = Path.GetExtension(Directory
                .GetFiles(path)
                .FirstOrDefault());

            return fileExt is null ?
                "" : Path.Combine(path, info.id + fileExt);
        }

        private SameExt FilesExtensionsTheSame(string directory)
        {
            var current_ext = string.Empty;

            foreach (var file in Directory.GetFiles(directory))
            {
                var ext = Path.GetExtension(file);

                if (String.Empty == current_ext)
                {
                    current_ext = ext;
                    continue;
                }
                else if (ext != current_ext)
                {
                    return new(current_ext, false);
                }
            }

            return new(current_ext, true);
        }

        #endregion

        /// <summary />
        /// <param name="Ext">Расширение файлов в папке</param>
        /// <param name="IsSame">Расширения в папке одинаковые</param>
        private record SameExt(string Ext, bool IsSame);
    }
}