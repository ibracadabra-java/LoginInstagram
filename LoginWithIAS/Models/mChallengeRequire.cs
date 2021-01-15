using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// modelo del challengeRequire
    /// </summary>
    public class mChallengeRequire : mLogin
    {
        /// <summary>
        /// Telefono del challenge
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// User del challenge
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Codigo de verificacion del challenge
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// Si el codigo se enviara al email
        /// </summary>
        public bool IsEmail { get; set; }
        /// <summary>
        /// Si el codigo se enviara por sms
        /// </summary>
        public bool IsPhone { get; set; }
    }
}