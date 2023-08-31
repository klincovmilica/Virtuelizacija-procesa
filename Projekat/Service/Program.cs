using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Konfiguracija i pokretanje WCF servisa
            using (ServiceHost host = new ServiceHost(typeof(LoadService)))
            {
                host.Open();
                Console.WriteLine("Server je pokrenut. Pritisnite Enter za zaustavljanje.");
                Console.ReadLine();
                host.Close();
            }

        }
    }
}
