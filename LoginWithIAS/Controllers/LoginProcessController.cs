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
        public async Task<enResponseToken> LoginUser (mLogin credencial) 
        {
            enResponseToken token = new enResponseToken();
            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };
            var _apiinst = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger (LogLevel.All)).Build();
            session.LoadSession(_apiinst);
            if (!_apiinst.IsUserAuthenticated)
            {
                var logInResult = await _apiinst.LoginAsync();
                if (logInResult.Succeeded)
                {
                    token.AuthToken = session.GenerarToken();
                    token.Message = logInResult.Info.Message;
                    session.SaveSession(_apiinst);
                    return token;
                    
                }
                else
                {
                    token.Message = logInResult.Info.Message;
                    return token;
                }
            }
            
             else
            {
                token.Message = "Ya se encuentra conectado";
                return token;
            }            
            
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
