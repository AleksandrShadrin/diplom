using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Services.FileBased.Converter;


namespace PSOSH.Infrastructure.tests
{

    public class FileBasedMonoTimeSeriesConverterTests
    {
        [Fact]
        public void CanConvertWAV()
        {
            // arrange
            string path2 = "M:\\учеба\\ИТАиОД(Yandex )\\КР\\signal\\2\\test.wav";
            byte[] arr = new byte[] { 1, 4, 3, 7, 5, 8, 7, 0, 9, 4, 4, 5, 6, 2, 3, 4, 5, 0 };
            double[] arr2 = new double[arr.Length];
            int sampleRate = 16000; // наша частота дискретизации.
            for (int i = 0; i < arr.Length; i++)
            {
                arr2[i] = (double)arr[i] / (sampleRate*2);
            }
            CreateElementInPathWav(path2, arr, sampleRate);


            Console.WriteLine("Готово");
            MonoTimeSeries expected = new MonoTimeSeries(arr2, new TimeSeriesInfo("2", "test"));

            //act
            FileBasedMonoTimeSeriesConverter c = new FileBasedMonoTimeSeriesConverter();
            MonoTimeSeries k = c.Convert(path2);


            //assert
            Assert.Equal(expected.TimeSeriesInfo, k.TimeSeriesInfo);
            Assert.Equal(expected.GetValues(), k.GetValues());

        }


        public void CreateElementInPathWav(string path, byte[] arr, int sampleRate)
        {
            if (!File.Exists(path))
            {
                var a = Path.GetDirectoryName(path);
                if (!File.Exists(a))//если не существует файл
                {
                    Directory.CreateDirectory(a);
                    Stream file = File.Create(path); // Создаем новый файл и стыкуем его с потоком.
                    SaveWave(file, arr, sampleRate); // Записываем наши данные в поток.
                    file.Close(); // Закрываем поток.
                }
                else
                {
                    Stream file = File.Create(path); // Создаем новый файл и стыкуем его с потоком.
                    SaveWave(file, arr, sampleRate); // Записываем наши данные в поток.
                    file.Close(); // Закрываем поток.
                }
            }
        }

        public static void SaveWave(Stream stream, byte[] data, int sampleRate)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            short frameSize = (short)(16 / 8); // Количество байт в блоке (16 бит делим на 8).
            writer.Write(0x46464952); // Заголовок "RIFF".
            writer.Write(36 + data.Length * frameSize); // Размер файла от данной точки.
            writer.Write(0x45564157); // Заголовок "WAVE".
            writer.Write(0x20746D66); // Заголовок "frm ".
            writer.Write(16); // Размер блока формата.
            writer.Write((short)1); // Формат 1 значит PCM.
            writer.Write((short)1); // Количество дорожек.
            writer.Write(sampleRate); // Частота дискретизации.
            writer.Write(sampleRate * frameSize); // Байтрейт (Как битрейт только в байтах).
            writer.Write(frameSize); // Количество байт в блоке.
            writer.Write((short)16); // разрядность.
            writer.Write(0x61746164); // Заголовок "DATA".
            writer.Write(data.Length * frameSize); // Размер данных в байтах.
            for (int index = 0; index < data.Length; index++)
            { // Начинаем записывать данные из нашего массива.
                foreach (byte element in BitConverter.GetBytes((short)data[index]))
                { // Разбиваем каждый элемент нашего массива на байты.
                    stream.WriteByte(element); // И записываем их в поток.
                }

            }
        }

        [Fact]
        public void CanConvertCSV()
        {
            // arrange
            string path = "M:\\учеба\\ИТАиОД(Yandex )\\КР\\signal\\6\\w.csv";
            double[] arr = new double[] { 0.871567567, 0.184760951, 0.628541082, 0.909767958, 0.831186251, 0.404781742, 0.836994983, 0.431044954, 0.108830303, 0.617524926, 0.904047623, 0.683696061, 0.953555013, 0.616409076, 0.344512247, 0.510237297, 0.721128554, 0.439679843, 0.924756979, 0.918603854, 0.198465004, 0.885303287, 0.730169609, 0.711043411, 0.215211911, 0.648145145, 0.476799499, 0.025970346, 0.066303381, 0.8257258, 0.78249436, 0.159457655, 0.486581331, 0.552016443, 0.159307466, 0.374340747, 0.513398811, 0.110148876, 0.203138194, 0.18890283, 0.827761074, 0.789498927, 0.465796706, 0.032949532, 0.591453545, 0.229132972, 0.327494219, 0.163854445, 0.975562342, 0.077735501, 0.550017878, 0.913707408, 0.787725093, 0.79064911, 0.089605867, 0.665707163, 0.863957673, 0.387782477, 0.642373059, 0.205773008, 0.189510936, 0.284563429, 0.153293218, 0.597054819, 0.150616939, 0.856001582, 0.064744374, 0.334638416, 0.092427765, 0.755839337, 0.091674384, 0.958332151, 0.896010475, 0.48037807, 0.869424497, 0.101311993, 0.7391595, 0.197097498, 0.704809855, 0.754020052, 0.579353278, 0.119864495, 0.822590032, 0.04937377, 0.53796797, 0.147511173, 0.320299535, 0.997942331, 0.659194057, 0.799064916, 0.852961778, 0.972515108, 0.36578962, 0.254752059, 0.658515533, 0.819531142, 0.467391196, 0.45521084, 0.909743012, 0.344353153, 0.846727974, 0.036807767, 0.567675385, 0.784558258, 0.95499067, 0.451825991, 0.909527282, 0.86350896, 0.253589437, 0.08855389, 0.3821712, 0.198330294, 0.292748854, 0.553800931, 0.713419813, 0.522368727, 0.479110483, 0.161030187, 0.555566022, 0.759809832, 0.041040095, 0.634960282, 0.955945501, 0.482999894, 0.260206936, 0.519075011, 0.371509918, 0.146670683, 0.310550148, 0.028304378, 0.795973001, 0.46867759, 0.064761328, 0.535594142, 0.38843922, 0.251622026, 0.603584485, 0.830295634, 0.23918769, 0.414066411, 0.225353908, 0.979028523, 0.162579375, 0.861125652, 0.615098713, 0.783460521, 0.50763222, 0.53100514, 0.174033648, 0.10159886, 0.707359336, 0.337622609, 0.44103656, 0.433831611, 0.876456027, 0.423674711, 0.964568185, 0.199386601, 0.715349769, 0.505940902, 0.783855721, 0.315537397, 0.432101834, 0.33638972, 0.106047236, 0.681924536, 0.387401894, 0.432994087, 0.789055224, 0.503866117, 0.928632194, 0.698854489, 0.331878176, 0.081033282, 0.287306762, 0.239216665, 0.384897535, 0.831103543, 0.711655278, 0.911418581, 0.063571153, 0.237552412, 0.484275742, 0.272997894, 0.27672875, 0.474982993, 0.726626097, 0.126588222, 0.233744975, 0.078687019, 0.930154717, 0.837344011, 0.447148601, 0.638950325, 0.412845536, 0.959209917, 0.559889163, 0.249027665, 0.454133419, 0.822698377, 0.328313614, 0.733656379, 0.395304824, 0.370032172, 0.070236965, 0.299439791, 0.655067943, 0.852476901, 0.739462365, 0.940366521, 0.751398461, 0.073765494, 0.15711404, 0.981156547, 0.20282814, 0.035375725, 0.532725605, 0.117208776, 0.74516035, 0.807095406};
            MonoTimeSeries expected = new MonoTimeSeries(arr, new TimeSeriesInfo("3", "w"));
            
            //act
            Action action = () =>
            {
                FileBasedMonoTimeSeriesConverter c = new FileBasedMonoTimeSeriesConverter();
                MonoTimeSeries k = c.Convert(path);
            };

            //assert
            //Assert.Equal(expected.TimeSeriesInfo, k.TimeSeriesInfo);
            //Assert.Equal(expected.GetValues(), k.GetValues());
            Assert.Throws<ThisFileWasNotFound>(action);

        }

        public void CreateElemantInPathCSV(string path, double[] arr)
        {
            if (!File.Exists(path))
            {
                var a = Path.GetDirectoryName(path);
                if (!File.Exists(a))//если не существует файл
                {
                    Directory.CreateDirectory(a);
                    var b = File.Create(path);
                    b.Close();
                    StreamWriter sw = new StreamWriter(path);
                    foreach (var item in arr)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                }
                else
                {
                    File.Create(path);
                    var b = File.Create(path);
                    b.Close();
                    StreamWriter sw = new StreamWriter(path);
                    foreach (var item in arr)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                }
            }
        }

    }
}
