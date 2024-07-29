using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechRecord.Models
{
    public class MethodsReuseability
    {
        static AptechSFCRecordEntities db = new AptechSFCRecordEntities();
        public static void ErrorMessage(string message, string details)
        {
            DateTime? datetime = Convert.ToDateTime(DateTime.Now.AddHours(9.00000));
            var id = Convert.ToInt32(db.usp_auto_errorid().Single());
            db.usp_error_log(id, message, details, datetime);
        }
    }
}