using PaymentGateway.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PaymentGateway.Controllers
{
    [RoutePrefix("api/auth")]
    public class AccountsController : ApiController
    {
        //using unity for dependency injection
        //Install-Package Unity.AspNet.WebApi -Version 5.11.1
        private readonly IAccountsRepository _repository;

        public AccountsController(IAccountsRepository repository)
        {
            _repository = repository;
        }

        [Route("token")]
        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult GenerateClientToken(string merchantId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(merchantId))
                    return BadRequest("Invalid merchant Id");

                string result = _repository.GenerateToken(merchantId);

                if (string.IsNullOrWhiteSpace(result))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
