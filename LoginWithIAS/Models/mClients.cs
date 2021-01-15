using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Cliente
    /// </summary>
    public class mClients
    {
        /// <summary>
        /// PK
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Info
        /// </summary>
        public mInfoClient Info { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public List<mClientAudience> ClientAudience { get; set; }
        /// <summary>
        /// Post
        /// </summary>
        public List<mClientPost> posts { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        public mTags Tags { get; set; }
    }
}
   
