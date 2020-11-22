using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// Clase para enviar un mensaje de texto desde un usuario autenticado hacia otro
    /// </summary>
    public class mchat:mLogin
    { 
        /// <summary>
        /// Usuario al que se le enviara el mensaje
        /// </summary>
        public string otheruser { get; set; }

        /// <summary>
        /// Texto a enviar
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Foto a enviar
        /// </summary>
        public InstaImage foto { get; set; }

        /// <summary>
        /// Video a enviar
        /// </summary>
        public InstaVideo video { get; set; }

        /// <summary>
        /// Audio a enviar
        /// </summary>
        public InstaAudioUpload audio { get; set; }

        /// <summary>
        /// Url a enviar
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Emoticon a enviar
        /// </summary>
        public string Giphyid { get; set; }


        /// <summary>
        /// Enviar un Hashtag
        /// </summary>
        public string Hashtag { get; set; }


        /// <summary>
        /// Listado de usuarios a los que enviar mensajes
        /// </summary>
        public List<long> listado_pk { get; set; }

    }
}