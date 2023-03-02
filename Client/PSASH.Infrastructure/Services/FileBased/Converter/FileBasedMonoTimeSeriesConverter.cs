using Microsoft.VisualBasic.FileIO;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using NAudio;

namespace PSASH.Infrastructure.Services.FileBased.Converter
{
    internal class FileBasedMonoTimeSeriesConverter :
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
            if (ext == ".csv") {
                return CSVProcessig(input);
            }
            if (ext == ".wav")
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
            using (NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(input))
            {
                List<double> valuesWAV = new List<double>();
                byte[] data = new byte[wave.Length];
                int read = wave.Read(data, 0, data.Length);
                for (int i = 0; i < read; i += 2)
                {
                    valuesWAV.Add(BitConverter.ToInt16(data, i) / (wave.WaveFormat.SampleRate * 2));
                }
                string[] parserPath = input.Split(new char[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
                TimeSeriesInfo info = new TimeSeriesInfo(parserPath[parserPath.Length - 1], parserPath[parserPath.Length]);
                mono = new MonoTimeSeries(valuesWAV, info);
            }
            return mono;
        }

        private MonoTimeSeries CSVProcessig(string input)
        {
            MonoTimeSeries mono;
            using (var parser = new TextFieldParser(input))
            {
                List<double> valuesCSV = new List<double>();
                var delimiter = ";";
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length > 1)
                    {
                        throw new CantConvertToMonoFileException(input);
                    }
                    if (double.TryParse(fields[0], CultureInfo.InvariantCulture, out var result))
                    {
                        valuesCSV.Add(result);
                    }
                    else
                    {
                        throw new CantConvertToMonoFileException(input);

                    }
                }

                string[] parserPath = input.Split(new char[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
                TimeSeriesInfo info = new TimeSeriesInfo(parserPath[parserPath.Length - 1], parserPath[parserPath.Length]);
                mono = new MonoTimeSeries(valuesCSV, info);
            }
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
