using Merchant.Model;
using Merchant.PaymentGatewaySDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGatewaySDK
{
    public class Gateway : EncryptionPKI
    {
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ClientToken { get; set; }


        public Gateway()
        {

        }

        internal GatewayClientTokenViewModel GetToken()
        {
            throw new NotImplementedException();
        }

        internal GatewayViewModel CreateSale(PaymentViewModel shopperDetails)
        {

            throw new NotImplementedException();
        }
    }




}