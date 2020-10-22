using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mLogin
    {
        private string user;
        private string pass;        
        /// <summary>
        /// Usuario que desea loguearse
        /// </summary>
        public string User { get => user; set => user = value; }
        /// <summary>
        /// Password del usuario que desea loguearse
        /// </summary>
        public string Pass { get => pass; set => pass = value; }


    }
}