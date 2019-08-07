using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.PaymentGatewaySDK
{
    public class GatewayViewModel
    {
        public string TrnsactionId { get; set; }
        public GatewayTransactionDetailsViewModel Details { get; set; }
        public GatewayPaymentViewModel Payment { get; set; }
    }

    public class GatewayPaymentViewModel
    {
        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
    }

    public class GatewayClientTokenViewModel
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionDetails { get; set; }
    }

    public class GatewayTransactionDetailsViewModel
    {
        public bool IsSuccess { get; set; }
        public int ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionDetails { get; set; }
        public DateTime TransactionDate { get; set; }
    }


}