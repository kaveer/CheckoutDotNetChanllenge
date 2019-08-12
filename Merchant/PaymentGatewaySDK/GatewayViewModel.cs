using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.PaymentGatewaySDK
{
    public class GatewayPaymentViewModel
    {
        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVC { get; set; }
    }

    public class GatewayViewModel
    {
        public string TransactionId { get; set; }
        public GatewayTransactionDetailsViewModel Details { get; set; }
        public GatewayPaymentViewModel Payment { get; set; }
        public GatewayMerchantViewModel Merchant { get; set; }

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
        public int Code { get; set; }
        public string Message { get; set; }
        public string MessageDetails { get; set; }
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

    public class TransactionLog
    {
        public int TransactionId { get; set; }
        public string TransactionIdentifiyer { get; set; }
        public string MerchantId { get; set; }
        public Nullable<bool> IsSuccess { get; set; }
        public Nullable<decimal> AmountCredited { get; set; }
        public string BankResponse { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public int CVC { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }


}