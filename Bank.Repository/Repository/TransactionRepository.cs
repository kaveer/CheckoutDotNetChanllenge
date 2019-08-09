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

            if (item?.Amount == 0 || item?.CardNumber == 0 || item?.ExpiryMonth == 0 || item?.ExpiryYear ==0)
                return false;

            if (item?.ExpiryMonth < DateTime.Now.Month)
                return false;

            if (item?.ExpiryYear < DateTime.Now.Year)
                return false;

            return result;
        }

        public PaymentResponseViewModel SalesTransaction(PaymentResponseViewModel item)
        {
            throw new NotImplementedException();
        }
    }
}
