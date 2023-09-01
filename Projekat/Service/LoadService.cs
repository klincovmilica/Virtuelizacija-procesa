using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LoadService : ILoadService
    {
        DatabaseHelper databaseHelper = new DatabaseHelper(new IMDatabase(), new XMLDatabase(new FileHelper()));
        public Result GetLoads(DateTime dateTime)
        {
            return databaseHelper.GetLoads(dateTime);
        }

    }
}
