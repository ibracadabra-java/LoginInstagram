using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using InstagramApiSharp;
using LoginWithIAS.Models;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using System.IO;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Helpers;
using System.Text;
using System.Runtime.InteropServices;
using LoginWithIAS.ApiBd;
using LoginWithIAS.Utiles;
using System.Web;
using System.Configuration;
using LoginWithIAS.App_Start;
<<<<<<< HEAD
using System.Text.RegularExpressions;
=======
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginProcessController : ApiController
    {
        MloginBD bd;
<<<<<<< HEAD
        ProxyBD prbd;
        Sesion session;
        Util util;
        Log log;
        ProxyBD bdprox;
        string path = HttpContext.Current.Request.MapPath("~/Logs");

=======
        Sesion session;
        Util util;
        Log log;
        string path = HttpContext.Current.Request.MapPath("~/Logs");
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b


        /// <summary>
        /// 
        /// </summary>
        public LoginProcessController()
        {

            bd = new MloginBD();
            bdprox = new ProxyBD();
            session = new Sesion();
<<<<<<< HEAD
            prbd = new ProxyBD();
=======
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
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
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy> ();
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

                proxys = prbd.CargarProxy();
                proxyconnect = util.ChoseProxy(proxys,credencial.Country,1);
                if (proxyconnect.ErrorResult) 
                {
                    //insertar en la pila de errores de tareas de login pendientes pendientes
                    bd.InsertarLogin(credencial);
                    //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                    //para esperar unos minutos.
                    token.result = InstaLoginResult.LimitError;
                    token.Message = "No hay Proxys disponibles";
                    return token;
                }
                else
                {
                    //update disponibilidad de los proxys. 
                    bdprox.Update_Proxy(proxyconnect, 1);
                }
                if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
                {


                    token.Message = "Deben introducir el Proxy completo";
                    return token;


                }
                var proxy = new WebProxy()
                {
                    Address = new Uri(proxyconnect.AddressProxy),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                     userName: proxyconnect.UsernameProxy,
                     password: proxyconnect.PassProxy
                     )


                };
                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };

                var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();



               /* if (!(credencial.AdId == null || string.IsNullOrEmpty(credencial.AndroidBoardName) || string.IsNullOrEmpty(credencial.AndroidBootloader) ||
                    credencial.AndroidBoardName == null || string.IsNullOrEmpty(credencial.DeviceBrand) || credencial.DeviceGuid == null || string.IsNullOrEmpty(credencial.DeviceId) ||
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
                }*/

                if (!InstaApi.IsUserAuthenticated)
                {
                    // await InstaApi.SendRequestsBeforeLoginAsync();

                    var logInResult = await InstaApi.LoginAsync();
                    log.Add(logInResult.Info.Message);
                    if (logInResult.Succeeded)
                    {
<<<<<<< HEAD
                        bdprox.Update_Proxy(proxyconnect, -1);                  
                        token.result = InstaLoginResult.Success;
=======
                        
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
                        token.Message = logInResult.Info.Message;
                        credencial.PK = InstaApi.GetLoggedUser().LoggedInUser.Pk.ToString();
<<<<<<< HEAD
                        token.PkUsuario = credencial.PK;
                        int expirationdays = 0;
                        if (credencial.FreeTrial)
                        {
                            expirationdays = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationShortDays"]);
                        }
                        else
                        {
                            expirationdays = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationLongDays"]);
=======
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
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
                        }
                        var now = DateTime.Now;
                        var payload = new Dictionary<string, object>()
                          {
                          { "IdUsuario", token.PkUsuario },
                          { "CreationDate", now },
                          { "ExpirationDate", now.AddDays(expirationdays) }
                          };
<<<<<<< HEAD
                        var pass = new Dictionary<string, object>()
                        {
                            {"Pass",credencial.Pass
                          }
                        };
                        token.AuthToken = SecurityAPI.Encrypt(payload);
                        credencial.Pass = SecurityAPI.Encrypt(pass);
                        var userSessionEncryp = new UserSessionData
                        {
                            UserName = credencial.User,
                            Password = credencial.Pass
                        };
                        InstaApi.SetUser(userSessionEncryp);
                        session.SaveSession(InstaApi);
                        session.SaveSessionBackup(InstaApi);
                        mResultadoBd result = bd.Insertar_Mlogin(credencial, InstaApi.GetCurrentDevice());
=======

                        token.AuthToken = SecurityAPI.Encrypt(payload);
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
                        return token;

                    }
                    else
                    {
                        if(logInResult.Value == InstaLoginResult.ChallengeRequired)
                    {
                            token.result = InstaLoginResult.ChallengeRequired;
                            var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();                            
                            if (challenge.Succeeded)
                            {
                                if (challenge.Value.SubmitPhoneRequired)
                                {
                                    token.Message = "Please type a valid phone number(with country code).\r\ni.e: +989123456789";                                    
                                    token.requireSubmitPhoneNumber = true;
                                    return token;
                                }
                                else
                                {
                                    token.requireSubmitPhoneNumber = false;
                                    if (challenge.Value.StepData != null)
                                    {
                                        if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                        {
                                                                                        
                                            token.UserPhone = challenge.Value.StepData.PhoneNumber;                                            
                                        }
                                        if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                                        {
                                            
                                            token.UserEmail = challenge.Value.StepData.Email;
                                        }

                                       token.Message = "You need to verify that this is your account. Please choose an method to verify your account";
                                    }
                                }
                            }
                            else
                            {
                                token.Message = challenge.Info.Message;
                                return token;
                            }
                        }
                        token.Message = logInResult.Info.Message;
                        token.result = logInResult.Value;
                        return token;
                    }                    
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
        /// Enviar el telefono del usuario a instagram para ChallengeRequire
        /// </summary>
        /// <param name="credencial"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SubmitPhoneNumber (mChallengeRequire credencial) 
        {
            enResponseToken token = new enResponseToken();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                token.result = InstaLoginResult.LimitError;
                token.Message = "No hay Proxys disponibles";
                return token;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                token.Message = "Deben introducir el Proxy completo";
                return token;


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();
            if (string.IsNullOrEmpty(credencial.UserPhone) ||
                     string.IsNullOrWhiteSpace(credencial.UserPhone))
            {
               token.Message = "Please type a valid phone number(with country code).\r\ni.e: +989123456789";
               token.result = InstaLoginResult.Exception;
               return token;
            }
            if (!credencial.UserPhone.StartsWith("+"))
                credencial.UserPhone = $"+{credencial.UserPhone}";
            var submitPhone = await InstaApi.SubmitPhoneNumberForChallengeRequireAsync(credencial.UserPhone);
            if (submitPhone.Succeeded)
            {
                token.Message = "Phone register successfully";
                token.result = InstaLoginResult.Success;
                return token;
            }
            else 
            {
                token.Message = submitPhone.Info.Message;
                return token;
            }

        }
        
        /// <summary>
        /// pedir el envio del codigo de verificacion
        /// </summary>
        /// <param name="credencial"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SubmitCode(mChallengeRequire credencial) 
        {
            enResponseToken token = new enResponseToken();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                token.result = InstaLoginResult.LimitError;
                token.Message = "No hay Proxys disponibles";
                return token;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                token.Message = "Deben introducir el Proxy completo";
                return token;


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            if (credencial.IsEmail)
            {
                var email = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                if (email.Succeeded)
                {
                    token.Message = $"We sent verify code to this email:\n{email.Value.StepData.ContactPoint}";
                    token.result = InstaLoginResult.Success;
                    return token;
                }
                else 
                {
                    token.Message = email.Info.Message;
                    token.result = InstaLoginResult.Exception;
                    return token;
                }                    
            }
            else
            {
                var phoneNumber = await InstaApi.RequestVerifyCodeToSMSForChallengeRequireAsync(replayChallenge: true);
                if (phoneNumber.Succeeded)
                {
                   token.Message = $"We sent verification code one more time\r\nto this phone number(it's end with this):{phoneNumber.Value.StepData.ContactPoint}";
                    token.result = InstaLoginResult.Success;
                    return token;
                }
                else
                {
                    token.Message = phoneNumber.Info.Message;
                    token.result = InstaLoginResult.Exception;
                    return token;
                }
            }
        }
        /// <summary>
        /// Verificar el codigo del challenge require
        /// </summary>
        /// <param name="credencial"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> VerificationCode(mChallengeRequire credencial) 
        {
            enResponseToken token = new enResponseToken();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                token.result = InstaLoginResult.LimitError;
                token.Message = "No hay Proxys disponibles";
                return token;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                token.Message = "Deben introducir el Proxy completo";
                return token;


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();
            credencial.VerifyCode = credencial.VerifyCode.Trim();
            credencial.VerifyCode = credencial.VerifyCode.Replace(" ", "");
            var regex = new Regex(@"^-*[0-9,\.]+$");
            if (!regex.IsMatch(credencial.VerifyCode))
            {
                token.Message =  "Verification code is numeric!!!";
                token.result = InstaLoginResult.Exception;
                return token;
            }
            if (credencial.VerifyCode.Length != 6)
            {
                token.Message = "Verification code must be 6 digits!!!";
                token.result = InstaLoginResult.Exception;
                return token;
            }
            try
            {
                // Note: calling VerifyCodeForChallengeRequireAsync function, 
                // if user has two factor enabled, will wait 15 seconds and it will try to
                // call LoginAsync.

                var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(credencial.VerifyCode);
                if (verifyLogin.Succeeded)
                {
                    // you are logged in sucessfully. 
                    token.result = InstaLoginResult.Success;
                    token.Message = verifyLogin.Info.Message;
                    credencial.PK = InstaApi.GetLoggedUser().LoggedInUser.Pk.ToString();
                    token.PkUsuario = credencial.PK;
                    int expirationdays = 0;
                    if (credencial.FreeTrial)
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
                    var pass = new Dictionary<string, object>()
                        {
                            {"Pass",credencial.Pass
                          }
                        };
                    token.AuthToken = SecurityAPI.Encrypt(payload);
                    credencial.Pass = SecurityAPI.Encrypt(pass);
                    var userSessionEncryp = new UserSessionData
                    {
                        UserName = credencial.User,
                        Password = credencial.Pass
                    };
                    InstaApi.SetUser(userSessionEncryp);
                    session.SaveSession(InstaApi);
                    session.SaveSessionBackup(InstaApi);
                    mResultadoBd result = bd.Insertar_Mlogin(credencial, InstaApi.GetCurrentDevice());
                    return token;

                }
                else
                {
                    if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        //TwoFactorGroupBox.Visible = true;
                        token.result = InstaLoginResult.TwoFactorRequired;
                        token.Message = verifyLogin.Info.Message;
                        return token;

                    }
                    else 
                    {
                        token.result = InstaLoginResult.Exception;
                        token.Message = verifyLogin.Info.Message;
                        return token;
                    }
                }

            }
          catch (Exception ex) 
            { 
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Autenticacion de dos factores
        /// </summary>
        /// <param name="credencial"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> LoginTwoFactor(mChallengeRequire credencial) 
        {
            enResponseToken token = new enResponseToken();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                token.result = InstaLoginResult.LimitError;
                token.Message = "No hay Proxys disponibles";
                return token;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                token.Message = "Deben introducir el Proxy completo";
                return token;


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();
            if (string.IsNullOrEmpty(credencial.VerifyCode))
            {
               token.Message = "Please type your two factor code.";
                token.result = InstaLoginResult.Exception;
                return token;
            }
            credencial.VerifyCode = credencial.VerifyCode.Trim();
            credencial.VerifyCode = credencial.VerifyCode.Replace(" ", "");
            var regex = new Regex(@"^-*[0-9,\.]+$");
            if (!regex.IsMatch(credencial.VerifyCode))
            {
                token.Message = "Verification code is numeric!!!";
                token.result = InstaLoginResult.Exception;
                return token;
            }
            if (credencial.VerifyCode.Length != 6)
            {
                token.Message = "Verification code must be 6 digits!!!";
                token.result = InstaLoginResult.Exception;
                return token;
            }
            var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(credencial.VerifyCode);
            if (twoFactorLogin.Succeeded)
            {
                token.Message = twoFactorLogin.Info.Message;
                credencial.PK = InstaApi.GetLoggedUser().LoggedInUser.Pk.ToString();
                token.PkUsuario = credencial.PK;
                int expirationdays = 0;
                if (credencial.FreeTrial)
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
                var pass = new Dictionary<string, object>()
                        {
                            {"Pass",credencial.Pass}
                        };
                token.AuthToken = SecurityAPI.Encrypt(payload);
                credencial.Pass = SecurityAPI.Encrypt(pass);
                var userSessionEncryp = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = credencial.Pass
                };
                InstaApi.SetUser(userSessionEncryp);
                session.SaveSession(InstaApi);
                mResultadoBd result = bd.Insertar_Mlogin(credencial, InstaApi.GetCurrentDevice());
                return token;
            }
            else 
            {
                token.Message = twoFactorLogin.Info.Message;
                token.result = InstaLoginResult.Exception;
                return token;
            }
        }
        /// <summary>
        /// relogear a un Usuario
        /// </summary>
        /// <param name="credencial"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> Relogin(mLogin credencial, int count = 0)
        {
            enResponseToken token = new enResponseToken();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                token.result = InstaLoginResult.LimitError;
                token.Message = "No hay Proxys disponibles";
                return token;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                token.Message = "Deben introducir el Proxy completo";
                return token;


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };
            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            var loginresult = await InstaApi.LoginAsync();
            if (loginresult.Succeeded)
            {
                bdprox.Update_Proxy(proxyconnect, -1);
                token.Message = loginresult.Info.Message;
                credencial.PK = InstaApi.GetLoggedUser().LoggedInUser.Pk.ToString();
                token.PkUsuario = credencial.PK;
                int expirationdays = 0;
                if (credencial.FreeTrial)
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
                var pass = new Dictionary<string, object>()
                        {
                            {"Pass",credencial.Pass
                          }
                        };
                token.AuthToken = SecurityAPI.Encrypt(payload);
                credencial.Pass = SecurityAPI.Encrypt(pass);
                var userSessionEncryp = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = credencial.Pass
                };
                InstaApi.SetUser(userSessionEncryp);
                session.SaveSession(InstaApi);
                return token;
            }
            else if (loginresult.Value == InstaLoginResult.ChallengeRequired)
            {
                session.LoadSessionBackup(InstaApi);                
                var pass = SecurityAPI.Decrypt(InstaApi.GetLoggedUser().Password);

                var userSessiontemp = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = pass["Pass"].ToString()
                };
                InstaApi.SetUser(userSessiontemp);
                await InstaApi.SendRequestsBeforeLoginAsync();

                var acceptchallenge = await InstaApi.AcceptChallengeAsync();
                count++;
                if (acceptchallenge.Succeeded)
                {
                    token = await Relogin(credencial,count);
                    return token;
                }
                else if (count < 4)
                {
                    token = await Relogin(credencial,count);
                    return token;
                }                
               
                    token.Message = acceptchallenge.Info.Message;
                    return token;
                
            }             
         
                token.Message = loginresult.Info.Message;
            return token;
            
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
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
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

                proxys = prbd.CargarProxy();
                proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
                if (proxyconnect.ErrorResult)
                {
                    //insertar en la pila de errores de tareas de login pendientes pendientes
                    bd.InsertarLogin(credencial);
                    //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                    //para esperar unos minutos.
                    token.result = InstaLoginResult.LimitError;
                    token.Message = "No hay Proxys disponibles";
                    return token;
                }
                else
                {
                    //update disponibilidad de los proxys. 
                    bdprox.Update_Proxy(proxyconnect, 1);
                }
                if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
                {


                    token.Message = "Deben introducir el Proxy completo";
                    return token;


                }
                var proxy = new WebProxy()
                {
                    Address = new Uri(proxyconnect.AddressProxy),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                     userName: proxyconnect.UsernameProxy,
                     password: proxyconnect.PassProxy
                     )

                };
                var httpClientHandler = new HttpClientHandler()
                    {
                        Proxy = proxy,
                    };


                    InstaApi.UseHttpClientHandler(httpClientHandler);
                


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
<<<<<<< HEAD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<mPerfil>> SearchUserPrueba(mMethodLike mlikemanypost)
        {
            List<mPerfil> listperfiles = new List<mPerfil>();
            string path = HttpContext.Current.Request.MapPath("~/Logs");
            mlikemanypost.User = ConfigurationManager.AppSettings["UserApp"];
            mlikemanypost.Pass = ConfigurationManager.AppSettings["PassApp"];            
            Exception ex = new Exception();
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, mlikemanypost.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(mlikemanypost);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                return null;
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                return null;
            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };


            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

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
                    /* foreach (var item in instauser.Value.Users)
                     {
                         count++;
                         mPerfil perfil = new mPerfil();
                         var user = await insta.UserProcessor.GetFullUserInfoAsync(item.Pk);
                         perfil.Nombre = user.Value.UserDetail.FullName;
                         perfil.UserName = user.Value.UserDetail.UserName;
                         perfil.Followers = user.Value.UserDetail.FollowerCount;
                         var imgpk = user.Value.UserDetail.ProfilePictureId;
                         if (imgpk != null)
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
                 */
                    return listperfiles;
                }
                else
                    throw new Exception(instauser.Info.Message);
            }
            else
                throw new Exception("Debe auntenticarse primero");
        }

        /// <summary>
        /// prueba
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public string prueba (string nombre) 
        {
            return nombre;
        }
=======

>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
        #endregion


    }
}
