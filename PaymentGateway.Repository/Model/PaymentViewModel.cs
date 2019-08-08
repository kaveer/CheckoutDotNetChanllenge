using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Model
{
    class PaymentViewModel
    {
        public int CardNumber { get; set; }
        //for amount better use decimal accounding to my experience and link: http://net-informations.com/q/faq/float.html
        //for performance use doucle but if dealing with money use decimal link: https://exceptionnotfound.net/decimal-vs-double-and-other-tips-about-number-types-in-net/
        public decimal Amount { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }

    public class OuterMapPaymentViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }
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
        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
    }

    public class GatewayTransactionDetailsViewModel
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class GatewayMerchantViewModel
    {
        public int MerchantId { get; set; }
        public decimal AmountCredited { get; set; }
        public DateTime DateCredited { get; set; }
        public decimal TotalAmount { get; set; }
    }




}
