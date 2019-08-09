using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Repository.Model
{
    class BankTransactionViewModel
    {
    }

    public class PaymentResponseViewModel
    {
        public string TransactionId { get; set; }
        public GatewayTransactionDetailsViewModel Details { get; set; }
        public GatewayPaymentViewModel Payment { get; set; }
        public GatewayMerchantViewModel Merchant { get; set; }
    }

    public class GatewayPaymentViewModel
    {
        public long CardNumber { get; set; }
        //for amount better use decimal accounding to my experience and link: http://net-informations.com/q/faq/float.html
        //for performance use doucle but if dealing with money use decimal link: https://exceptionnotfound.net/decimal-vs-double-and-other-tips-about-number-types-in-net/
        public decimal Amount { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }

    public class GatewayTransactionDetailsViewModel
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class GatewayMerchantViewModel: GatewayMerchantDetailsViewModel
    {
        public int MerchantId { get; set; }
        public decimal AmountCredited { get; set; }
        public DateTime DateCredited { get; set; }
    }

    public class GatewayMerchantDetailsViewModel
    {
        public long CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
