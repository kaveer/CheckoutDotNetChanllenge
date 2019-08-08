using PaymentGateway.Repository.Helper;
using PaymentGateway.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace PaymentGateway.Controllers
{
    [RoutePrefix("api/transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ITransactionRepository _repository;
        private readonly IAccountsRepository _accountRepository;

        public TransactionsController(ITransactionRepository repository, IAccountsRepository accountRepository)
        {
            _repository = repository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [Route("sales")]
        public IHttpActionResult Sales()
        {
            try
            {
                string token = Request.Headers?.Authorization?.ToString();
                token = token.Replace("Bearer ", string.Empty);

                if (!_accountRepository.IsTokenValid(token))
                {
                    CommonAction.Log(Constants.ApplicationLogType.Transaction_Sales_Token_Invalid.ToString(), "Invalid or expired token");
                    return BadRequest("Invalid token");
                }





                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
