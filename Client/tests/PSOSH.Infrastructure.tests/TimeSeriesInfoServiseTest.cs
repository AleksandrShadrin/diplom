using PSASH.Core.ValueObjects;
using PSASH.Presentation.Models;
using PSASH.Presentation.Services;

namespace PSOSH.Infrastructure.tests
{
    public class TimeSeriesInfoServiseTest
    {
        [Fact]
        public void CanGetMean()
        {
            // arrange
            List<double> arr = new List<double> { 9, 4, 9, 7, 5, 8, 7 };

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr, TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();

            //assert
            Assert.Equal(7, time.GetMean(expected));

        }

        [Fact]
        public void CanGetStd()
        {
            // arrange
            List<double> arr = new List<double> { 9, 4, 9, 7, 5, 8, 7 };

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr, TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();


            //assert
            Assert.Equal(1.91485, time.GetStd(expected), 5);

        }

        [Fact]
        public void TestForALargeNumberOfElements()
        {
            // arrange
            IEnumerable<double> arr = Enumerable.Repeat(1000000000.0, 10000000);

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr.ToList(), TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();


            //assert
            Assert.Equal(1000000000.0, time.GetMean(expected));
        }

        [Fact]
        public void CanGetCV()
        {
            // arrange
            List<double> arr = new List<double> { 9, 4, 9, 7, 5, 8, 7 };

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr, TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();



            //assert
            Assert.Equal(27.36, time.GetCV(expected), 2);
        }

        [Fact]
        public void CanGetMax()
        {
            // arrange
            List<double> arr = new List<double> { 9, 4, 9, 7, 5, 8, 7 };

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr, TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();


            //assert
            Assert.Equal(9, time.GetMax(expected), 2);
        }

        [Fact]
        public void CanGetMin()
        {
            // arrange
            List<double> arr = new List<double> { 9, 4, 9, 7, 5, 8, 7 };

            //act
            TimeSeriesDto expected = new TimeSeriesDto(arr, TimeSeriesName.FromTimeSeriesInfo(new TimeSeriesInfo("", "")));
            TimeSeriesInfoService time = new TimeSeriesInfoService();


            //assert
            Assert.Equal(4, time.GetMin(expected), 2);
        }
    }
}
