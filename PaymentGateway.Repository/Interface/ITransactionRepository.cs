using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Datalayer;
using PaymentGateway.Repository.Model;

namespace PaymentGateway.Repository.Interface
{
    public interface ITransactionRepository
    {
        PaymentResponseViewModel PerformSale(OuterMapPaymentViewModel item);

        string MerchantId { get; set; }

        TransactionLog Retrieve(string merchantId);
    }
}
