using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DatabaseHelper
    {
        private readonly IMDatabase imDatabase;
        private readonly XMLDatabase xmlDatabase;

        public DatabaseHelper(IMDatabase imDatabase, XMLDatabase xmlDatabase)
        {
            this.imDatabase = imDatabase;
            this.xmlDatabase = xmlDatabase;
        }


        public Result GetLoads(DateTime date)
        {
            Result result = new Result();
            result.Loads = imDatabase.GetLoads(date);

            string message;
            if (result.Loads == null)
            {
                result = xmlDatabase.GetLoads(date);
                if (result.Loads != null && result.Loads.Count > 0)
                {
                    imDatabase.AddLoads(date, result.Loads);
                    message = $"Load objekti za datum {date.ToShortDateString()} su preuzeti iz XML baze";
                }
                else
                {
                    message = $"Load objekti za datum {date.ToShortDateString()} ne postoje";
                }
            }
            else
            {
                message = $"Load objekti za datum {date.ToShortDateString()} su preuzeti iz in-memory baze";
            }

            result.Audits.Add(new Audit { Message = message, MessageType = MessageType.INFO });
            imDatabase.AddAudits(result.Audits);
            xmlDatabase.AddAudits(result.Audits);

            return result;
        }


    }
}
