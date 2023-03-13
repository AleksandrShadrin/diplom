using Microsoft.VisualBasic.FileIO;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using NAudio;
using System;
using NAudio.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
            if (!File.Exists(input))
            {
                throw new ThisFileWasNotFound();
            }
            string ext = Path.GetExtension(input);
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
            using (NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(input))
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

        private MonoTimeSeries CSVProcessig(string input)
        {
            
            MonoTimeSeries mono;

            using (var parser = new TextFieldParser(input))
            {
                //
                List<double> valuesCSV = new List<double>();
                var delimiter = ";";

                // Получить текст файла.
                //var whole_file = File.ReadAllText(input);

                //for (int i = 0; i < whole_file.Length; i++)
                //{
                //    var lineSt = line.Split(';');

                //    if (lineSt.Length == 1) 
                //    {
                //        valuesCSV.Add(ParseToDouble(lineSt.First(), input));
                //    }
                //}

                // Получить текст файла.
                string whole_file = File.ReadAllText(input);

                // Разделение на строки.
                whole_file = whole_file.Replace('\n', '\r');
                string[] lines = whole_file.Split(new char[] { '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                // Посмотрим, сколько строк и столбцов есть.
                double num;
                int num_cols = lines[0].Split('.').Length;
                if (num_cols == 1)
                {
                    if (double.TryParse(lines[0], out num))
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            valuesCSV.Add(double.Parse(lines[i]));
                        }

                    }
                    else
                    {
                        for (int i = 1; i < lines.Length; i++)
                        {
                            valuesCSV.Add(double.Parse(lines[i]));
                        }
                    }
                }
                else
                {
                    throw new CantConvertToMonoFileException(input);
                }

                TimeSeriesInfo info = new TimeSeriesInfo(Path.GetFileName(Path.GetDirectoryName(input)), Path.GetFileName(input));
                mono = new MonoTimeSeries(valuesCSV, info);
            }

            return mono;
        }

        //private double ParseToDouble(string str, string input)
        //{
        //    double number;
        //    if (double.TryParse(str, CultureInfo.InvariantCulture, out number))
        //        return number;
        //    throw new CantConvertToMonoFileException(input);
        //}
        

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
