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
using System.Text;
using System.Runtime.InteropServices;
using LoginWithIAS.ApiBd;
using LoginWithIAS.Utiles;
using System.Web;
using System.Configuration;
using LoginWithIAS.App_Start;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginProcessController : ApiController
    {        
        MloginBD bd;
        Sesion session;
        Util util;
        Log log;
        string path = HttpContext.Current.Request.MapPath("~/Logs");


        /// <summary>
        /// 
        /// </summary>
        public LoginProcessController()
        {

            bd = new MloginBD();
            session = new Sesion();
            util = new Util();
            log = new Log(path);

        }     

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
               
                if (string.IsNullOrEmpty(credencial.User) || string.IsNullOrEmpty(credencial.Pass))
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }
               
                var userSession = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = credencial.Pass
                };

                if (string.IsNullOrEmpty(credencial.AddressProxy)|| string.IsNullOrEmpty(credencial.UsernameProxy) || string.IsNullOrEmpty(credencial.PassProxy))
                {


                    token.Message = "Deben introducir el Proxy completo";
                    return token;


                }
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

                var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All))./*UseHttpClientHandler(httpClientHandler)*/Build();
                


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
                        
                        token.Message = logInResult.Info.Message;
                        session.SaveSession(InstaApi);
                        credencial.PK = InstaApi.GetLoggedUser().LoggedInUser.Pk.ToString();
                        mResultadoBd result = bd.Insertar_Mlogin(credencial,InstaApi.GetCurrentDevice());
                        token.PkUsuario = credencial.PK;
                        int expirationdays = 0;
                        if(credencial.FreeTrial)
                        {
                          expirationdays = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationShortDays"]);
                        }
                        else 
                        {
                           expirationdays = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationLongDays"]);
                        }
                        var now = DateTime.Now;
                        var payload = new Dictionary<string, object>()
                          {
                          { "IdUsuario", token.PkUsuario },
                          { "CreationDate", now },
                          { "ExpirationDate", now.AddDays(expirationdays) }
                          };

                        token.AuthToken = SecurityAPI.Encrypt(payload);
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
                                                bd.Insertar_Mlogin(credencial,InstaApi.GetCurrentDevice());
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
                                                    mResultadoBd result = bd.Insertar_Mlogin(credencial,InstaApi.GetCurrentDevice());
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
                                                    bd.Insertar_Mlogin(credencial,InstaApi.GetCurrentDevice());
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
                                bd.Insertar_Mlogin(credencial,InstaApi.GetCurrentDevice());
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
            var path = Path.Combine(Helper.AccountPathDirectory, $"{insta.GetLoggedUser().UserName}{Helper.SessionExtension}");
            session.LoadSession(insta);

            var logoutResult = await insta.LogoutAsync();
            if (logoutResult.Succeeded)
            {
                 File.Delete(path);
                bd.Eliminar_Mlogin(credencial.PK);
            }
               
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

        /// <summary>
        /// Login con facebook
        /// </summary>
        /// <param name="credencial"></param>
        /// <param name="html"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> LoginUserFacebook(mLogin credencial,string url, string html)
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

                if (!(string.IsNullOrEmpty(credencial.AddressProxy) || string.IsNullOrEmpty(credencial.UsernameProxy) || string.IsNullOrEmpty(credencial.PassProxy)))
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


                if (!(credencial.AdId == null || string.IsNullOrEmpty(credencial.AndroidBoardName) || string.IsNullOrEmpty(credencial.AndroidBootloader) ||
                    credencial.AndroidBoardName == null || string.IsNullOrEmpty(credencial.DeviceBrand) || credencial.DeviceGuid == null || string.IsNullOrEmpty(credencial.DeviceId) ||
                    string.IsNullOrEmpty(credencial.DeviceModel) || string.IsNullOrEmpty(credencial.DeviceModelBoot) || string.IsNullOrEmpty(credencial.DeviceModelIdentifier)
                    || string.IsNullOrEmpty(credencial.Dpi) || string.IsNullOrEmpty(credencial.Resolution) || string.IsNullOrEmpty(credencial.FirmwareFingerprint) ||
                    string.IsNullOrEmpty(credencial.FirmwareTags) || string.IsNullOrEmpty(credencial.FirmwareType)))
                {
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

                    if (!url.Equals("https://m.facebook.com/") || !url.Equals("https://facebook.com/") || !url.Equals("https://www.facebook.com/"))
                    {
                        token.Message = "Direccion URL incorrecta";
                        return token;
                    }

                    if (string.IsNullOrEmpty(html))
                    {
                        token.Message = "Pagina HTML incorrecta o vacia";
                        return token;
                    }

                    if (InstaFbHelper.IsLoggedIn(html))
                    {

                        var cookies = GetUriCookies(new Uri(url));
                        var fbToken = InstaFbHelper.GetAccessToken(html);

                        var logInResult = await InstaApi.LoginWithFacebookAsync(fbToken,cookies);
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
                            else if (logInResult.Value == InstaLoginResult.BadPassword)
                            {
                                token.Message = logInResult.Info.Message;
                                return token;
                            }
                            else
                            {
                                token.Message = logInResult.Info.Message;
                                return token;
                            }

                        }

                        token.Message = logInResult.Info.Message;
                        return token;
                    }                    
                }
                else
                {
                    token.Message = "Ya se encuentra conectado";
                    return token;
                }
                token.Message = "Null";
                return token;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieName"></param>
        /// <param name="cookieData"></param>
        /// <param name="size"></param>
        /// <param name="dwFlags"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>        
        #region DllImport for getting full cookies from WebBrowser
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);


        private const Int32 InternetCookieHttponly = 0x2000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetUriCookies(Uri uri)
        {
            string cookies = "";
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return cookies;
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return cookies;
            }
            if (cookieData.Length > 0)
            {
                cookies = cookieData.ToString();
            }
            return cookies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<mPerfil>> SearchUser (mMethodLike mlikemanypost) 
        {
            List<mPerfil> listperfiles = new List<mPerfil>();
             string path = HttpContext.Current.Request.MapPath("~/Logs");
             mlikemanypost.User = ConfigurationManager.AppSettings["UserApp"];
             mlikemanypost.Pass = ConfigurationManager.AppSettings["PassApp"];

             int count = 0;
             Exception ex = new Exception();            

             var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

             if (!(string.IsNullOrEmpty(mlikemanypost.User) || string.IsNullOrEmpty(mlikemanypost.Pass)))
             {
                 var userSession = new UserSessionData
                 {
                     UserName = mlikemanypost.User,
                     Password = mlikemanypost.Pass
                 };
                 insta.SetUser(userSession);
             }
             else
             {
                 log.Add("Deben introducir Usuario y Contraseña");
                 throw new Exception("Deben introducir Usuario y Contraseña");

             }

             session.LoadSession(insta);

             if (!insta.GetLoggedUser().Password.Equals(mlikemanypost.Pass))
             {
                 log.Add("Contraseña incorrecta");
                 throw new Exception("Contraseña incorrecta"); 
             }
             if (insta.IsUserAuthenticated)
             {
                 var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                 if (instauser.Succeeded)
                 {
                     foreach (var item in instauser.Value.Users)
                     {
                         count++;
                         mPerfil perfil = new mPerfil();                        
                         var user = await insta.UserProcessor.GetFullUserInfoAsync(item.Pk);
                         perfil.Nombre = user.Value.UserDetail.FullName;
                         perfil.UserName = user.Value.UserDetail.UserName;
                         perfil.Followers = user.Value.UserDetail.FollowerCount;
                         var imgpk = user.Value.UserDetail.ProfilePictureId;
                         if (imgpk!=null)
                         {
                             var imgpr = await insta.MediaProcessor.GetMediaByIdAsync(imgpk);
                             perfil.ProfilePicture = imgpr.Value.Images[0].Uri;
                         }
                         else
                         perfil.ProfilePicture = user.Value.UserDetail.ProfilePicture;
                         listperfiles.Add(perfil);
                         if (count > 5)
                             break;
                     }



                     return listperfiles;
                 }

                 else
                     throw new Exception(instauser.Info.Message);
             }
             else
                 throw new Exception("Debe auntenticarse primero");
        }

        #endregion


    }
}
