using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mLikeManyPost:mLogin
    {
        /// <summary>
        /// Usuario al que se desea dar like
        /// </summary>
        public string userlike { get; set; }
        /// <summary>
        /// Cantidad de like a dar
        /// </summary>
        public int cantLike { get; set; }
        /// <summary>
        /// Tiempo de espera entre like en segundos
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// Usuario seguido por userlike
        /// </summary>
        public string userfollow { get; set; }
    }
}