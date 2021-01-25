using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using Microsoft.Ajax.Utilities;
using System;
using LoginWithIAS.Utiles;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LoginWithIAS.ApiBd;
using LoginWithIAS.App_Start;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageProcessController : ApiController
    {
        Sesion session;
        Util util;
        ProxyBD bdprox;
        ProxyBD prbd;
        MloginBD bd;

        /// <summary>
        /// 
        /// </summary>
        public MessageProcessController()
        {
            session = new Sesion();
            util = new Util();
            bd = new MloginBD();
            bdprox = new ProxyBD();
            prbd = new ProxyBD();
        }

        /// <summary>
        /// Metodo para enviar un mensaje de texto a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult<string>> SendDirectMessage(mchat chat)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                mProxy proxyconnect = new mProxy();
                List<mProxy> proxys = new List<mProxy>();

                proxys = prbd.CargarProxy();
                proxyconnect = util.ChoseProxy(proxys, chat.Country, 1);
                if (proxyconnect.ErrorResult)
                {
                    //insertar en la pila de errores de tareas de login pendientes pendientes
                    bd.InsertarLogin(chat);
                    //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                    //para esperar unos minutos.
                    return Result.Fail<string>("No hay Proxys disponibles");
                }
                else
                {
                    //update disponibilidad de los proxys. 
                    bdprox.Update_Proxy(proxyconnect, 1);
                }
                if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
                {
                    return Result.Fail<string>("Deben introducir el Proxy completo");
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

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return Result.Fail<string>("Deben introducir Usuario y Contraseña");
                }

                session.LoadSession(insta);

                var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

                if (!pass["Pass"].Equals(chat.Pass))
                {
                    return Result.Fail<string>("Contraseña incorrecta");
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.text))
                        {
                            var resul = await insta.MessagingProcessor.SendDirectTextAsync(user.Value.Pk.ToString(), String.Empty, chat.text);
                            if (resul.Succeeded)
                            {
                                return Result.Success<string>(resul.Info.Message);
                            }
                            else
                            {
                                return Result.Fail<string>(resul.Info.Message);
                            }
                        }
                        else
                        {
                            return Result.Fail<string>("Introdusca el texto a enviar");
                        }
                       
                    }
                    else
                    {
                        return Result.Fail<string>(inboxThreads.Info.Message);
                    }
                }
                else
                {
                   return Result.Fail<string>(user.Info.Message);
                }
                              
            }
            catch (HttpRequestException httpException)
            {
                return Result.Fail(httpException, default(string), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<string>(ex);
            }
        }

        /// <summary>
        /// Metodo para enviar foto a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult<string>> SendPhoto(mchat chat)
        {
            try
            {
                
                mProxy proxyconnect = new mProxy();                
                enResponseToken token = new enResponseToken();
                List<mProxy> proxys = new List<mProxy>();

                proxys = prbd.CargarProxy();
                proxyconnect = util.ChoseProxy(proxys, chat.Country, 1);
                if (proxyconnect.ErrorResult)
                {
                    //insertar en la pila de errores de tareas de login pendientes pendientes
                    bd.InsertarLogin(chat);
                    //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                    //para esperar unos minutos.
                    return Result.Fail("No hay Proxys disponibles",(string)null);
                }
                else
                {
                    //update disponibilidad de los proxys. 
                    bdprox.Update_Proxy(proxyconnect, 1);
                }
                if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
                {
                    return Result.Fail("Deben introducir el Proxy completo",(string)null);
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

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    
                    return Result.Fail("Deben introducir Usuario y Contraseña",(string )null);
                }

                session.LoadSession(insta);

                var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

                if (!pass["Pass"].Equals(chat.Pass))
                {
                    return Result.Fail("Contraseña incorrecta",(string)null);
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.foto != null)
                        {
                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectPhotoToRecipientsAsync(chat.foto,recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                
                                return Result.Success(resul.Info.Message);
                            }
                            else
                            {
                                
                                return Result.Fail(resul.Info.Message,(string)null);
                            }
                        }
                        else
                        {                            
                            return Result.Fail("Debe introducir la foto a enviar", (string)null);
                        }                       
                    }
                    else
                    {                        
                        return Result.Fail(inboxThreads.Info.Message, (string)null); ;
                    }
                }
                else
                {                    
                    return Result.Fail(user.Info.Message, user.Info.Message);
                }

            }
            catch (HttpResponseException s)
            {
              return Result.Fail(s.Message,s.Message );
            }
        }

        /// <summary>
        /// Metodo para enviar video a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendVideo(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.video != null)
                        {
                            var videoupload = new InstaVideoUpload
                            {
                                Video = chat.video
                            };

                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectVideoToRecipientsAsync(videoupload, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el video a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar audio a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendAudio(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.audio != null)
                        {
                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectVoiceToRecipientsAsync(chat.audio, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el audio a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar Url a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendUrl(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.Url))
                        {
                            string[] recipient = new string[1];
                            string[] thread = new string[inboxThreads.Value.Inbox.Threads.Count];
                            recipient[0] = user.Value.Pk.ToString();

                            for (int i = 0; i < inboxThreads.Value.Inbox.Threads.Count; i++)
                            {
                                thread[i] = inboxThreads.Value.Inbox.Threads[i].ThreadId;
                            }

                            var resul = await insta.MessagingProcessor.SendDirectLinkAsync(chat.text,chat.Url,thread,recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el video a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar Objetos animados a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendEmoji(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.Giphyid))
                        {
                            
                            string[] thread = new string[1];

                            var resul = await insta.MessagingProcessor.SendDirectAnimatedMediaAsync(chat.Giphyid,thread);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el id del Emoticon a enviar a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Enviar un hashtag
        /// </summary>
        /// <returns></returns>
        public async Task<enResponseToken> AgregarHashtag(mchat media)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(media.User) || string.IsNullOrEmpty(media.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = media.User,
                        Password = media.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message= "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(media.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;

                }

                if (!string.IsNullOrEmpty(media.Hashtag) && !string.IsNullOrEmpty(media.text) && !string.IsNullOrEmpty(media.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(media.otheruser);
                    if (user.Succeeded)
                    {
                        var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                        if (!inboxThreads.Succeeded)
                        {

                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            string[] threads = new string[inboxThreads.Value.Inbox.Threads.Count];
                            for (int i = 0; i < inboxThreads.Value.Inbox.Threads.Count; i++)
                            {
                                threads[i] = inboxThreads.Value.Inbox.Threads[i].ThreadId;

                            }

                            var resul = await insta.MessagingProcessor.SendDirectHashtagAsync(media.text, media.Hashtag,threads, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = inboxThreads.Info.Message;
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = user.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = "Tiene que introducir el texto, el hashtag y el usuario al que le desea enviar el hashtag";
                    return token;
                }
                
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
        /// <summary>
        /// Enviar mensaje directo desde el search
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SendMessageFromSearch(mchat chat) 
        {
            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

            if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = chat.User,
                    Password = chat.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                return "Deben introducir Usuario y Contraseña";
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
            {
                return "Contraseña incorrecta";
            }
            var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(chat.otheruser), PaginationParameters.MaxPagesToLoad(1));

            if(instauser.Succeeded)
            {
                var getusuario = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (getusuario.Succeeded) 
                {
                    if (!string.IsNullOrEmpty(chat.text))
                    {
                        var resul = await insta.MessagingProcessor.SendDirectTextAsync(getusuario.Value.Pk.ToString(), String.Empty, chat.text);
                        if (resul.Succeeded)
                        {        
                            return resul.Info.Message;
                        }
                        else
                        {                            
                            return resul.Info.Message;
                        }
                    }
                    else
                    {
                        return "Introdusca el texto a enviar";
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Metodo para enviar un mensaje de texto a un usuario
        /// </summary>
        /// <param name="sending"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult<enResponseToken>> Mass_Sending(mMasSending sending)
        {
            try
            {
                mResultadoBd objResultado = new mResultadoBd();
                TareasBd objbd = new TareasBd();
                enResponseToken token = new enResponseToken();
                mProxy proxyconnect = new mProxy();
                List<mProxy> proxys = new List<mProxy>();

                proxys = prbd.CargarProxy();
                proxyconnect = util.ChoseProxy(proxys, sending.Country, 1);
                if (proxyconnect.ErrorResult)
                {
                    //insertar en la pila de errores de tareas de login pendientes pendientes
                    bd.InsertarLogin(sending);
                    //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                    //para esperar unos minutos.
                    token.result = InstaLoginResult.LimitError;
                    token.Message = "No hay Proxys disponibles";
                    return Result.Success(token);
                }
                else
                {
                    //update disponibilidad de los proxys. 
                    bdprox.Update_Proxy(proxyconnect, 1);
                }
                if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
                {
                    token.Message = "Deben introducir el Proxy completo";
                    return Result.Success(token);
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

                if (!(string.IsNullOrEmpty(sending.User) || string.IsNullOrEmpty(sending.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = sending.User,
                        Password = sending.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return Result.Success(token);
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(sending.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return Result.Success(token);
                }

                if (string.IsNullOrEmpty(sending.Texto))
                {
                    token.Message = "Debe Introducir el texto del mensaje a enviar";
                    return Result.Success(token);
                }
                var user = await insta.UserProcessor.GetUserAsync(sending.User);
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(string.Empty, PaginationParameters.MaxPagesToLoad(1));
                #region Enviar Si el cliente esta Online

                if (instauser.Succeeded)
                {
                    //var online = await insta.MessagingProcessor.GetUsersPresenceAsync();

                    int contador = 0;
                    string[] linea = sending.Usuarios.Split(',');

                    var resultado = await insta.MessagingProcessor.SendDirectTextAsync(sending.Usuarios, String.Empty, sending.Texto);

                    if (resultado.Succeeded)
                    {
                        mReports_Mess mReports_Mess = new mReports_Mess(resultado.Value.ThreadId, resultado.Value.ItemId, user.Value.Pk, linea.Length, contador, 0, 0, sending.Usuarios);
                        objResultado = objbd.Insertar_Reportes_Mensages(mReports_Mess);
                        token.Message = "De un total de:" + linea.Length + ", mensajes se enviaron:" + contador + ".";
                    }
                }
                else
                {
                    token.Message = instauser.Info.Message;
                }
                #endregion

                return Result.Success(token);
            }
            catch (HttpRequestException httpException)
            {
                return Result.Fail(httpException, default(enResponseToken), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<enResponseToken>(ex);
            }
        }

        private bool Aparece(string[] lista, string valor)
        {
            for (int i = 0; i < lista.Length; i++)
            {
                if (lista[i].Equals(valor))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Obtener Reporte de los mensajes enviados a los usuarios de los clientes
        /// </summary>
        /// <returns></returns>
        public async Task<List<mReports_Mess>> Reporte_Mensaje(mchat chat)
        {
            try
            {
                mResultadoBd objResultado = new mResultadoBd();
                TareasBd objbd = new TareasBd();
                List<mReports_Mess> mReports_Messes = new List<mReports_Mess>();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    return null;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.User);
                if (user.Succeeded)
                {
                   mReports_Messes = objbd.Get_Reports_Mess(user.Value.Pk);
                    for (int i = 0; i < mReports_Messes.Count; i++)
                    {                        
                        var thread = await insta.MessagingProcessor.GetDirectInboxThreadAsync(mReports_Messes[i].Thread_Id, PaginationParameters.MaxPagesToLoad(1));

                        if (thread.Succeeded)
                        {
                            //esto no me convence quiero verlo con raul
                            mReports_Messes[i].Cant_Vistos = thread.Value.Items[0].RavenSeenCount;
                            mReports_Messes[i].Cant_Reacc = thread.Value.Items[0].RavenSeenUserIds.Count;
                            objbd.Update_Reportes_Mensages(mReports_Messes[i]);
                        }
                    }
                }
                else
                {
                    return null;
                }

                return mReports_Messes;
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }
    }
}
