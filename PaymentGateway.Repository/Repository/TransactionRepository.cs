using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
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

        protected readonly string baseAPIUrl = "http://localhost:64624";

        public PaymentResponseViewModel PerformSale(OuterMapPaymentViewModel item)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel();
            string applicationLogType;

            PaymentViewModel decryptedData = DecryptedData(item);
            if (decryptedData.CardNumber == 0 || decryptedData.Amount == 0)
                return null;

            result = AcquiringBankRequest(decryptedData);
            if (result.Details.IsSuccess)
                applicationLogType = Constants.ApplicationLogType.Transaction_Sales_Bank_Response_Success.ToString();
            else
                applicationLogType = Constants.ApplicationLogType.Transaction_Sales_Bank_Response_Fail.ToString();

            //save transaction here
            CommonAction.Log(applicationLogType, result?.Details?.Message);

            return result;
        }

        /// Since the requirement of this challenge is to Simulating the bank and due to time constraint 
        /// thus i am not performing encryption of response between payment gateway and bank
        private PaymentResponseViewModel AcquiringBankRequest(PaymentViewModel item)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel();

            string routePrefix = "api/bank";
            string route = "sales";
            string endpoint = Path.Combine(baseAPIUrl, routePrefix, route);

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                PaymentResponseViewModel jasonModel = new PaymentResponseViewModel()
                {
                    TransactionId = "0",
                    Details = new GatewayTransactionDetailsViewModel(),
                    Merchant = GetMerchantDetails(),
                    Payment = item
                };

                string jsonObject = JsonConvert.SerializeObject(jasonModel, Formatting.None);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();

                var response = httpClient.PostAsync(endpoint, content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var responseObject = JsonConvert.DeserializeObject<PaymentResponseViewModel>(responseContent);
                    if (responseObject != null)
                        result = responseObject;
                    else
                        result = InvalidTransaction("Fail to retrieve data from bank");
                }
                else
                    result = InvalidTransaction("Bank transaction fail");
            }

            return result;
        }

        private PaymentResponseViewModel InvalidTransaction(string message, string details = "")
        {
            if (string.IsNullOrWhiteSpace(details))
                details = message;

            PaymentResponseViewModel result = new PaymentResponseViewModel()
            {
                TransactionId = "0",
                Details = new GatewayTransactionDetailsViewModel()
                {
                    IsSuccess = false,
                    TransactionDate = DateTime.Now,
                    Details = details,
                    Code = 1503,
                    Message = message
                },
                Merchant = new GatewayMerchantViewModel(),
                Payment = new PaymentViewModel()
            };

            return result;
        }

        private GatewayMerchantViewModel GetMerchantDetails()
        {
            GatewayMerchantViewModel result = new GatewayMerchantViewModel();

            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                var merchantData = context.MerchantDetails?
                                    .Where(x => x.MerchantId.Trim() == MerchantId.Trim())
                                    .FirstOrDefault();

                if (merchantData != null)
                {
                    result = new GatewayMerchantViewModel()
                    {
                        CardNumber = long.Parse(merchantData.CardNumber),
                        ExpiryMonth = merchantData.ExpiryMonth,
                        ExpiryYear = merchantData.ExpiryYear
                    };
                }
            }

            return result;
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
                            CardNumber = long.Parse(Decrypt(keys.PrivateKey, item.CardNumber)),
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
