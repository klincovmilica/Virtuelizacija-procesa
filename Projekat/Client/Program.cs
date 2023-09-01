using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ILoadService> factory = new ChannelFactory<ILoadService>("LoadService");

            string folderPath = ConfigurationManager.AppSettings["folderPath"];
            if (string.IsNullOrEmpty(folderPath))
            {
                Console.WriteLine("Putanja za čuvanje fajlova nije dodata u konfiguraciju");
                return;
            }
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var csv = new CSVFile(folderPath);
            while (true)
            {
                Console.WriteLine("Unesite datum: ");
                string input = Console.ReadLine();
                if (!DateTime.TryParse(input, out DateTime date))
                {
                    Console.WriteLine("Nevalidan datum");
                    continue;
                }
                var loadService = factory.CreateChannel();
                var serviceResult = loadService.GetLoads(date);
                PrintAudits(serviceResult.Audits);
                SaveResults(serviceResult.Loads, date, csv, folderPath);
            }
        }

        static void PrintAudits(List<Audit> audits)
        {
            foreach (var item in audits)
            {
                Console.WriteLine($"[{item.MessageType}] {item.Message}");
            }
        }

        static void SaveResults(List<Load> loads, DateTime date, CSVFile csv, string folderPath)
        {
            if (loads != null && loads.Count != 0)
            {
                string fileName = $"result_{date:yyyy_MM_dd}.csv";
                csv.Save(loads, fileName);
                Console.WriteLine("Rezultati su u file-u na putanji " + Path.Combine(folderPath, fileName));
            }





        }
    }
}
