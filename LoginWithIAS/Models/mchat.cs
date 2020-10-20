using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase para enviar un mensaje de texto desde un usuario autenticado hacia otro
    /// </summary>
    public class mchat
    { 
        /// <summary>
        /// Usuario autenticado
        /// </summary>
        public string userauthenticated { get; set; }

        /// <summary>
        /// Password del usuario eutenticado
        /// </summary>
        public string passworduserauthenticated { get; set; }

        /// <summary>
        /// Usuario al que se le enviara el mensaje
        /// </summary>
        public string otheruser { get; set; }

        /// <summary>
        /// Texto a enviar
        /// </summary>
        public string text { get; set; }
    }
}