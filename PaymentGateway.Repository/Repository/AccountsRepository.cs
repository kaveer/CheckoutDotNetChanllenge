using PaymentGateway.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        public string GenerateToken(string merchantId)
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(merchantId))
                return string.Empty;

            return result;
        }
    }
}
