using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LoginWithIAS.App_Start
{
    public class UserLogged : IPrincipal
    {
        public string User { get; set; } 
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }
    }
}