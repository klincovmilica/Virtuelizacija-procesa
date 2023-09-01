using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class IMDatabase
    {
        private readonly Dictionary<DateTime, List<Load>> loadsbase = new Dictionary<DateTime, List<Load>>();
        private readonly Dictionary<DateTime, DateTime> expireTimes = new Dictionary<DateTime, DateTime>();
        private readonly Dictionary<long, Audit> auditbase = new Dictionary<long, Audit>();
        private readonly object lockObject = new object();
        private readonly int dataTimeout = 15;

        public IMDatabase()
        {
            int timeout;
            if (int.TryParse(ConfigurationManager.AppSettings["DateTimeout"], out timeout) && timeout > 0)
            {
                dataTimeout = timeout;
            }
        }

        public List<Load> GetLoads(DateTime date)
        {
            lock (lockObject)
            {
                return loadsbase.TryGetValue(date.Date, out var loads) ? loads : null;
            }
        }

        public void AddLoads(DateTime date, List<Load> loads)
        {
            lock (lockObject)
            {
                loadsbase[date.Date] = loads;
                expireTimes[DateTime.Now.AddMinutes(dataTimeout)] = date.Date;
            }
        }


        public void AddAudits(List<Audit> audits)
        {
            lock (lockObject)
            {
                long id = auditbase.Count != 0 ? auditbase.Values.Max(x => x.Id) + 1 : 1;

                foreach (var audit in audits)
                {
                    audit.Id = id++;
                    auditbase[audit.Id] = audit;
                }
            }
        }


        //prolazim kroz expireTimes i dodajem kljuceve koje treba ukloniti
        //u listu keysToRemove, a zatim ponovo prolazim kroz istu listu da bih uklonila kljuceve iz 
        //loadsDatabase i expireTimes
        public void DeleteOldLoads()
        {
            lock (lockObject)
            {
                DateTime now = DateTime.Now;
                var keysToRemove = new List<DateTime>();

                foreach (var item in expireTimes)
                {
                    if (item.Key <= now)
                    {
                        keysToRemove.Add(item.Key);
                    }
                }

                foreach (var key in keysToRemove)
                {
                    loadsbase.Remove(expireTimes[key]);
                    expireTimes.Remove(key);
                }
            }
        }

       

    }
}
