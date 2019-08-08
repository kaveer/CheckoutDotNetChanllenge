using PaymentGateway.Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Helper
{
    public static class CommonAction
    {
        public static void Log(string type, string details)
        {
            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                ApplicationLog data = new ApplicationLog()
                {
                    LogType = type,
                    Details = details,
                    DateCreated = DateTime.Now
                };

                context.ApplicationLogs.Add(data);
                context.SaveChanges();
            }
        }
    }
}
