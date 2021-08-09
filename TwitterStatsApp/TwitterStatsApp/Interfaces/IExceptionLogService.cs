using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStatsApp.Interfaces
{
   public interface IExceptionLogService
    {
       void LogException(Exception ex);
    }
}
