using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.Model
{
    public class PaymentViewModel
    {
        public long CardNumber { get; set; }
        //for amount better use decimal accounding to my experience and link: http://net-informations.com/q/faq/float.html
        //for performance use doucle but if dealing with money use decimal link: https://exceptionnotfound.net/decimal-vs-double-and-other-tips-about-number-types-in-net/
        public decimal Amount { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVC { get; set; }
    }

    
}