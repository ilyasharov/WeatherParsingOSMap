using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherConsoleApplication
{
    public class WeatherResponse
    {
        public TemperatureInfo Main { get; set; }
        public SunriseInfo Sys { get; set; }
        public string Name { get; set; }

    }
}
