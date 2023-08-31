using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Load
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double ForecastValue { get; set; }
        public double MeasuredValue { get; set; }


        public Load()
        {
            Id = 0;
            Timestamp = DateTime.Now;
            ForecastValue = 0;
            MeasuredValue = 0;
        }

        public Load(long id, DateTime timestamp, double forecastValue, double measuredValue)
        {
            Id = id;
            Timestamp = timestamp;
            ForecastValue = forecastValue;
            MeasuredValue = measuredValue;
        }

    }
}
