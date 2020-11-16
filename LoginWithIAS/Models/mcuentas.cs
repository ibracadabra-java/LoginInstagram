using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Para el trabajo con las cuentas de usuarios poco activas, falsas o eliminadas
    /// </summary>
    public class mcuentas : mLogin
    {
        /// <summary>
        /// Otro Usuario
        /// </summary>
        public string Other_User { get; set; }

        /// <summary>
        /// Identificador de la Propiedad Other_User
        /// </summary>
        public long Pk_Other_User { get; set; }

        

    }
}