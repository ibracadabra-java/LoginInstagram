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
         /*   var device = new AndroidDevice
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

            };*/
            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };
            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            //InstaApi.SetDevice(device);
            session.LoadSession(InstaApi);
            
            if (!InstaApi.IsUserAuthenticated)
            {
               // await InstaApi.SendRequestsBeforeLoginAsync();

                var logInResult = await InstaApi.LoginAsync();
                if (logInResult.Succeeded)
                {
                    token.AuthToken = session.GenerarToken();
                    token.Message = logInResult.Info.Message;
                   // session.SaveSession(InstaApi);
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
                                            // Save session
                                            session.LoadSession(InstaApi);
                                            token.Message = logInResult.Info.Message;
                                            return token;
                                        }
                                        else
                                        {
                                            // two factor is required
                                            if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                                            {
                                                var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(credencial.Pass_thow_factor);
                                                if (twoFactorLogin.Succeeded)
                                                {
                                                    // connected
                                                    // save session
                                                    session.SaveSession(InstaApi);
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
                                                token.Message = logInResult.Info.Message;
                                                return token;
                                            }
                                        }
                                    }
                                }

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
                                                token.Message = logInResult.Info.Message;
                                                return token;
                                            }
                                            else
                                            {
                                                if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                                                {
                                                    var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(credencial.Pass_thow_factor);
                                                    if (twoFactorLogin.Succeeded)
                                                    {
                                                        // connected
                                                        // save session
                                                        session.SaveSession(InstaApi);
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
                                                    token.Message = logInResult.Info.Message;
                                                    return token;
                                                }
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
                                                token.Message = logInResult.Info.Message;
                                                return token;
                                            }
                                            else
                                            {
                                                if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                                                {
                                                    var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(credencial.Pass_thow_factor);
                                                    if (twoFactorLogin.Succeeded)
                                                    {
                                                        // connected
                                                        // save session
                                                        session.SaveSession(InstaApi);
                                                        token.Message = logInResult.Info.Message;
                                                        return token;
                                                    }
                                                    else
                                                    {
                                                        token.Message = logInResult.Info.Message;
                                                        return token;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            token.Message = logInResult.Info.Message;
                                            return token;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            token.Message = logInResult.Info.Message;
                            return token;
                        }
                    }
                    else if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        token.Message = logInResult.Info.Message;
                        return token;
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

        /// <summary>
        /// Método para desloguearse de la plataforma
        /// </summary>
        /// <param name="credencial">Clase del modelo que captura las credenciales del usuario autenticado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LogoutUser(mLogin credencial)
        {
            string msgSalida = string.Empty;

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

            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };

            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            InstaApi.SetDevice(device);
            session.LoadSession(InstaApi);

            session.LoadSession(InstaApi);

            var logoutResult = await InstaApi.LogoutAsync();
            if (logoutResult.Succeeded)
                session.SaveSession(InstaApi);
            msgSalida = logoutResult.Info.Message;

            return msgSalida;

        }

    }
}
