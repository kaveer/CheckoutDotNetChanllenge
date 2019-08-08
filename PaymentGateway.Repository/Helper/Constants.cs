using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Helper
{
    public class Constants
    {
        public enum ApplicationLogType
        {
            Token_Create_Success = 1,
            Token_Create_Fail = 2,
            Application_Error = 3,
            Transaction_Sales_Token_Invalid = 4,
            Transaction_Sale_Card_invalid = 5,
            Transaction_Sales_Card_Decrypt_Fail = 6
        }
    }
}
