using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mLikeManyPost { 
        /// <summary>
        /// Usuario quede desea dar like
        /// </summary>
    public string user { get; set; }
        /// <summary>
        /// Password del usuario que desea dar like
        /// </summary>
    public string pass { get; set; }
        /// <summary>
        /// Usuario al que se desea dar like
        /// </summary>
    public string userlike { get; set;}
        /// <summary>
        /// Cantidad de like a dar
        /// </summary>
    public int cantLike { get; set; }
 }
}