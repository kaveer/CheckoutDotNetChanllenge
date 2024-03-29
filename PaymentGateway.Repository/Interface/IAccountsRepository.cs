﻿using PaymentGateway.Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Interface
{
    public interface IAccountsRepository
    {
        string GenerateToken(string merchantId);
        bool IsTokenValid(string token);

        Token ExtractTokenData(string token);
    }
}
