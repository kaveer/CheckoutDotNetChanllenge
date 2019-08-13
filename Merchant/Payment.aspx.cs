using Merchant.Model;
using PaymentGatewaySDK;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace Merchant
{
    public partial class Payment : System.Web.UI.Page
    {
        protected PaymentViewModel shopperDetails;
        protected bool IsConfigurationValid;
        protected string merchantId;
        protected string privateKey;
        protected string publicKey;

        public Payment()
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
                pnl_fail.Visible = false;
                pnl_sucess.Visible = false;

                if (!IsConfigurationValid)
                    throw new Exception("Application Error: Invalid configuration");
               
            }
            catch (Exception ex)
            {
                pnl_fail.Visible = true;
                pnl_sucess.Visible = false;

                lbl_fail_message.Text = ex.Message;
                Log("Transaction_Fail", ex.Message);
            }
        }

        private decimal GetAmount(string amount)
        {
            //in case a shopper is using currency symbol in the amount textfield this will extract the amount only
            decimal result = 0;
            CultureInfo culture = new CultureInfo("en-US", true);

            //skip the currency symbol to take amount only
            string[] temp = amount.Split(new string[] { culture.NumberFormat.CurrencySymbol }, StringSplitOptions.None);
            if (temp.Length == 2)
                decimal.TryParse(temp[1].Trim(), out result);
            else
                decimal.TryParse(temp[0].Trim(), out result);

            return result;
        }

        private bool IsModelValid()
        {
            bool result = true;

            if (IsModelNull() && IsFormatValid())
                return true;

            return result;
        }

        private bool IsFormatValid()
        {
            bool result = true;
            DateTime datevalue = DateTime.Now;
            int month = Convert.ToInt32(datevalue.Month.ToString());
            int year = Convert.ToInt32(datevalue.Year.ToString());

            //Check for decimal, currency symbol, and alphanumeric characters 
            if (!float.TryParse(txt_amount.Text, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out float amount))
                throw new Exception("Invalid amount format");

            //check based on documentation on: https://en.wikipedia.org/wiki/Payment_card_number
            if (txt_card_number.Text.Trim().Length < 14)
                throw new Exception("Invalid card number");

            //check for month and year
            //validation for range, comapre with current date and format
            if (txt_expiry_year.Text.Trim().Length != 4 || !txt_expiry_year.Text.All(char.IsDigit))
                throw new Exception("Invalid year format");
            else
            {
                if (Convert.ToInt32(txt_expiry_year.Text.Trim()) < year)
                    throw new Exception("Invalid expiry year");
            }

            if (txt_expiry_month.Text.Trim().Length != 2 || !txt_expiry_month.Text.All(char.IsDigit))
                throw new Exception("Invalid month format");
            else
            {
                if (Convert.ToInt32(txt_expiry_month.Text.Trim()) < month && Convert.ToInt32(txt_expiry_year.Text.Trim()) <= year)
                    throw new Exception("Invalid expiry month");
            }

            if (txt_cvc.Text.Trim().Length != 3 || !txt_cvc.Text.All(char.IsDigit))
                throw new Exception("Invalid CVC code");

            return result;
        }

        private bool IsModelNull()
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(txt_amount.Text))
                throw new Exception("Amount cannot be empty");
            if (string.IsNullOrWhiteSpace(txt_card_number.Text))
                throw new Exception("Card cannot be empty");
            if (string.IsNullOrWhiteSpace(txt_cvc.Text))
                throw new Exception("cvc cannot be empty");
            if (string.IsNullOrWhiteSpace(txt_expiry_month.Text))
                throw new Exception("Expiry date cannot be empty");
            if (string.IsNullOrWhiteSpace(txt_expiry_year.Text))
                throw new Exception("year cannot be empty");

            return result;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    if (IsModelValid())
                    {
                        shopperDetails = new PaymentViewModel()
                        {
                            Amount = GetAmount(txt_amount.Text.Trim()),
                            CardNumber = long.Parse(txt_card_number.Text.Trim()),
                            ExpiryMonth = Convert.ToInt32(txt_expiry_month.Text.Trim()),
                            ExpiryYear = Convert.ToInt32(txt_expiry_year.Text.Trim()),
                            CVC = Convert.ToInt32(txt_cvc.Text.Trim())
                        };

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
                        var result = transaction.CreateSale(shopperDetails);

                        if (!result.Details.IsSuccess)
                            throw new Exception(result.Details.Message);

                        pnl_fail.Visible = false;
                        pnl_sucess.Visible = true;
                        lbl_success_message.Text = "Transaction successfull";
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_fail.Visible = true;
                pnl_sucess.Visible = false;

                lbl_fail_message.Text = ex.Message;
                Log("Transaction_Fail", ex.Message);
            }
        }

        protected void btn_transaction_Click(object sender, EventArgs e)
        {
            Response.Redirect("Transactions");
        }

        public void Log(string type, string details)
        {
            using (MerchantEntities context = new MerchantEntities())
            {
                ApplicationLog data = new ApplicationLog()
                {
                    LogType = type,
                    Details = details,
                    DateCreated = DateTime.Now
                };

                context.ApplicationLogs.Add(data);
                context.SaveChanges();
            }
        }
    }
}