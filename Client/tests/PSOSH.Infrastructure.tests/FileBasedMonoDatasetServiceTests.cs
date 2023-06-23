using FluentAssertions;
using NSubstitute;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
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
            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

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

            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            var act = () =>
            {
                fileBasedMonoDatasetService.SetPath(tempPath);
                fileBasedMonoDatasetService.LoadDataset();
            };

            // Assert
            act.Should()
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

            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

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

        [Fact]
        public void Dataset_With_Files_Of_Different_Extensions_Should_Throw_DatasetStructureInvalidException()
        {
            // Arrange
            var name = "Dataset_With_Files_Of_Different_Extensions_Should_Throw_DatasetStructureInvalidException";
            var count = 10;
            var tempPath = Path
                .Combine(Path.GetTempPath(),
                name);

            CreateDirectoryWithPathAndFilesExtension(tempPath,
                "ext",
                count,
                count);

            File.Create(Path.Combine(tempPath, "1", "1" + ".incorrect"));

            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            fileBasedMonoDatasetService.SetPath(tempPath);
            var act = () => fileBasedMonoDatasetService.LoadDataset();

            // Assert
            act.Should()
                .Throw<DatasetStructureInvalidException>("because dataset structure is invalid");
        }

        [Fact]
        public void When_Dataset_Not_Loaded_LoadTimeSeries_Should_Throw_DatasetNotLoadedException()
        {
            // Arrange
            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            var act = () => fileBasedMonoDatasetService.LoadTimeSeries(new
                TimeSeriesInfo("1", "1"));

            // Assert 
            act.Should()
                .Throw<DatasetNotLoadedException>("Dataset wasn't loaded");
        }

        [Fact]
        public void On_LoadTimeSeries_TimeSeriesConverter_Should_Get_Valid_FileName()
        {
            // Arrange
            var name = "On_LoadTimeSeries_TimeSeriesConverter_Should_Get_Valid_FileName";
            var count = 10;
            var tempPath = Path
                .Combine(Path.GetTempPath(),
                name);

            CreateDirectoryWithPathAndFilesExtension(tempPath,
                "ext",
                count,
                count);

            var timeSeriesInfo = new TimeSeriesInfo("1", "1");

            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

            converter
                .Convert(Arg.Any<string>())
                .Returns(new MonoTimeSeries(Enumerable.Empty<double>(), timeSeriesInfo));

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            fileBasedMonoDatasetService.SetPath(tempPath);
            fileBasedMonoDatasetService.LoadDataset();
            fileBasedMonoDatasetService.LoadTimeSeries(timeSeriesInfo);


            // Assert
            converter
                .Received(1)
                .Convert($"{tempPath}{Path.DirectorySeparatorChar}1{Path.DirectorySeparatorChar}1.ext");
        }

        [Fact]
        public void When_TimeSeriesInfo_Of_Dont_Existed_File_LoadTimeSeries_Should_Throw_TimeSeriesDontExistException()
        {
            // Arrange
            var name = "When_TimeSeriesInfo_Of_Dont_Existed_File_LoadTimeSeries_Should_Throw_TimeSeriesDontExistException";
            var count = 10;
            var tempPath = Path
                .Combine(Path.GetTempPath(),
                name);

            CreateDirectoryWithPathAndFilesExtension(tempPath,
                "ext",
                count,
                count);

            var timeSeriesInfo = new TimeSeriesInfo("1", "11");

            var converter = Substitute.For<IFileBasedMonoTimeSeriesConverter>();

            var fileBasedMonoDatasetService =
                new FileBasedMonoDatasetService(converter);

            // Act
            fileBasedMonoDatasetService.SetPath(tempPath);
            fileBasedMonoDatasetService.LoadDataset();
            var act = () => fileBasedMonoDatasetService.LoadTimeSeries(timeSeriesInfo);


            // Assert
            act.Should()
                .Throw<TimeSeriesDontExistException>()
                .Which
                .Message
                .Should()
                .Be("TimeSereis with class: 1 and id: 11 don't exist", "because class was given as 1 and id as 11");
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
