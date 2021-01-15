using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mInfoClient
    {
        /// <summary>
        /// 
        /// </summary>
        public string clientid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientusername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long clientcantpost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long clientcantfollowers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long clientcantfollowing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public mClientDescription clientdescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientemail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientphone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientcity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientcountry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public mClientAccounType ClientAccounType { get; set; }

    }
}
