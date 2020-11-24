using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase que hace referencia a la tabla MasSending
    /// </summary>
    public class mMasSending
    {
        /// <summary>
        /// PK de los usuarios a los que deseamos enviar un mensaje. Estos estaran separados por coma.
        /// </summary>
        public string Usuarios { get; set; }

        /// <summary>
        /// Mensajes que seran enviados a la lista de usuarios
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Usuario del cliente que se encuentra autenticado
        /// </summary>
        public string Usuario { get; set; }


        /// <summary>
        /// Contrasena del usuario que se encuentra autenticado
        /// </summary>
        public string Pass { get; set; }
    }
}