using Bank.Repository.Interface;
using Bank.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Repository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        public bool AddCard(GatewayPaymentViewModel item)
        {
            throw new NotImplementedException();
        }

        public PaymentResponseViewModel SalesTransaction(PaymentResponseViewModel item)
        {
            throw new NotImplementedException();
        }
    }
}
