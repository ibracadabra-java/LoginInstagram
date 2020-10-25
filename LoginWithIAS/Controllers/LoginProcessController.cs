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
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Helpers;
using InstagramApiSharp.Classes.SessionHandlers;


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
        public async Task<enResponseToken> LoginUser(mLogin credencial)
        {
            enResponseToken token = new enResponseToken();
            try
            {                
                var InstaApi = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();
                if (!(string.IsNullOrEmpty(credencial.User) || string.IsNullOrEmpty(credencial.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = credencial.User,
                        Password = credencial.Pass
                    };
                    InstaApi.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                if (!(string.IsNullOrEmpty(credencial.AddressProxy)|| string.IsNullOrEmpty(credencial.UsernameProxy) || string.IsNullOrEmpty(credencial.PassProxy)))
                {
                    var proxy = new WebProxy()
                    {
                        Address = new Uri(credencial.AddressProxy),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(
                       userName: credencial.UsernameProxy,
                       password: credencial.PassProxy
                       )

                    };
                    var httpClientHandler = new HttpClientHandler()
                    {
                        Proxy = proxy,
                    };

                    
                    InstaApi.UseHttpClientHandler(httpClientHandler);
                }


                if (!(credencial.AdId ==null || string.IsNullOrEmpty(credencial.AndroidBoardName) || string.IsNullOrEmpty(credencial.AndroidBootloader)||
                    credencial.AndroidBoardName == null || string.IsNullOrEmpty(credencial.DeviceBrand) || credencial.DeviceGuid == null ||string.IsNullOrEmpty(credencial.DeviceId) ||
                    string.IsNullOrEmpty(credencial.DeviceModel) || string.IsNullOrEmpty(credencial.DeviceModelBoot) || string.IsNullOrEmpty(credencial.DeviceModelIdentifier)
                    || string.IsNullOrEmpty(credencial.Dpi) || string.IsNullOrEmpty(credencial.Resolution) || string.IsNullOrEmpty(credencial.FirmwareFingerprint) ||
                    string.IsNullOrEmpty(credencial.FirmwareTags) || string.IsNullOrEmpty(credencial.FirmwareType))) {
                    var device = new AndroidDevice
                    {

                        AdId = credencial.AdId,
                        AndroidBoardName = credencial.AndroidBoardName,
                        AndroidBootloader = credencial.AndroidBootloader,
                        AndroidVer = credencial.AndroidVer,
                        DeviceBrand = credencial.DeviceBrand,
                        DeviceGuid = new Guid(credencial.DeviceGuid.ToString()),
                        DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(credencial.DeviceId.ToString())),
                        DeviceModel = credencial.DeviceModel,
                        DeviceModelBoot = credencial.DeviceModelBoot,
                        DeviceModelIdentifier = credencial.DeviceModelIdentifier,
                        Dpi = credencial.Dpi,
                        Resolution = credencial.Resolution,
                        FirmwareFingerprint = credencial.FirmwareFingerprint,
                        FirmwareTags = credencial.FirmwareTags,
                        FirmwareType = credencial.FirmwareType

                    };

                    InstaApi.SetDevice(device);
                }

                session.LoadSession(InstaApi);
                if (!InstaApi.GetLoggedUser().Password.Equals(credencial.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }
                if (!InstaApi.IsUserAuthenticated)
                {
                    // await InstaApi.SendRequestsBeforeLoginAsync();

                    var logInResult = await InstaApi.LoginAsync();
                    if (logInResult.Succeeded)
                    {
                        token.AuthToken = session.GenerarToken();
                        token.Message = logInResult.Info.Message;
                        session.SaveSession(InstaApi);
                        return token;

                    }
                    else
                    {
                        if (logInResult.Value == InstaLoginResult.ChallengeRequired)
                        {
                            var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();
                            if (challenge.Succeeded)
                            {
                                if (challenge.Value.SubmitPhoneRequired)
                                {
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                    {
                                        var submitPhone = await InstaApi.SubmitPhoneNumberForChallengeRequireAsync(challenge.Value.StepData.PhoneNumber);
                                        if (submitPhone.Succeeded)
                                        {
                                            var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(submitPhone.Value.StepData.SecurityCode);
                                            if (verifyLogin.Succeeded)
                                            {
                                                // Save sessionç                                            
                                                token.Message = logInResult.Info.Message;
                                                token.AuthToken = session.GenerarToken();
                                                session.SaveSession(InstaApi);
                                                return token;
                                            }
                                            else
                                            {

                                                token.Message = verifyLogin.Info.Message;
                                                return token;

                                            }
                                        }
                                        else
                                        {
                                            token.Message = submitPhone.Info.Message;
                                            return token;
                                        }
                                    }
                                    token.Message = "No se encontró número de teléfono asignado a la cuenta";
                                    return token;


                                }
                                else
                                {
                                    if (challenge.Value.StepData != null)
                                    {
                                        if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                        {
                                            var submitPhone = await InstaApi.RequestVerifyCodeToSMSForChallengeRequireAsync();
                                            if (submitPhone.Succeeded)
                                            {
                                                var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(submitPhone.Value.StepData.SecurityCode);
                                                if (verifyLogin.Succeeded)
                                                {
                                                    // Save session
                                                    session.SaveSession(InstaApi);
                                                    token.Message = verifyLogin.Info.Message;
                                                    return token;
                                                }
                                                else
                                                {
                                                    token.Message = verifyLogin.Info.Message;
                                                    return token;
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                                        {
                                            var email = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                                            if (email.Succeeded)
                                            {
                                                var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(email.Value.StepData.SecurityCode);
                                                if (verifyLogin.Succeeded)
                                                {
                                                    // Save session
                                                    session.SaveSession(InstaApi);
                                                    token.Message = verifyLogin.Info.Message;
                                                    token.AuthToken = session.GenerarToken();
                                                    return token;
                                                }
                                                else
                                                {
                                                    token.Message = verifyLogin.Info.Message;
                                                    return token;
                                                }
                                            }
                                            else
                                            {
                                                token.Message = email.Info.Message;
                                                return token;
                                            }

                                        }
                                        else
                                        {
                                            token.Message = "No se encontró ni número de teléfono ni Email asignado a esta cuenta";
                                            return token;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                token.Message = challenge.Info.Message;
                                return token;
                            }
                        }
                        else if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                        {
                            var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(credencial.Pass_thow_factor);
                            if (twoFactorLogin.Succeeded)
                            {
                                // connected
                                // save session
                                session.SaveSession(InstaApi);
                                token.Message = twoFactorLogin.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = twoFactorLogin.Info.Message;
                                return token;
                            }

                        }

                    }

                    token.Message = logInResult.Info.Message;
                    return token;
                }
                else
                {
                    token.Message = "Ya se encuentra conectado";
                    return token;
                }
            }
            catch (Exception)
            {

                throw;
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
                return "Deben introducir Usuario y Contraseña";                
            }

            session.LoadSession(insta);

            var logoutResult = await insta.LogoutAsync();
            if (logoutResult.Succeeded)
                session.SaveSession(insta);
            msgSalida = logoutResult.Info.Message;

            return msgSalida;

        }

        /// <summary>
        /// Obtener sesiones de login activas
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<InstaLoginSession>> GetLoginSessionActive(mLogin login)
        {
            try
            {
                List<InstaLoginSession> lista = new List<InstaLoginSession>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(login.User) || string.IsNullOrEmpty(login.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = login.User,
                        Password = login.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(login.Pass))
                {
                    return null;
                }

                var user = await insta.AccountProcessor.GetLoginSessionsAsync();

                if (user.Succeeded)
                {
                    for (int i = 0; i < user.Value.Sessions.Count; i++)
                    {
                        lista.Add(user.Value.Sessions[i]);
                    }
                }
                else
                {
                    return null;
                }

                return lista;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }


    }
}
