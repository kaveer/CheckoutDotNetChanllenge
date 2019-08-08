using PaymentGateway.Repository.Helper;
using PaymentGateway.Repository.Interface;
using PaymentGateway.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

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

        [HttpPost]
        [Route("sales")]
        [ResponseType(typeof(PaymentResponseViewModel))]
        public IHttpActionResult Sales(OuterMapPaymentViewModel item)
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
                _repository.MerchantId = _accountRepository.ExtractTokenData(token).MerchantId;

                if (string.IsNullOrWhiteSpace(item?.Amount) || string.IsNullOrWhiteSpace(item?.CardNumber))
                {
                    CommonAction.Log(Constants.ApplicationLogType.Transaction_Sale_Card_invalid.ToString(), "Invalid Card details");
                    return BadRequest("Invalid card details");
                }

                PaymentResponseViewModel result = _repository.PerformSale(item);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("retrieve")]
        [ResponseType(typeof(TransactionViewModel))]
        public IHttpActionResult GetByMerchantId(string merchantId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
