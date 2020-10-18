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
using System.IO;
using System.Diagnostics;
using InstagramApiSharp.API;

namespace LoginWithIAS.Controllers
{
    public class LoginController : ApiController
    {
        const string StateFile = "state.bin";
        [HttpPost]
        public async Task<enResponseToken> LoginUser (mLogin credencial) 
        {
            enResponseToken token = new enResponseToken();
            Random obj = new Random();
            string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string sNuevacadena = string.Empty;
            char cletra;            
            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };
            var _apiinst = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger (LogLevel.All)).Build();
            LoadSession(_apiinst);

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
                    SaveSession(_apiinst);
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
            LoadSession(_apiinst);
           var logoutResult = await _apiinst.LogoutAsync();
            if (logoutResult.Succeeded)
                SaveSession(_apiinst);
            msgSalida = logoutResult.Info.Message;

            return msgSalida;

        }
        public void LoadSession(IInstaApi IstanciaApi)
        {
            try
            {
                if (File.Exists(StateFile))
                {

                    using (var fs = File.OpenRead(StateFile))
                    {
                        IstanciaApi.LoadStateDataFromStream(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public void SaveSession(IInstaApi IstanciaApi)
        {
            if (IstanciaApi == null)
                return;
            var state = IstanciaApi.GetStateDataAsStream();
            using (var fileStream = File.Create(StateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }



    }
}
