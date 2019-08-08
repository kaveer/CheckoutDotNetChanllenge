using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaymentGateway.Controllers
{
    [RoutePrefix("api/auth")]
    public class AccountsController : ApiController
    {
        [Route("token")]
        [HttpGet]
        public IHttpActionResult GenerateClientToken(string merchantId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(merchantId))
                    return BadRequest("Invalid merchant Id");

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
