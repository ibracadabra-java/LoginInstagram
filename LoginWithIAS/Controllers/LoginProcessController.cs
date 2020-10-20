using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Web.Http;
using System.Threading.Tasks;
using InstagramApiSharp;
using LoginWithIAS.Models;
using System.Security.Cryptography;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using InstagramApiSharp.API.Processors;
using System.IO;
using System.Diagnostics;
using InstagramApiSharp.API;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginProcessController : ApiController
    {
        Session session;
        /// <summary>
        /// 
        /// </summary>
        public LoginProcessController()
        {
            session = new Session();
        }

        /// <summary>
        /// 
        /// </summary>
        public Session Session { get => session; set => session = value; }

        /// <summary>
        /// Metodo para Loguearse en la plataforma
        /// </summary>
        /// <param name="credencial"> Clase del modelo que captura las credenciales del usuario autenticado</param>        
        /// <returns></returns>

        [HttpPost]
        public  enResponseToken LoginUser (mLogin credencial) 
        {
                        
            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };
            var _apiinst = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger (LogLevel.All)).Build();
            session.LoadSession(_apiinst);

            return session.GenerarToken(_apiinst).Result;
            
        }
        /// <summary>
        /// Método para desloguearse de la plataforma
        /// </summary>
        /// <param name="credencial">Clase del modelo que captura las credenciales del usuario autenticado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LogoutUser(mLogin credencial)
        {
            string msgSalida = string.Empty;

            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };

            var _apiinst = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(_apiinst);
           var logoutResult = await _apiinst.LogoutAsync();
            if (logoutResult.Succeeded)
                session.SaveSession(_apiinst);
            msgSalida = logoutResult.Info.Message;

            return msgSalida;

        }   
    }
}
