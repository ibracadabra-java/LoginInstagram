using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using System.Net;

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
        private string addressProxy;
        private string usernameProxy;
        private string passProxy;
        private string pk;
        private bool freeTrial;
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
        /// <summary>
        /// Proxy que usará el cliente
        /// </summary>
        public string AddressProxy { get => addressProxy; set => addressProxy = value; }
        /// <summary>
        /// Usuario del Proxy
        /// </summary>
        public string UsernameProxy { get => usernameProxy; set => usernameProxy = value; }
        /// <summary>
        /// Password del proxy
        /// </summary>
        public string PassProxy { get => passProxy; set => passProxy = value; }
       
        /// <summary>
        /// Identificador unico
        /// </summary>
        public string PK { get => pk; set => pk = value; }
        /// <summary>
        /// Face de Prueba activada
        /// </summary>
        public bool FreeTrial { get => freeTrial; set => freeTrial = value; }
    }
}