using InstagramApiSharp.Classes.Android.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase Dispositivo
    /// </summary>
    public class mdevice:AndroidDevice
    {
        /// <summary>
        /// Usuario que desea loguearse
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Password del usuario que desea loguearse
        /// </summary>
        public string Pass { get; set; }

    }
}