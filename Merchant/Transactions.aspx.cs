using Merchant.Model;
using Merchant.PaymentGatewaySDK;
using PaymentGatewaySDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Merchant
{
    public partial class Transactions : System.Web.UI.Page
    {
        protected bool IsConfigurationValid;
        protected string merchantId;
        protected string privateKey;
        protected string publicKey;

        public Transactions()
        {
            IsConfigurationValid = ReadConfiguration();
        }

        protected bool ReadConfiguration()
        {
            bool result = true;

            merchantId = ConfigurationManager.AppSettings["Merchant:Id"].ToString();
            privateKey = ConfigurationManager.AppSettings["Merchant:PrivateKey"].ToString();
            publicKey = ConfigurationManager.AppSettings["Merchant:PublicKey"].ToString();

            if (string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
                return false;

            return result;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsConfigurationValid)
                {
                    List<TransactionLog> result = new List<TransactionLog>();
                    List<TransactionOuterViewModel> transactions = new List<TransactionOuterViewModel>();
                    List<TransactionOuterViewModel> previousTransaction = new List<TransactionOuterViewModel>();

                    Gateway transaction = new Gateway()
                    {
                        MerchantId = merchantId,
                        PrivateKey = privateKey,
                        PublicKey = publicKey
                    };

                    var clientAuth = transaction.GetToken();
                    if (!clientAuth.IsSuccess)
                        throw new Exception(clientAuth.ExceptionDetails);

                    transaction.ClientToken = clientAuth.Token;
                    result = transaction.RetrieveTransactionByMerchantId(merchantId);

                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            transactions.Add(new TransactionOuterViewModel()
                            {
                                Card_Number = item.CardNumber,
                                Credited = item.AmountCredited,
                                CVC = item.CVC,
                                Details = item.BankResponse,
                                Expiry_Month = item.ExpiryMonth,
                                Expiry_Year = item.ExpiryYear,
                                Identifiyer = item.TransactionIdentifiyer,
                                Status = item.IsSuccess == true ? "Success" : "Fail",
                                Transaction_Date = item.TransactionDate
                            });
                        }

                        var secondRecord = result.ElementAt(0);
                        previousTransaction.Add(new TransactionOuterViewModel()
                        {
                            Card_Number = secondRecord.CardNumber,
                            Credited = secondRecord.AmountCredited,
                            CVC = secondRecord.CVC,
                            Details = secondRecord.BankResponse,
                            Expiry_Month = secondRecord.ExpiryMonth,
                            Expiry_Year = secondRecord.ExpiryYear,
                            Identifiyer = secondRecord.TransactionIdentifiyer,
                            Status = secondRecord.IsSuccess == true ? "Success" : "Fail",
                            Transaction_Date = secondRecord.TransactionDate
                        });
                    }


                    grd_transaction.DataSource = transactions;
                    grd_transaction.DataBind();

                    grd_previous.DataSource = previousTransaction;
                    grd_previous.DataBind();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DataTable ConvertListToDataTable(List<string[]> list)
        {
            DataTable table = new DataTable();

            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                    columns = array.Length;
            }

            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }
    }
}