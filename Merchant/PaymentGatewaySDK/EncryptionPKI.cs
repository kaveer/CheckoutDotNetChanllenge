using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PaymentGatewaySDK
{
    public class EncryptionPKI
    {
        //use only one to generate the merchante Id, public and private ket to save in site.config
        protected void GetKey()
        {
            string guid = Guid.NewGuid().ToString();
            int sizeRSA = 1024;

            if (!string.IsNullOrWhiteSpace(guid))
            {
                AsymmetricCipherKeyPair keyPairs = GenerateKeys(sizeRSA);
                if (keyPairs == null)
                {
                    throw new Exception("Fail to generate PKI");
                }

                Dictionary<int, string> keys = ExtractKey(keyPairs);
                if (keys.Count > 0)
                {

                    string PublicKey = keys[2];
                    string PrivateKey = keys[1];

                }
            }
        }

        protected AsymmetricCipherKeyPair GenerateKeys(int keySizeInBits)
        {
            var r = new RsaKeyPairGenerator();
            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(randomGenerator), keySizeInBits));
            var keys = r.GenerateKeyPair();
            return keys;
        }

        protected Dictionary<int, string> ExtractKey(AsymmetricCipherKeyPair key)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(key.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();

            result.Add(1, Convert.ToBase64String(serializedPrivateBytes));
            result.Add(2, Convert.ToBase64String(serializedPublicBytes));

            return result;
        }

        protected string Encrypt(string publicKey, string txtToEncrypt)
        {
            byte[] keyBytes = Convert.FromBase64String(publicKey);

            RsaKeyParameters publicKeyInfo = (RsaKeyParameters)PublicKeyFactory.CreateKey(keyBytes);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = publicKeyInfo.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = publicKeyInfo.Exponent.ToByteArrayUnsigned();
            rsa.ImportParameters(rsaParameters);

            byte[] bytes = Encoding.UTF8.GetBytes(txtToEncrypt);
            byte[] enc = rsa.Encrypt(bytes, false);
            string base64Enc = Convert.ToBase64String(enc);

            return base64Enc;
        }

        protected string Decrypt(string pvtKey, string txtToDecrypt)
        {
            RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(pvtKey));
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            RSAParameters rsaParameters2 = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)privateKey);

            rsa.ImportParameters(rsaParameters2);

            byte[] dec = rsa.Decrypt(Convert.FromBase64String(txtToDecrypt), false);
            string decStr = Encoding.UTF8.GetString(dec);

            return decStr;
        }
    }


}