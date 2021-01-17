using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramApiSharp.Classes;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class enResponseToken
    {
        /// <summary>
        /// Token generado al auntenticarse
        /// </summary>
        public string AuthToken { get; set; }
        /// <summary>
        /// Mensaje descriptivo al loguarse
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// id unico del usuario en instagram
        /// </summary>
        public string PkUsuario { get; set; }
        /// <summary>
        /// Email del usuario
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Phone del usuario
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// Resultado del login
        /// </summary>
        public InstaLoginResult result { get; set; }
        /// <summary>
        /// Requiere que el usuario envie su numero de telefono a instagram
        /// </summary>
        public bool requireSubmitPhoneNumber { get; set; }
    }
}