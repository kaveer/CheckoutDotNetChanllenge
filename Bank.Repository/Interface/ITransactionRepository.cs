using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Repository.Model;

namespace Bank.Repository.Interface
{
    public interface ITransactionRepository
    {
        bool AddCard(GatewayPaymentViewModel item);
        PaymentResponseViewModel SalesTransaction(PaymentResponseViewModel item);
    }
}
