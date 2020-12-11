using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}