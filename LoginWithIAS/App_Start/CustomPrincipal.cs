using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LoginWithIAS.App_Start
{
<<<<<<< HEAD
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
=======
    public class UserLogged : IPrincipal
    {
        public string User { get; set; } 
        public IIdentity Identity { get; private set; }
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
        public bool IsInRole(string role) { return false; }
    }
}