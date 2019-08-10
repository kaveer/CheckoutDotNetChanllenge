using Bank.Datalayer;
using Bank.Repository.Interface;
using Bank.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Repository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        public bool AddCard(GatewayPaymentViewModel item)
        {
            bool result = true;

            if (!IsCardValid(item))
                return false;

            using (BankEntities context = new BankEntities())
            {
                context.CardDetails.Add(new CardDetail()
                {
                    CardNumber = item.CardNumber.ToString(),
                    ExpiryMonth = item.ExpiryMonth,
                    ExpiryYear = item.ExpiryYear,
                    TotalAmount = item.Amount
                });

                context.SaveChanges();
            }

            return result;
        }

        private bool IsCardValid(GatewayPaymentViewModel item)
        {
            bool result = true;

            if (item?.Amount == 0 || item?.CardNumber == 0 || item?.ExpiryMonth == 0 || item?.ExpiryYear == 0)
                return false;

            if (item?.ExpiryMonth < DateTime.Now.Month)
                return false;

            if (item?.ExpiryYear < DateTime.Now.Year)
                return false;

            return result;
        }

        public PaymentResponseViewModel SalesTransaction(PaymentResponseViewModel item)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel();

            if (IsModelValid(item))
            {
                using (BankEntities context = new BankEntities())
                {

                    var debitAccount = context.CardDetails
                                    .Where(x=> long.Parse(x.CardNumber) == item.Payment.CardNumber)
                                    .FirstOrDefault();

                    if (debitAccount.TotalAmount < item.Payment.Amount)
                        return result = InValidTransaction("Insufficent amount. Transaction aborted");

                    debitAccount.TotalAmount = (debitAccount.TotalAmount - item.Payment.Amount);
                    context.SaveChanges();

                    var merchant = context.CardDetails
                                        .Where(x => long.Parse(x.CardNumber) == item.Merchant.CardNumber)
                                        .FirstOrDefault();

                    merchant.TotalAmount = merchant.TotalAmount + item.Payment.Amount;

                    return result = new PaymentResponseViewModel()
                    {
                        TransactionId = Guid.NewGuid().ToString(),
                        Details = new GatewayTransactionDetailsViewModel()
                        {
                            IsSuccess = true,
                            Code = 2000,
                            Details = string.Format("Transaction successfull between debit: {0} and and credit {1} account. {2} was successfully credited", item.Payment.CardNumber, item.Merchant.CardNumber, item.Payment.Amount),
                            Message = "Transaction successfull",
                            TransactionDate = DateTime.Now
                        },
                        Payment = item.Payment,
                        Merchant = item.Merchant
                    };
                }
            }
            else
            {
                return result = InValidTransaction("Invalid model for transaction");
            }

        }

        private PaymentResponseViewModel InValidTransaction(string message)
        {
            PaymentResponseViewModel result = new PaymentResponseViewModel()
            {
                TransactionId = "0",
                Merchant = new GatewayMerchantViewModel(),
                Payment = new GatewayPaymentViewModel(),
                Details = new GatewayTransactionDetailsViewModel()
                {
                    IsSuccess = false,
                    Message = message,
                    Details = message,
                    Code = 2039,
                    TransactionDate = DateTime.Now
                }
            };

            throw new NotImplementedException();
        }

        private bool IsModelValid(PaymentResponseViewModel item)
        {
            bool result = true;

            if (!IsCardValid(item?.Payment))
                return false;

            if (!IsMerchantValid(item?.Merchant))
                return false;

            if (!IsCardExist(item.Payment.CardNumber))
                return false;

            return result;
        }

        private bool IsMerchantValid(GatewayMerchantViewModel merchant)
        {
            bool result = true;

            if (merchant.CardNumber == 0 || merchant.CardNumber == 0)
                return false;

            if (merchant.ExpiryMonth < DateTime.Now.Month)
                return false;

            if (merchant.ExpiryYear < DateTime.Now.Year)
                return false;

            if (!IsCardExist(merchant.CardNumber))
                return false;

            return result;
        }

        private bool IsCardExist(long cardNumber)
        {
            bool result = true;

            using (BankEntities context = new BankEntities())
            {
                context.CardDetails
                           .Where(x => long.Parse(x.CardNumber) == cardNumber)
                           .FirstOrDefault();

            }

            return result;
        }
    }
}
