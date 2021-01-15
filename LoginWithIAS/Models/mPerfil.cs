using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Perfil de un usuario
    /// </summary>
    public class mPerfil
    {
        /// <summary>
        /// nombre de usuario
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// nombre del usuario
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// cantidad de seguidores
        /// </summary>
        public long Followers { get; set; }
        /// <summary>
        /// Foto de perfil
        /// </summary>
        public string ProfilePicture { get; set; }
    }
}