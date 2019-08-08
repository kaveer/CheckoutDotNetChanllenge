using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using PaymentGateway.Datalayer;
using PaymentGateway.Repository.Helper;
using PaymentGateway.Repository.Interface;
using PaymentGateway.Repository.Model;

namespace PaymentGateway.Repository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        public string MerchantId { get; set; }

        public PaymentResponseViewModel PerformSale(OuterMapPaymentViewModel item)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel();

            PaymentViewModel decryptedData = DecryptedData(item);
            if (decryptedData.CardNumber == 0 || decryptedData.Amount == 0)
                return null;

            result = AcquiringBankRequest(item);

            return result;
        }

        /// Since the requirement of this challenge is to Simulating the bank and due to time constraint thus i am not performing encryption of response between payment gateway and bank
        private PaymentResponseViewModel AcquiringBankRequest(OuterMapPaymentViewModel item)
        {
            throw new NotImplementedException();
        }

        private PaymentViewModel DecryptedData(OuterMapPaymentViewModel item)
        {
            PaymentViewModel result = new PaymentViewModel();

            try
            {
                using (PaymentGatewayEntities context = new PaymentGatewayEntities())
                {
                    var merchantData = context.Merchants?
                                        .Where(x => x.MerchantId.Trim() == MerchantId.Trim())
                                        .FirstOrDefault();

                    var keys = context.MerchantKeys?
                                        .Where(x => x.merId == merchantData.merId)
                                        .FirstOrDefault();

                    if (keys != null)
                    {
                        result = new PaymentViewModel()
                        {
                            Amount = Convert.ToDecimal(Decrypt(keys.PrivateKey, item.Amount)),
                            CardNumber = Convert.ToInt32(Decrypt(keys.PrivateKey, item.CardNumber)),
                            ExpiryMonth = Convert.ToInt32(Decrypt(keys.PrivateKey, item.ExpiryMonth)),
                            ExpiryYear = Convert.ToInt32(Decrypt(keys.PrivateKey, item.ExpiryYear))
                        };
                    }
                }
            }
            catch (Exception)
            {
                CommonAction.Log(Constants.ApplicationLogType.Transaction_Sales_Card_Decrypt_Fail.ToString(), "Fail to decrypt data");
                result.Amount = 0;
                result.CardNumber = 0;
            }
           

            return result;
        }

        protected string Decrypt(string pvtKey, string txtToDecrypt)
        {
            RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(pvtKey));
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            RSAParameters rsaParameters2 = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)privateKey);

            rsa.ImportParameters(rsaParameters2);

            byte[] dec = rsa.Decrypt(Convert.FromBase64String(txtToDecrypt), false);
            string decStr = Encoding.UTF8.GetString(dec);

            return decStr;
        }
    }
}
