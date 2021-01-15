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
        private string pk;
        private bool freeTrial;
        private string country;
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
        /// Identificador unico
        /// </summary>
        public string PK { get => pk; set => pk = value; }
        /// <summary>
        /// Face de Prueba activada
        /// </summary>
        public bool FreeTrial { get => freeTrial; set => freeTrial = value; }
        /// <summary>
        /// Pais del cliente
        /// </summary>
        public string Country { get => country; set => country = value; }
    }
}