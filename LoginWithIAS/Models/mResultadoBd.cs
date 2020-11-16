using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mResultadoBd
    {
        public int ID_TIPO { get; set; }
        public string ID_ERROR { get; set; }
        public string DES_ERROR { get; set; }
        public string VALOR { get; set; }
    }
}