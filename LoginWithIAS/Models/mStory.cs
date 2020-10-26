using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase Story
    /// </summary>
    public class mStory:mLogin
    {
        /// <summary>
        /// Username del usuario del que queremos obtener sus historias
        /// </summary>
        public string Other_user { get; set; }


        /// <summary>
        /// Texto a compartir
        /// </summary>
       public string Text_share { get; set; }


        /// <summary>
        /// Cantidad de historias a compartir
        /// </summary>
        public int cantidad { get; set; }
      
    }
}