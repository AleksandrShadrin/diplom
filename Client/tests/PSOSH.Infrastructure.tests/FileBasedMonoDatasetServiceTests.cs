using FluentAssertions;
using NSubstitute;
using PSASH.Core.Entities;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Services.FileBased;
using PSASH.Infrastructure.Services.FileBased.Converter;

namespace PSOSH.Infrastructure.tests
{
    public class FileBasedMonoDatasetServiceTests
    {
        [Fact]
        public void Set_Of_not_Existing_Path_Should_Throw_PathDontExistException()
        {
            // Arrange
            var converter = Substitute.For<ITimeSeriesConverter<string, MonoTimeSeries>>();

            var path = Path
                .Combine(Path.GetTempPath(),
                    Guid.NewGuid().ToString());

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            var action = () => fileBasedMonoDatasetService.SetPath(path);

            // Assert
            action
                .Should()
                .Throw<PathDontExistException>()
                .Which
                .Path
                .Should()
                .Be(path);
        }

        [Fact]
        public void Valid_Dataset_Structure_Should_Not_Throw_DatasetStructureInvalidException()
        {
            // Arrange
            var tempPath = Path
                .Combine(Path.GetTempPath(),
                "Valid_Dataset_Structure_Should_Not_Throw_DatasetStructureInvalidException");

            CreateDirectoryWithPathAndFilesExtension(tempPath, "ext", 10, 10);

            var converter = Substitute.For<ITimeSeriesConverter<string, MonoTimeSeries>>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            var act = () =>
            {
                fileBasedMonoDatasetService.SetPath(tempPath);
                fileBasedMonoDatasetService.LoadDataset();
            };

            // Assert
            act
                .Should()
                .NotThrow<DatasetStructureInvalidException>();
        }

        [Fact]
        public void Valid_Dataset_Should_Have_Valid_Structure()
        {
            // Arrange
            var name = "Valid_Dataset_Should_Have_Valid_Structure";
            var count = 10;
            var tempPath = Path
                .Combine(Path.GetTempPath(),
                name);

            CreateDirectoryWithPathAndFilesExtension(tempPath,
                "ext",
                count,
                count);

            var converter = Substitute.For<ITimeSeriesConverter<string, MonoTimeSeries>>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            fileBasedMonoDatasetService.SetPath(tempPath);
            var dataset = fileBasedMonoDatasetService.LoadDataset();

            // Assert
            dataset
                .Name
                .Should()
                .Be(name, "because it's folder name.");

            dataset
                .GetValues()
                .Should()
                .HaveCount(count * count, $"because files in each folder is {count} and folders count is {count}");
        }

        #region ARRANGE

        private void CreateDirectoryWithPathAndFilesExtension(string path, string ext, int foldersCount, int filesCount)
        {
            // Get Temp Path
            var tempFolder = path;

            //Recreate already created Temp folder
            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }
            Directory.CreateDirectory(tempFolder);

            // Create subfolder and fill it with files of same extension
            var folders = Enumerable
                .Range(1, foldersCount)
                .Select(f => Path.Combine(tempFolder, f.ToString()))
                .ToList();

            folders
                .ForEach(f => Directory.CreateDirectory(f));

            var files = Enumerable
                .Range(1, filesCount)
                .Select(n => n.ToString() + "." + ext)
                .ToList();

            folders
                .ForEach(fold => files
                    .ForEach(file => File
                        .Create(Path.Combine(fold, file))));
        }

        #endregion
    }
}
