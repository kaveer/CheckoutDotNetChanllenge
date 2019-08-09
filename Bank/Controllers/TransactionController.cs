using Bank.Repository.Interface;
using Bank.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bank.Controllers
{
    [RoutePrefix("api/bank")]
    public class TransactionController : ApiController
    {
        private readonly ITransactionRepository _repository;

        public TransactionController(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("card")]
        public IHttpActionResult SaveCard(GatewayPaymentViewModel item)
        {
            try
            {
                if (item?.CardNumber == 0 || item?.ExpiryMonth == 0 || item?.ExpiryYear == 0)
                    return BadRequest();

                bool result = _repository.AddCard(item);
                if (!result)
                    return StatusCode(HttpStatusCode.Forbidden);

                return Ok("Card successfully added");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("sales")]
        public IHttpActionResult SalesTransaction(PaymentResponseViewModel item)
        {
            try
            {
                if (item?.Payment?.CardNumber == 0 || item?.Merchant?.CardNumber == 0)
                    return BadRequest();

                PaymentResponseViewModel result = _repository.SalesTransaction(item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
