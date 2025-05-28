using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherHistoryItem
    {
        public string City { get; set; }
        public DateTime RequestTime { get; set; }
        public double Temperature { get; set; }
    }
}
