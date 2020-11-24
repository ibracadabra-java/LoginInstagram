using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mPurificador:mLogin
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> UserList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_tarea { get; set; }
    }
}