using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CSVFile
    {
        private readonly string folderPath;

        public CSVFile(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public void Save(List<Load> loads, string name)
        {
            string csv = CreateCsvContent(loads);

            SaveCsvToFile(csv, name);
        }

        private string CreateCsvContent(List<Load> loads)
        {
            var csv = new StringBuilder();
            csv.AppendLine("TIME_STAMP,,FORECAST_VALUE,MEASURED_VALUE");

            foreach (var load in loads)
            {
                var timestamp = load.Timestamp.ToString("yyyy-MM-dd");
                var time = load.Timestamp.ToString("hh:mm");
                var forecastValue = load.ForecastValue.ToString(CultureInfo.InvariantCulture);
                var measuredValue = load.MeasuredValue.ToString(CultureInfo.InvariantCulture);

                csv.AppendLine($"{timestamp},{time},{forecastValue},{measuredValue}");
            }

            return csv.ToString();
        }

        private void SaveCsvToFile(string csvContent, string name)
        {
            var filePath = Path.Combine(folderPath, name);

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(csvContent);
            }
        }
    }
}
