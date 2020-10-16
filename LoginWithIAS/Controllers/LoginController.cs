using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Web.Http;
using System.Threading.Tasks;
using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using System.Security.Cryptography;

namespace LoginWithIAS.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public async Task<enResponseToken> Login (mLogin credencial) 
        {
            enResponseToken token = new enResponseToken();
            Random obj = new Random();
            string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string sNuevacadena = string.Empty;
            char cletra;
            var userSession = new UserSessionData
            {
                UserName = credencial.user,
                Password = credencial.pass
            };
            var _apiinst = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger (LogLevel.All)).Build();

            if (!_apiinst.IsUserAuthenticated)
            {
                var logInResult = await _apiinst.LoginAsync();
                if (logInResult.Succeeded)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        cletra = sCadena[obj.Next(sCadena.Length)];
                        sNuevacadena += cletra.ToString();
                    }
                    token.AuthToken = sNuevacadena;
                    token.Message = logInResult.Info.Message;
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


    }
}
