using System;
using System.Collections.Generic;
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

            SaveTransaction(result);
            CommonAction.Log(applicationLogType, result?.Details?.Message);

            return result;
        }

        private void SaveTransaction(PaymentResponseViewModel result)
        {
            using (PaymentGatewayEntities contect = new PaymentGatewayEntities())
            {
                TransactionLog transaction = new TransactionLog()
                {
                    AmountCredited = result.Payment.Amount,
                    BankResponse = result.Details.Message,
                    IsSuccess = result.Details.IsSuccess,
                    TransactionDate = DateTime.Now,
                    MerchantId = MerchantId,
                    TransactionIdentifiyer = Guid.NewGuid().ToString(),
                    CardNumber = result.Payment.CardNumber.ToString(),
                    CVC= result.Payment.CVC,
                    ExpiryMonth = result.Payment.ExpiryMonth,
                    ExpiryYear = result.Payment.ExpiryYear
                };

                contect.TransactionLogs.Add(transaction);
                contect.SaveChanges();
            }
        }

        /// Since the requirement of this challenge is to Simulating the bank and due to time constraint 
        /// thus i am not performing encryption of response between payment gateway and bank
        private PaymentResponseViewModel AcquiringBankRequest(PaymentViewModel item)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel();

            string routePrefix = "api/bank";
            string route = "sales";
            string endpoint = @"http://localhost:64624/api/bank/sales";

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                PaymentResponseViewModel jasonModel = new PaymentResponseViewModel()
                {
                    TransactionId = "0",
                    Details = new GatewayTransactionDetailsViewModel()
                    {
                        TransactionDate = DateTime.Now,
                        Code = 5321,
                        Details = "",
                        IsSuccess = false,
                        Message = ""
                    },
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
                        ExpiryYear = merchantData.ExpiryYear,
                        CVC = merchantData.CVC,
                        AmountCredited = 1,
                        DateCredited = DateTime.Now,
                        MerchantId = 0,
                        TotalAmount = 10000

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
                            ExpiryYear = Convert.ToInt32(Decrypt(keys.PrivateKey, item.ExpiryYear)),
                            CVC = Convert.ToInt32(Decrypt(keys.PrivateKey, item.CVC))
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

        public List<TransactionLog> Retrieve(string merchantId)
        {
            List<TransactionLog> result = new List<TransactionLog>();

            if (merchantId != MerchantId)
                return result;

            if (string.IsNullOrWhiteSpace(merchantId))
                return null;

            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                var transaction = context.TransactionLogs
                                            .Where(x => x.MerchantId == merchantId)
                                            .OrderByDescending(x=>x.TransactionDate)
                                            .ToList();

                if (transaction == null)
                    return result;

                foreach (var item in transaction)
                {
                    result.Add(new TransactionLog() {
                        CardNumber = MaskDetails(item.CardNumber),
                        AmountCredited = item.AmountCredited,
                        BankResponse = item.BankResponse,
                        CVC = item.CVC,
                        ExpiryMonth = item.ExpiryMonth,
                        ExpiryYear = item.ExpiryYear,
                        IsSuccess = item.IsSuccess,
                        TransactionDate = item.TransactionDate,
                        TransactionIdentifiyer = item.TransactionIdentifiyer,
                        MerchantId = item.MerchantId
                    });
                }

                return result;
            }
        }

        private string MaskDetails(string cardNumber)
        {
            int unmaskedChar = 3;
            string result = string.Empty;
            char[] ch = cardNumber.ToCharArray();
            int length = ch.Length - unmaskedChar;
            int count = 2;

            while (count != length)
            {
                ch[count] = 'X';
                count++;
            }


            result = new string(ch);
            return result;
        }
    }
}
