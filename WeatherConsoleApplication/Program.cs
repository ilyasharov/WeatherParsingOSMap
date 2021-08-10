using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace WeatherConsoleApplication
{
    class Program {

        public const string BEGIN_OF_URL = "http://api.openweathermap.org/data/2.5/weather?q=";
        public const string END_OF_URL = "&units=metric&appid=eed662e679edbffbea195ad9f83281c4";

        static void Main() {


            Console.WriteLine("Введите название города: ");

            string cityName = Console.ReadLine();

            string url = BEGIN_OF_URL + cityName + END_OF_URL;
            string response = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

            /*******************Преобразование формата времени восхода и заката********************/

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var sunrise = dt.AddSeconds(weatherResponse.Sys.Sunrise).ToLocalTime();
            var sunset = dt.AddSeconds(weatherResponse.Sys.Sunset).ToLocalTime();

            /****************************************************/

            Console.WriteLine("\n");
            Console.WriteLine("Температура в городе {0}: {1} °C\nВлажность: {2}%\nВосход: {3:T}\nЗакат: {4:T}\n",
                weatherResponse.Name, weatherResponse.Main.Temp, weatherResponse.Main.Humidity, sunrise, sunset);

            /*******************Запись в файл********************/

            string filename = DateTime.Now.ToString("dd-MM-yyyy");
            var myFile = File.Create(@"C:\" + filename + ".txt");
            string path = @"C:\" + filename + ".txt";
            myFile.Close();

            try
            {
                using (StreamWriter file = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    file.Write("Погода в городе " + weatherResponse.Name + ":\n\n");
                    file.Write("Температура: " + weatherResponse.Main.Temp + " °C\n");
                    file.Write("Влажность: " + weatherResponse.Main.Humidity + "%\n");
                    file.Write("Восход: " + sunrise + "\n");
                    file.Write("Закат: " + sunset + "\n");
                }

                Console.WriteLine("***********************\n");
                Console.WriteLine("Запись в файл выполнена\n");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            /***************************************/

            Console.WriteLine("Нажмите ENTER для выхода");
            Console.ReadLine();

        }
    }
}
