using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaymentGateway.Controllers
{
    [RoutePrefix("api/transactions")]
    public class TransactionsController : ApiController
    {
        [HttpGet]
        [Route("sales")]
        public IHttpActionResult Sales()
        {
            return Ok("sucecss");

        }
    }
}
