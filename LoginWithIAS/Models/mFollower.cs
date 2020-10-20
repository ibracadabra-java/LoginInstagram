using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mFollower
    {
        /// <summary>
        /// Usuario que desea eliminar seguidor
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// Password del usuario que desea eliminar seguidor
        /// </summary>
        public string pass { get; set; }
        /// <summary>
        /// Ususario que se desea eliminar 
        /// </summary>
        public string userdel { get; set; }
    }
}