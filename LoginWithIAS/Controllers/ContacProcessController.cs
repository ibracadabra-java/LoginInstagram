﻿using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador de Contactos
    /// </summary>
    public class ContacProcessController : ApiController
    {
        /// <summary>
        /// Objeto seccion para salvar y calgar los datos
        /// </summary>
        Session session;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContacProcessController()
        {
            this.session = new Session();
        }

        /*/// <summary>
        /// Metodo para extraer el contacto de un usuario
        /// </summary>
        /// <returns></returns>
       public async Task<List<string>> getContactos(mContacts credencial)
        {
            List<string> listado_contactos = new List<string>();

            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

            if (!(string.IsNullOrEmpty(credencial.User) || string.IsNullOrEmpty(credencial.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = credencial.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                return null;
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(credencial.Pass))
            {
                return null;
            }

            if (insta.IsUserAuthenticated)
            {
                
            }
            else
            {
                return null; 
            }

            return listado_contactos;

        }*/
    }
}
