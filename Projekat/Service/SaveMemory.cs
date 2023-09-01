using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class SaveMemory
    {
        // Definisanje delegata za brisanje
        public delegate void DeleteDelegate();

        // Metoda za pokretanje pozadinskog brisanja
        internal static void Start()
        {
            // Kreiranje instance InMemoryDatabaseContext
            IMDatabase imDatabase = new IMDatabase();

            // Kreiranje delegata za brisanje
            DeleteDelegate Delete = new DeleteDelegate(imDatabase.DeleteOldLoads);

            // Beskonačna petlja koja čeka 1 sekundu i zatim poziva brisanje
            while (true)
            {
                Task.Delay(1000).Wait();
                Delete();
            }
        }

    }
}
