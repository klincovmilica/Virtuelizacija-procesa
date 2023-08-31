using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MessageType
    {
        INFO,
        WARNING,
        ERROR
    }

    public enum ResultTypes
    {
        SUCCESS,
        FAILED
    }

    public enum DatabaseType
    {
        XML,
        INMEMORY
    }
}
