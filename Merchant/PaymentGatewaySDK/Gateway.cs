using Merchant.Model;
using Merchant.PaymentGatewaySDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace PaymentGatewaySDK
{
    public class Gateway : EncryptionPKI
    {
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ClientToken { get; set; }

        protected readonly string baseAPIUrl = "";


        public Gateway()
        {

        }

        internal GatewayClientTokenViewModel GetToken()
        {
            GatewayClientTokenViewModel result =  new GatewayClientTokenViewModel();
            try
            {
                string routePrefix = "api/auth";
                string route = "token";
                string endpoint = Path.Combine(baseAPIUrl, routePrefix, route, MerchantId);

                if (!string.IsNullOrWhiteSpace(MerchantId) || !string.IsNullOrWhiteSpace(endpoint))
                {

                    var httpClient = new HttpClient();
                    var response = httpClient.GetAsync(endpoint).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                       
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<GatewayClientTokenViewModel>(responseContent);

                        if (responseObject != null)
                        {
                            result = new GatewayClientTokenViewModel()
                            {
                                Token = responseObject.Token,
                                IsSuccess = responseObject.IsSuccess,
                                ExceptionDetails = responseObject.ExceptionDetails
                            };
                        }
                    }
                    else
                    {
                        throw new Exception("Fail to retrieve token");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = new GatewayClientTokenViewModel()
                {
                    Token = string.Empty,
                    IsSuccess = false,
                    ExceptionDetails = ex.Message
                };

                return result;
            }
        }

        internal GatewayViewModel CreateSale(PaymentViewModel shopperDetails)
        {
            GatewayViewModel result = new GatewayViewModel();
            try
            {
                string routePrefix = "api/transaction";
                string route = "sale";
                string endpoint = Path.Combine(baseAPIUrl, routePrefix, route);

                if (!string.IsNullOrWhiteSpace(endpoint))
                {
                    GatewayPaymentViewModel postModel = new GatewayPaymentViewModel()
                    {
                        Amount = Encrypt(PublicKey, shopperDetails.Amount.ToString()),
                        CardNumber = Encrypt(PublicKey, shopperDetails.CardNumber.ToString()),
                        ExpiryMonth = Encrypt(PublicKey, shopperDetails.ExpiryMonth.ToString()),
                        ExpiryYear = Encrypt(PublicKey, shopperDetails.ExpiryYear.ToString())
                    };

                    string jsonObject = JsonConvert.SerializeObject(postModel, Formatting.None);
                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClientToken);

                    var response = httpClient.PostAsync(endpoint, content).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<GatewayViewModel>(responseContent);
                        if (responseObject != null)
                        {
                            result = responseObject;
                        }
                    }
                    else
                    {
                        throw new Exception("Fail to create transaction");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = new GatewayViewModel()
                {
                    Details = new GatewayTransactionDetailsViewModel()
                    {
                        IsSuccess = false,
                        ExceptionCode = 400,
                        ExceptionDetails = ex.Message,
                        TransactionDate = DateTime.Now,
                        ExceptionMessage = string.Empty
                    },
                    Payment = new GatewayPaymentViewModel()
                    {
                        Amount = string.Empty,
                        CardNumber = string.Empty,
                        ExpiryMonth = string.Empty,
                        ExpiryYear = string.Empty
                    },
                };

                return result;
            }
        }
    }




}