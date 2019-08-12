using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.Model
{
    public class TransactionViewModel
    {
    }

    public class TransactionOuterViewModel
    {
        public string Identifiyer { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public Nullable<decimal> Credited { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public int CVC { get; set; }
        public string Card_Number { get; set; }
        public int Expiry_Month { get; set; }
        public int Expiry_Year { get; set; }
    }
}