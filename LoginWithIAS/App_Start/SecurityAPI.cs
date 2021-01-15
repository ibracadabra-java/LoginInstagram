using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.App_Start
{
    /// <summary>
    /// clase que implementa seguridad en la api
    /// </summary>
    public class SecurityAPI
    {
        private static readonly byte[] secretKey = new byte[] { 164, 0, 194, 0, 161, 189, 50, 38, 130, 89, 141, 164, 45, 170, 188, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 157, 231, 117, 37, 138, 225, 234 };

        /// <summary>
        /// Metodo para encryptar el token
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string Encrypt(Dictionary<string, object> payload)
        {
            string token = Jose.JWT.Encode(payload, secretKey, JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512);

            return token;
        }

        /// <summary>
        /// Metodo para desencryptar
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Dictionary<string, object> Decrypt(string token)
        {
            return Jose.JWT.Decode<Dictionary<string, object>>(token, secretKey);
        }
    }
}