using PaymentGateway.Datalayer;
using PaymentGateway.Repository.Interface;
using System;
using System.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using PaymentGateway.Repository.Helper;

namespace PaymentGateway.Repository.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        protected string secretKey;
        protected string expireMinuites;
        protected readonly bool isConfigurationValid;

        public AccountsRepository()
        {
            isConfigurationValid = ReadConfiguration();
        }

        public string GenerateToken(string merchantId)
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(merchantId))
                return string.Empty;

            if (!IsMerchantIdValid(merchantId))
                return string.Empty;

            result = GenerateJWT(merchantId);
            if (string.IsNullOrWhiteSpace(result))
            {
                CommonAction.Log(Constants.ApplicationLogType.Token_Create_Fail.ToString(), string.Format("generate token for merchant Id: {0}", merchantId));
                return string.Empty;
            }

            SaveToken(result, merchantId);
            CommonAction.Log(Constants.ApplicationLogType.Token_Create_Success.ToString(), string.Format("generate token for merchant Id: {0}", merchantId));

            return result;
        }

        public bool IsTokenValid(string token)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            var tokenData = ExtractTokenData(token);

            if (tokenData == null)
                return false;

            //In usual case this should be less than 5 minuite
            if (DateTime.UtcNow > tokenData.DateCreated.AddMinutes(20))
                return false;

            if (!ValidateMerchandId(tokenData))
                return false;

            return result;
        }

        private bool ValidateMerchandId(Token tokenData)
        {
            bool result = true;

            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                var data = context.Tokens?
                                    .Where(x => x.ClientToken.Trim() == tokenData.ClientToken.Trim())
                                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(data?.MerchantId))
                    return false;

                if (data.MerchantId != tokenData.MerchantId)
                    return false;
            }

            return result;
        }

        public Token ExtractTokenData(string token)
        {
            Token result = new Token();


            JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
            var serializeToken = hand.ReadJwtToken(token);

            if (serializeToken == null)
                return null;

            result = new Token()
            {
                ClientToken = token,
                DateCreated = serializeToken.ValidFrom,
                MerchantId = serializeToken.Claims.SingleOrDefault(x => x.Type == "unique_name").Value
            };


            return result;

        }

        private void SaveToken(string token, string merchantId)
        {
            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                Token data = new Token()
                {
                    MerchantId = merchantId,
                    ClientToken = token,
                    DateCreated = DateTime.Now
                };

                context.Tokens.Add(data);
                context.SaveChanges();
            }
        }

        private string GenerateJWT(string merchantId)
        {
            //using : https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/
            //using: https://www.nuget.org/packages/Microsoft.IdentityModel.Tokens/

            string result = string.Empty;
            if (!isConfigurationValid)
                return string.Empty;

            var symmetricKey = Convert.FromBase64String(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, merchantId),
                        new Claim(ClaimTypes.PrimarySid, merchantId),
                        new Claim(ClaimTypes.Expiration, now.AddMinutes(Convert.ToInt32(expireMinuites)).ToString())
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinuites)),

                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(symmetricKey), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            result = tokenHandler.WriteToken(stoken);

            return result;
        }

        private bool IsMerchantIdValid(string merchantId)
        {
            bool result = true;

            using (PaymentGatewayEntities context = new PaymentGatewayEntities())
            {
                var data = context.Merchants?
                                    .Where(x => x.MerchantId.Trim() == merchantId.Trim())
                                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(data?.MerchantId))
                    return false;
            }

            return result;
        }

        private bool ReadConfiguration()
        {
            bool result = true;

            secretKey = ConfigurationManager.AppSettings["JWT:Key"]?.ToString();
            expireMinuites = ConfigurationManager.AppSettings["JWT:Expire:Minuites"]?.ToString();

            if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(expireMinuites))
                return false;

            return result;
        }

    }
}
