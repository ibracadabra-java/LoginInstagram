using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LoginWithIAS.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLogged : IPrincipal
    {
        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IIdentity Identity { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>

        public bool IsInRole(string role) { return false; }

        /// <summary>
        /// 
        /// </summary>

        public class IUserLogged : IPrincipal
        {
            /// <summary>
            /// 
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public IIdentity Identity { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="role"></param>
            /// <returns></returns>
            public bool IsInRole(string role) { return false; }
        }
    }
}