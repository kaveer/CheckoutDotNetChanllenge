using Merchant.Model;
using System;
using System.Globalization;
using System.Linq;

namespace Merchant
{
    public partial class Payment : System.Web.UI.Page
    {
        private PaymentViewModel payment;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_fail.Visible = false;
            pnl_sucess.Visible = false;

            try
            {
                decimal y = 12m;
                if (IsPostBack)
                {
                    if (IsModelValid())
                    {
                        payment = new PaymentViewModel()
                        {
                            Amount = GetAmount(txt_amount.Text.Trim()),
                            CardNumber = Convert.ToInt32(txt_card_number.Text.Trim()),
                            ExpiryMonth = Convert.ToInt32(txt_expiry_month.Text.Trim()),
                            ExpiryYear = Convert.ToInt32(txt_expiry_year.Text.Trim())
                        };

                        //http request here
                        //emcryption of value
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_fail.Visible = true;
                pnl_sucess.Visible = false;

                lbl_fail_message.Text = ex.Message;
                //implement log here
            }
        }

        private decimal GetAmount(string amount)
        {
            //in case a shopper is using currency symbol in the amount textfield this will extract the amount only
            decimal result = 0;
            CultureInfo culture = new CultureInfo("en-US", true);

            //skip the currency symbol to take amount only
            string[] temp = amount.Split(new string[] {culture.NumberFormat.CurrencySymbol }, StringSplitOptions.None);
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
            if (txt_expiry_month.Text.Trim().Length != 2 || !txt_expiry_month.Text.All(char.IsDigit))
                throw new Exception("Invalid month format");
            else 
            {
                if (Convert.ToInt32(txt_expiry_month.Text.Trim()) < month)
                    throw new Exception("Invalid expiry month");
            }

            if (txt_expiry_year.Text.Trim().Length != 4 || !txt_expiry_year.Text.All(char.IsDigit))
                throw new Exception("Invalid year format");
            else
            {
                if (Convert.ToInt32(txt_expiry_month.Text.Trim()) < month)
                    throw new Exception("Invalid expiry year");
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
    }
}