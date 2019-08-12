﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Model
{
    public class TransactionViewModel
    {
    }

    public class TransactionOuterViewModel
    {
        public string Identifiyer { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public decimal Credited { get; set; }
        public DateTime Transaction_Date { get; set; }
        public int CVC { get; set; }
        public string Card_Number { get; set; }
        public int Expiry_Month { get; set; }
        public int Expiry_Yeat { get; set; }
    }
}
