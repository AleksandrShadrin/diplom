using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Services.FileBased.Converter;

namespace PSASH.Infrastructure.Services.FileBased
{
    public class FileBasedMonoDatasetService : IFileBasedMonoDatasetService
    {
        private string _path = String.Empty;
        private Dataset _dataset;
        private readonly ITimeSeriesConverter<string, MonoTimeSeries> _timeSeriesConverter;

        public FileBasedMonoDatasetService(ITimeSeriesConverter<string, MonoTimeSeries> timeSeriesConverter)
        {
            _timeSeriesConverter = timeSeriesConverter;
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
                var timeSeriesInfoForDirectory =
                    Directory
                        .GetFiles(directory)
                        .Select(Path.GetFileNameWithoutExtension)
                        .Select(fn => new TimeSeriesInfo(directory, fn));

                timeSeriesInfoList.AddRange(timeSeriesInfoForDirectory);
            }

            return new Dataset(timeSeriesInfoList, datasetName);
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
            if (Directory.Exists(path) == false)
            {
                throw new PathDontExistException(path);
            }

            _path = path;
        }

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

        record SameExt(string Ext, bool IsSame);
    }
}