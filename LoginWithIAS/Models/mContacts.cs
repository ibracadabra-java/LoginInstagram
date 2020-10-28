using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramApiSharp.Classes.Models;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase Contactos
    /// </summary>
    public class mContacts:mLogin
    {
        /// <summary>
        /// Usuario del que se obtendra su lista de contactos
        /// </summary>
        public string otheruser { get; set; }
        /// <summary>
        /// Lista de contactos
        /// </summary>
        public InstaContactList ListContact { get; set; }
    }
}