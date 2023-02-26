using Microsoft.VisualBasic.FileIO;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

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
            List<double> valuesCSV = new List<double>();
            var delimiter = ";";
            using (var parser = new TextFieldParser(input))
            {
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
                TimeSeriesInfo info = new TimeSeriesInfo(parserPath[parserPath.Length-1], parserPath[parserPath.Length]);
                var mono = new MonoTimeSeries(valuesCSV, info);
                return mono;

            }
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
