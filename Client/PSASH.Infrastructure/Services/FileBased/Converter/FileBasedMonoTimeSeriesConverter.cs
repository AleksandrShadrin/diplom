using NAudio.Wave;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using System.Data;


namespace PSASH.Infrastructure.Services.FileBased.Converter
{
    public class FileBasedMonoTimeSeriesConverter :
        ITimeSeriesConverter<string, MonoTimeSeries>
    {
        /// <summary>
        /// Конвертирует путь к временному ряду в одиночный временной ряд
        /// </summary>
        /// <param name="input">Путь к файлу</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MonoTimeSeries Convert(string input)
        {
            string ext = Path.GetExtension(input);
            if (!File.Exists(input))
            {
                throw new ThisFileWasNotFound();
            }
            if (ext == ".csv")
            {
                return CSVProcessig(input);
            }
            else if (ext == ".wav")
            {
                return WAVProcessing(input);
            }
            else
            {
                throw new FileExtensionNotSupportedException();
            }

        }

        private MonoTimeSeries WAVProcessing(string input)
        {
            MonoTimeSeries mono;
            using (WaveFileReader wave = new WaveFileReader(input))
            {
                //var mixer = new MixingSampleProvider()
                //WaveFileWriter.CreateWaveFile()
                List<double> valuesWAV = new List<double>();
                byte[] data = new byte[wave.Length];
                int read = wave.Read(data, 0, data.Length);
                for (int i = 0; i < read; i += 2)
                {
                    valuesWAV.Add(BitConverter.ToInt16(data, i) / (double)(wave.WaveFormat.SampleRate * 2));
                }

                TimeSeriesInfo info = new TimeSeriesInfo(Path.GetFileName(Path.GetDirectoryName(input)), Path.GetFileName(input));
                mono = new MonoTimeSeries(valuesWAV, info);
            }
            return mono;
        }

        bool FirstLineIsHeader(string line)
        {
            double num;
            if (double.TryParse(line, out num))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        double TryParseToDouble(string line)
        {
            double num;
            if (double.TryParse(line, out num))
            {
                return num;
            }
            else
            {
                throw new CantConvertToMonoFileException("");
            }
        }
        private MonoTimeSeries CSVProcessig(string input)
        {

            MonoTimeSeries mono;
            TimeSeriesInfo info = new TimeSeriesInfo(Path.GetFileName(Path.GetDirectoryName(input)), Path.GetFileName(input));
            var lines = File.ReadLines(input);

            if (lines.Any() is false)
                return new(Enumerable.Empty<double>(), info);

            if (FirstLineIsHeader(lines.First()))
                lines = lines
                    .Skip(1);

            var values = lines
               .Select(TryParseToDouble)
               .ToList();


            mono = new MonoTimeSeries(values, info);

            return mono;
        }

        private enum AvailableExtensions
        {
            CSV,
            NOT_AVAILABLE
        }

        private AvailableExtensions ConvertToAvailableExtension(string ext)
            => ext switch
            {
                _ => AvailableExtensions.NOT_AVAILABLE,
            };
    }
}
