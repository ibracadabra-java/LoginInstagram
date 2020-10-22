using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mFollower:mLogin
    {
        /// <summary>
        /// Ususario que se desea eliminar 
        /// </summary>
        public string otheruser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long pk_otheruser { get; set; }
    }
}