using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase que hace referencia a la tabla MasSending
    /// </summary>
    public class mMasSending:mLogin
    {
        /// <summary>
        /// PK de los usuarios a los que deseamos enviar un mensaje. Estos estaran separados por coma.
        /// </summary>
        public string Usuarios { get; set; }

        /// <summary>
        /// Mensajes que seran enviados a la lista de usuarios
        /// </summary>
        public string Texto { get; set; }
       
    }
}