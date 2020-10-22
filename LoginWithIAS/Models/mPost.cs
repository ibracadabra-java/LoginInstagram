using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Media Post
    /// </summary>
    public class mPost:mLogin
    {
        /// <summary>
        /// id del post
        /// </summary>
        public string idpost { get; set; }

        /// <summary>
        /// Tipo de post
        /// </summary>
        public int tipo_media { get; set; }

    }
}