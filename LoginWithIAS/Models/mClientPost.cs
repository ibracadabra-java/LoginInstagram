using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Post del cliente
    /// </summary>
    public class mClientPost
    {
        /// <summary>
        /// Id
        /// </summary>
        public string postid { get; set; }
        /// <summary>
        /// Likers
        /// </summary>
        public List<string> likers { get; set; }
        /// <summary>
        /// Cantidad de likes
        /// </summary>
        public int cantlike { get; set; }
    }
}
