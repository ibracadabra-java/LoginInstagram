using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramApiSharp.Classes.Android.DeviceInfo;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class mLogin : AndroidDevice
    {
        private string user;
        private string pass;
        private string pass_thow_factor;
        /// <summary>
        /// Usuario que desea loguearse
        /// </summary>
        public string User { get => user; set => user = value; }
        /// <summary>
        /// Password del usuario que desea loguearse
        /// </summary>
        public string Pass { get => pass; set => pass = value; }

        /// <summary>
        /// Propiedad para autenticacion en dos pasos
        /// </summary>
        public string Pass_thow_factor { get => pass_thow_factor; set => pass_thow_factor = value; }

        
    }
}