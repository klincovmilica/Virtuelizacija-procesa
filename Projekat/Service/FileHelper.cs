using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    //provjeravam da li je fajl vec u upotrebi
    public interface ICheckFile
    {
        bool IsFileInUse(string filePath);
    }


    public class FileHelper : ICheckFile
    {
        
            private const int MaxAttempts = 10;
            private const int WaitMilliseconds = 500;

            public bool IsFileInUse(string filePath)
            {
                int attempts = 0;
                while (attempts < MaxAttempts && IsFileAvailable(new FileInfo(filePath)))
                {
                    Thread.Sleep(WaitMilliseconds);
                    attempts++;
                }
                return attempts >= MaxAttempts;
            }

            private bool IsFileAvailable(FileInfo file)
            {
                FileStream fileStream = null;
                try
                {
                    fileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    // Datoteka je nedostupna jer se:
                    // još uvek piše u nju
                    // ili je u obradi od strane druge niti
                    // ili ne postoji (već je obrađena)

                    return true;
                }
                finally
                {
                    fileStream?.Dispose();
                }
                // Datoteka nije zaključana
                return false;
            }
        





    }
}
