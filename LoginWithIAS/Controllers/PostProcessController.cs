using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using LoginWithIAS.Utiles;
using Microsoft.SqlServer.Server;
using System.Web;
using Telegram.Bot;
using System.Configuration;
using LoginWithIAS.ApiBd;
using LoginWithIAS.App_Start;

namespace LoginWithIAS.Controllers
{

    /// <summary>
    /// Controlador para dar likes
    /// </summary>
    public class PostProcessController : ApiController
    {
        Sesion session;
        Util util;
        Log log;
        string path = HttpContext.Current.Request.MapPath("~/Logs");
        TelegramBotClient botClient;
        ErrorBd objerror;
        ProxyBD prbd;
        ProxyBD bdprox;
        MloginBD bd;

        /// <summary>
        /// constructor de la clase
        /// </summary>
        public PostProcessController()
        {

            session = new Sesion();
            util = new Util();
            log = new Log(path);
            botClient = new TelegramBotClient(ConfigurationManager.AppSettings["AccesToken"]);
            objerror = new ErrorBd();
            prbd = new ProxyBD();
            bdprox = new ProxyBD();
            bd = new MloginBD();
        }

        /// <summary>
        /// Método para dar x cantidad de likes a un Usuario y
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> LikeManyPost(mLikeManyPost mlikemanypost)
        {
            try
            {
                List<string> Error = new List<string>();
                string LogError = "No hubo errores";
                int cantlike = mlikemanypost.cantLike;
                int count = 0;
                enResponseToken token = new enResponseToken();

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
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(mlikemanypost.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(mlikemanypost.userlike))
                {
                    var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                    log.Add("Get post from " + mlikemanypost.userlike + " -" + media.Info.Message);
                    if (media.Succeeded)
                    {
                        for (int i = 0; i < media.Value.Count; i++)
                        {
                            if (cantlike > 0)
                            {
                                var liked = await insta.MediaProcessor.LikeMediaAsync(media.Value[i].InstaIdentifier);
                                log.Add("like to " + mlikemanypost.userlike + "-" + liked.Info.Message);

                                if (liked.Succeeded)
                                {
                                    cantlike--;
                                    count++;
                                }
                                else
                                    Error.Add(liked.Info.Message);

                            }
                            else break;

                        }
                        if (count > 0) {
                            foreach (string error in Error)
                                LogError += " " + error;
                            token.Message = "Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike + " y se encontraron los siguientes errores " + LogError; }
                        return token;
                    }
                    else
                    {
                        token.Message = media.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = "Debe introducir el usuario al que se le quiere dar like";
                    return token;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Simular dar like a través del buscador o de la lista de seguidores
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        //[AuthorizationRequired]
        [HttpPost]
        public async Task<string> SimulationLikeManyPost(mMethodLike mlikemanypost)
        {
            List<string> Error = new List<string>();
            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            Random valorandon = new Random();
            string LogError = "No hubo errores";
            List<InstaMedia> milista = new List<InstaMedia>();
            enResponseToken token = new enResponseToken();
            string cadena = "abcde";
            mError TareaError = new mError();
            ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12;
            var me = botClient.GetMeAsync().Result;
            await botClient.SendTextMessageAsync(
            chatId: ConfigurationManager.AppSettings["ChannelId"],
            text: "Se ha iniciado la Tarea Expancion para el usuario: " + mlikemanypost.User + " a las:" + DateTime.Now

             );
            string PassTem = string.Empty;
            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

            if (!(string.IsNullOrEmpty(mlikemanypost.User) || string.IsNullOrEmpty(mlikemanypost.Pass)))
            {
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";

            }

            session.LoadSession(insta);

            if (!SecurityAPI.Decrypt(insta.GetLoggedUser().Password).Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Contraseña incorrecta";
            }

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);

            if (insta.IsUserAuthenticated)
            {
                count = 0;
                cantlike = mlikemanypost.cantLike;

                if (string.IsNullOrEmpty(mlikemanypost.userfollow))
                {
                    var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userlike), PaginationParameters.MaxPagesToLoad(1));
                    log.Add(mlikemanypost.User + " Search_user_to_like " + mlikemanypost.userlike + "-" + instauser.Info.Message);
                    if (instauser.Succeeded)
                    {
                        var getusuario = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);
                        var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                        if (getusuario.Succeeded)
                        {
                            log.Add(mlikemanypost.User + " Get user to like " + mlikemanypost.userlike + "-" + getusuario.Info.Message);
                            if (media.Succeeded)
                            {
                                log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + media.Info.Message);
                                if (media.Value.Count > 0)
                                {

                                    string accion = util.Accion(cadena);
                                    for (int i = 0; i < accion.Length; i++)
                                    {
                                        switch (accion[i])
                                        {
                                            case 'a':

                                                var mediass = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                                                log.Add(mlikemanypost.User + " Entrando al perfil y obteniendo post de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos " + " - " + mediass.Info.Message);
                                                Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));


                                                break;
                                            case 'b':
                                                if (media.Value.Count > 0)
                                                {

                                                    for (int j = 0; j < mlikemanypost.cant(mlikemanypost.vel); j++)
                                                    {
                                                        var post = await insta.MediaProcessor.GetMediaByIdAsync(media.Value[valorandon.Next(0, media.Value.Count)].InstaIdentifier);
                                                        log.Add(mlikemanypost.User + " abriendo post de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para abrir otro " + " - " + post.Info.Message);
                                                        Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel) * 1000);
                                                    }
                                                }
                                                break;
                                            case 'c':
                                                var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                                log.Add(mlikemanypost.User + " viendo seguidores de " + mlikemanypost.userlike + " - " + seguidores.Info.Message);
                                                break;
                                            case 'd':
                                                var direct = await insta.MessagingProcessor.GetPendingDirectAsync(PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                                log.Add(mlikemanypost.User + " abriendo direct de message " + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos" + " - " + direct.Info.Message);
                                                Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                break;
                                            case 'e':
                                                var historias = await insta.StoryProcessor.GetUserStoryAsync(getusuario.Value.Pk);
                                                if (historias.Succeeded)
                                                {
                                                    if (historias.Value.Items.Count > 0)
                                                    {
                                                        for (int j = 0; j < mlikemanypost.cantInter(mlikemanypost.vel); j++)
                                                        {
                                                            int posicion = valorandon.Next(0, historias.Value.Items.Count);
                                                            long takenat = historias.Value.TakenAtUnix;
                                                            var historia = await insta.StoryProcessor.MarkStoryAsSeenAsync(historias.Value.Items[posicion].Id, takenat);
                                                            log.Add(mlikemanypost.User + " viendo historia de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para ver otra " + " - " + historia.Info.Message);
                                                            Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                        }
                                                    }
                                                }
                                                break;

                                        }
                                        Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.time) * 1000);
                                    }

                                    for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                                    {
                                        if (!media.Value[i].HasLiked)
                                        {
                                            if (milista.Count <= 6)
                                            {
                                                milista.Add(media.Value[i]);
                                            }
                                            else
                                                break;
                                        }
                                    }

                                    for (int i = 0; i < milista.Count; i++)
                                    {
                                        if (cantlike > 0)
                                        {
                                            var liked = await insta.MediaProcessor.LikeMediaAsync(milista[i].InstaIdentifier);
                                            Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.vel) * 1000);
                                            if (liked.Succeeded)
                                            {
                                                log.Add(mlikemanypost.User + " Like_to " + mlikemanypost.userlike + " - " + liked.Info.Message);
                                                cantlike--;
                                                count++;
                                            }
                                            else
                                            {
                                                log.Add(mlikemanypost.User + " Like_to " + mlikemanypost.userlike + " - " + liked.Info.Message);
                                                TareaError = objerror.SysError(liked.Info.Message);
                                                switch (TareaError.action)
                                                {
                                                    case 1:
                                                        return "1";

                                                    case 2:
                                                        return "2";
                                                    case 3:
                                                        return "3";
                                                    case 4:
                                                        await botClient.SendTextMessageAsync(
                                                        chatId: ConfigurationManager.AppSettings["ChannelId"],
                                                        text: "Ha ocurrido un error no registrado con el usuario : " + mlikemanypost.User + " a las:" + DateTime.Now + " tipo de error: " + liked.Info.Message
                                                         );
                                                        return "4";


                                                }
                                                Error.Add(liked.Info.Message);
                                            }
                                        }
                                        else break;
                                    }
                                }
                                else
                                {
                                    log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + " No tiene publicaciones");
                                    return " No tiene publicaciones";

                                }
                            }
                            else
                            {
                                log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + media.Info.Message);
                                return media.Info.Message;

                            }
                        }
                        else
                        {
                            log.Add(mlikemanypost.User + " Get_User_to_like " + mlikemanypost.userlike + "-" + getusuario.Info.Message);
                            return getusuario.Info.Message;

                        }
                    }
                    else
                    {
                        log.Add(mlikemanypost.User + " Search_User_to_like " + mlikemanypost.userlike + "-" + instauser.Info.Message);
                        return instauser.Info.Message;
                    }
                }
                else
                {
                    var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userfollow), PaginationParameters.MaxPagesToLoad(1));
                    log.Add("Search userfollow to like " + mlikemanypost.userfollow + "-" + instauser.Info.Message);
                    if (instauser.Succeeded)
                    {
                        var getusuariofollowing = await insta.UserProcessor.GetUserAsync(mlikemanypost.userfollow);
                        log.Add("Get user follow to like " + mlikemanypost.userfollow + "-" + getusuariofollowing.Info.Message);
                        if (getusuariofollowing.Succeeded)
                        {
                            var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userfollow, PaginationParameters.MaxPagesToLoad(1));
                            log.Add("Get follower of " + mlikemanypost.userfollow + "-" + seguidores.Info.Message);
                            if (seguidores.Succeeded)
                            {
                                var getuserlike = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);
                                log.Add("Get user to like " + mlikemanypost.userlike + "-" + getuserlike.Info.Message);
                                if (getuserlike.Succeeded)
                                {
                                    var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                                    log.Add("Get post to like " + media.Info.Message);
                                    if (media.Succeeded)
                                    {

                                        for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                                        {
                                            if (!media.Value[i].HasLiked)
                                            {
                                                if (milista.Count <= 6)
                                                {
                                                    milista.Add(media.Value[i]);
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        for (int i = 0; i < milista.Count; i++)
                                        {
                                            if (cantlike > 0)
                                            {
                                                var liked = await insta.MediaProcessor.LikeMediaAsync(milista[i].InstaIdentifier);
                                                log.Add("Like to " + milista[i].InstaIdentifier + "-" + liked.Info.Message);
                                                Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.vel) * 1000);

                                                if (liked.Succeeded)
                                                {
                                                    cantlike--;
                                                    count++;
                                                }
                                                else
                                                    Error.Add(liked.Info.Message);

                                            }
                                            else break;

                                        }
                                    }
                                    else
                                    {
                                        token.Message = media.Info.Message;
                                        return media.Info.Message;
                                    }
                                }
                                else
                                {
                                    token.Message = getuserlike.Info.Message;
                                    return getuserlike.Info.Message;
                                }
                            }
                            else
                            {
                                token.Message = seguidores.Info.Message;
                                return seguidores.Info.Message;
                            }
                        }
                        else
                        {
                            token.Message = getusuariofollowing.Info.Message;
                            return getusuariofollowing.Info.Message;
                        }
                    }
                    else
                    {
                        token.Message = instauser.Info.Message;
                        return instauser.Info.Message;
                    }
                }

                foreach (string error in Error)
                    LogError += " " + error;
                log.Add("Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike + " y se encontraron los siguientes errores " + LogError);
                return "Método expanción realizado con éxito con el fan: " + mlikemanypost.userlike;

            }
            else
            {
                return "Debe autenticarse primero";

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async void SimulationHumanLikeManyPost(mMethodLike mlikemanypost)
        {
            List<string> Error = new List<string>();
            List<string> userlista = mlikemanypost.ListUser;
            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            Random valorandon = new Random();
            string LogError = "No hubo errores";
            List<InstaMedia> milista = new List<InstaMedia>();
            enResponseToken token = new enResponseToken();
            string cadena = "abcde";
            DateTime Dia = DateTime.Now;
            DateTime hora = DateTime.Now;
            int cicloHora = 1;
            int cicloDia = 1;
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, mlikemanypost.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(mlikemanypost);
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (!string.IsNullOrEmpty(proxyconnect.AddressProxy) || !string.IsNullOrEmpty(proxyconnect.UsernameProxy) || !string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                token.Message = "Deben introducir el Proxy completo";
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
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return;
            }

            session.LoadSession(insta);
            var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

            if (!pass["Pass"].Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return;
            }

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);
            if (insta.IsUserAuthenticated)
            {
                for (int y = 0; y < userlista.Count; y++)
                {
                    milista.Clear();
                    count = 0;
                    cantlike = mlikemanypost.cantLike;

                    if (string.IsNullOrEmpty(mlikemanypost.userfollow))
                    {
                        var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(userlista[y]), PaginationParameters.MaxPagesToLoad(1));
                        log.Add(mlikemanypost.User + " Search_user_to_like " + userlista[y] + "-" + instauser.Info.Message);
                        if (instauser.Succeeded)
                        {
                            var getusuario = await insta.UserProcessor.GetUserAsync(userlista[y]);
                            var media = await insta.UserProcessor.GetUserMediaAsync(userlista[y], PaginationParameters.MaxPagesToLoad(1));
                            if (getusuario.Succeeded)
                            {
                                log.Add(mlikemanypost.User + " Get user to like " + userlista[y] + "-" + getusuario.Info.Message);
                                if (media.Succeeded)
                                {
                                    log.Add(mlikemanypost.User + " Get_media_to_like " + userlista[y] + "-" + media.Info.Message);
                                    if (media.Value.Count > 0)
                                    {

                                        string accion = util.Accion(cadena);
                                        for (int i = 0; i < accion.Length; i++)
                                        {
                                            switch (accion[i])
                                            {
                                                case 'a':
                                                    
                                                        var mediass = await insta.UserProcessor.GetUserMediaAsync(userlista[y], PaginationParameters.MaxPagesToLoad(1));
                                                        log.Add(mlikemanypost.User + " Entrando al perfil y obteniendo post de " + userlista[y] + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos " + " - " + mediass.Info.Message);
                                                        Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                    

                                                    break;
                                                case 'b':
                                                    if (media.Value.Count > 0)
                                                    {

                                                        for (int j = 0; j < mlikemanypost.cant(mlikemanypost.vel); j++)
                                                        {
                                                            var post = await insta.MediaProcessor.GetMediaByIdAsync(media.Value[valorandon.Next(0, media.Value.Count)].InstaIdentifier);
                                                            log.Add(mlikemanypost.User + " abriendo post de " + userlista[y] + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para abrir otro " + " - " + post.Info.Message);
                                                            Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel) * 1000);
                                                        }
                                                    }
                                                    break;
                                                case 'c':
                                                    var seguidores = await insta.UserProcessor.GetUserFollowersAsync(userlista[y], PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                                    log.Add(mlikemanypost.User + " viendo seguidores de " + userlista[y] + " - " + seguidores.Info.Message);
                                                    break;
                                                case 'd':
                                                    var direct = await insta.MessagingProcessor.GetPendingDirectAsync(PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                                    log.Add(mlikemanypost.User + " abriendo direct de message " + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos" + " - " + direct.Info.Message);
                                                    Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                    break;
                                                case 'e':
                                                    var historias = await insta.StoryProcessor.GetUserStoryAsync(getusuario.Value.Pk);
                                                    if (historias.Succeeded)
                                                    {
                                                        if (historias.Value.Items.Count > 0)
                                                        {
                                                            for (int j = 0; j < mlikemanypost.cantInter(mlikemanypost.vel); j++)
                                                            {
                                                                int posicion = valorandon.Next(0, historias.Value.Items.Count);
                                                                long takenat = historias.Value.TakenAtUnix;
                                                                var historia = await insta.StoryProcessor.MarkStoryAsSeenAsync(historias.Value.Items[posicion].Id, takenat);
                                                                log.Add(mlikemanypost.User + " viendo historia de " + userlista[y] + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para ver otra " + " - " + historia.Info.Message);
                                                                Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                            }
                                                        }
                                                    }
                                                    break;

                                            }
                                            Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.time) * 1000);
                                        }

                                        for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                                        {
                                            if (!media.Value[i].HasLiked)
                                            {
                                                if (milista.Count <= 6)
                                                {
                                                    milista.Add(media.Value[i]);
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        for (int i = 0; i < milista.Count; i++)
                                        {
                                            if (cantlike > 0)
                                            {
                                                var liked = await insta.MediaProcessor.LikeMediaAsync(milista[i].InstaIdentifier);
                                                Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.vel) * 1000);
                                                if (liked.Succeeded)
                                                {
                                                    log.Add(mlikemanypost.User + " Like_to " + userlista[y] + " - " + liked.Info.Message);
                                                    cantlike--;
                                                    count++;
                                                }
                                                else
                                                {
                                                    log.Add(mlikemanypost.User + " Like_to " + userlista[y] + " - " + liked.Info.Message);
                                                    if (liked.Info.Message.Equals("feedback_required"))
                                                        return;
                                                    Error.Add(liked.Info.Message);
                                                }
                                            }
                                            else break;
                                        }
                                    }
                                    else
                                    {
                                        log.Add(mlikemanypost.User + " Get_media_to_like " + userlista[y] + "-" + " No tiene publicaciones");

                                    }
                                }
                                else
                                {
                                    log.Add(mlikemanypost.User + " Get_media_to_like " + userlista[y] + "-" + media.Info.Message);
                                }
                            }
                            else
                            {
                                log.Add(mlikemanypost.User + " Get_User_to_like " + userlista[y] + "-" + getusuario.Info.Message);
                            }
                        }
                        else
                        {
                            log.Add(mlikemanypost.User + " Search_User_to_like " + userlista[y] + "-" + instauser.Info.Message);

                        }
                    }
                    else
                    {
                        var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userfollow), PaginationParameters.MaxPagesToLoad(1));
                        log.Add("Search userfollow to like " + mlikemanypost.userfollow + "-" + instauser.Info.Message);
                        if (instauser.Succeeded)
                        {
                            var getusuariofollowing = await insta.UserProcessor.GetUserAsync(mlikemanypost.userfollow);
                            log.Add("Get user follow to like " + mlikemanypost.userfollow + "-" + getusuariofollowing.Info.Message);
                            if (getusuariofollowing.Succeeded)
                            {
                                var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userfollow, PaginationParameters.MaxPagesToLoad(1));
                                log.Add("Get follower of " + mlikemanypost.userfollow + "-" + seguidores.Info.Message);
                                if (seguidores.Succeeded)
                                {
                                    var getuserlike = await insta.UserProcessor.GetUserAsync(userlista[y]);
                                    log.Add("Get user to like " + userlista[y] + "-" + getuserlike.Info.Message);
                                    if (getuserlike.Succeeded)
                                    {
                                        var media = await insta.UserProcessor.GetUserMediaAsync(userlista[y], PaginationParameters.MaxPagesToLoad(1));
                                        log.Add("Get post to like " + media.Info.Message);
                                        if (media.Succeeded)
                                        {

                                            for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                                            {
                                                if (!media.Value[i].HasLiked)
                                                {
                                                    if (milista.Count <= 6)
                                                    {
                                                        milista.Add(media.Value[i]);
                                                    }
                                                    else
                                                        break;
                                                }
                                            }

                                            for (int i = 0; i < milista.Count; i++)
                                            {
                                                if (cantlike > 0)
                                                {
                                                    var liked = await insta.MediaProcessor.LikeMediaAsync(milista[i].InstaIdentifier);
                                                    log.Add("Like to " + milista[i].InstaIdentifier + "-" + liked.Info.Message);
                                                    Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.vel) * 1000);

                                                    if (liked.Succeeded)
                                                    {
                                                        cantlike--;
                                                        count++;
                                                    }
                                                    else
                                                        Error.Add(liked.Info.Message);

                                                }
                                                else break;

                                            }
                                        }
                                        else
                                        {
                                            token.Message = media.Info.Message;

                                        }
                                    }
                                    else
                                    {
                                        token.Message = getuserlike.Info.Message;

                                    }
                                }
                                else
                                {
                                    token.Message = seguidores.Info.Message;

                                }
                            }
                            else
                            {
                                token.Message = getusuariofollowing.Info.Message;

                            }
                        }
                        else
                        {
                            token.Message = instauser.Info.Message;

                        }
                    }

                    foreach (string error in Error)
                        LogError += " " + error;
                    log.Add("Se le dio like a " + count + " post del usuario " + userlista[y] + " y se encontraron los siguientes errores " + LogError);

                    if (y == (mlikemanypost.SleepHora(mlikemanypost.vel)-1)*cicloHora)
                    {
                        cicloHora++;
                        if ((DateTime.Now - hora).TotalMinutes < 60)
                        {
                            Thread.Sleep(valorandon.Next(45, 61)*60 * 1000);
                        }
                        hora = DateTime.Now;
                    }
                    if (y >= (mlikemanypost.SleepDia(mlikemanypost.vel)-1)*cicloDia)
                    {
                        cicloDia++;
                        if ((DateTime.Now - Dia).TotalHours < 24)
                        {
                            Thread.Sleep(3600 * 1000 * 10);
                        }                        
                        Dia = DateTime.Now;
                    }

                }
            }
            else { log.Add("Debe autenticarse primero"); }
        }

        /// <summary>
        /// Obtener un post
        /// </summary>
        /// <param name="userpost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaMedia> extraerPost(mPost userpost)
        {
            try
            {
                InstaMedia obj = new InstaMedia();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(userpost.User) || string.IsNullOrEmpty(userpost.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = userpost.User,
                        Password = userpost.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(userpost.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(userpost.idpost))
                {
                    var post = await insta.MediaProcessor.GetMediaByIdAsync(userpost.idpost);

                    if (post.Succeeded)
                    {
                        obj = post.Value;
                        return obj;
                    }
                }
                return null;
                
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Eliminar un post
        /// </summary>
        /// <param name="userpost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> deleteallPost(mFollower userpost)
        {
            try
            {                
                int count = 0;                
                InstaMediaList listpost = new InstaMediaList() ;
                PaginationParameters pagination = PaginationParameters.MaxPagesToLoad(1);
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(userpost.User) || string.IsNullOrEmpty(userpost.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = userpost.User,
                        Password = userpost.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return "Deben introducir Usuario y Contraseña";

                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(userpost.Pass))
                {
                    return "Contraseña incorrecta";

                }
                var user = await insta.UserProcessor.GetUserAsync(userpost.User);
                var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                var cantpost = userinfo.Value.UserDetail.MediaCount;
                long countpost = cantpost / 12;
                for (int j = 0; j < countpost+1; j++)
                {


                    var listmedia = await insta.UserProcessor.GetUserMediaAsync(userpost.User, pagination);
                    if (listmedia.Succeeded)
                    {
                        for (int i = 0; i < listmedia.Value.Count; i++)
                        {
                            listpost.Add(listmedia.Value[i]);
                        }
                    }                  
                }
                for (int i = 0; i < listpost.Count; i++)
                {


                    if (listpost[i].MediaType == InstaMediaType.Image)
                    {
                        var post = await insta.MediaProcessor.DeleteMediaAsync(listpost[i].InstaIdentifier, InstaMediaType.Image);
                        if (post.Succeeded)
                        { count++; log.Add("post " + count + " eliminado"); }
                           
                        else
                            log.Add(post.Info.Message);
                    }
                    else if (listpost[i].MediaType == InstaMediaType.Video)
                    {
                        var post = await insta.MediaProcessor.DeleteMediaAsync(listpost[i].InstaIdentifier, InstaMediaType.Video);
                        if (post.Succeeded)
                        { count++; log.Add("post " + count + " eliminado"); }
                        else
                            log.Add(post.Info.Message);
                    }
                    else if (listpost[i].MediaType == InstaMediaType.Carousel)
                    {
                        var post = await insta.MediaProcessor.DeleteMediaAsync(listpost[i].InstaIdentifier, InstaMediaType.Carousel);
                        if (post.Succeeded)
                        { count++; log.Add("post " + count + " eliminado"); }
                        else
                            log.Add( post.Info.Message);
                    }
                   
                }
                return "fin de la tarea eliminados " + count + " post";
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Extraer un Caption
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaCaption> ExtraerCaption(mPost post) 
        {
            try
            {
                InstaCaption caption = new InstaCaption();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(post.User) || string.IsNullOrEmpty(post.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = post.User,
                        Password = post.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;

                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(post.Pass))
                {
                    return null;

                }

                if (!string.IsNullOrEmpty(post.idpost))
                {
                    var media = await insta.MediaProcessor.GetMediaByIdAsync(post.idpost);
                    if (media.Succeeded)
                    {
                        caption = media.Value.Caption;
                    }
                    return caption;
                }
                else
                    return null;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }        
        }

        /// <summary>
        /// Editar Caption
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaCaption> EditarCaption(mcaption caption)
        {
            try
            {
                InstaCaption instacaption = new InstaCaption();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(caption.User) || string.IsNullOrEmpty(caption.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = caption.User,
                        Password = caption.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(caption.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(caption.Idmedia))
                {
                    var media = await insta.MediaProcessor.GetMediaByIdAsync(caption.Idmedia);
                    if (media.Succeeded)
                    {
                        instacaption = media.Value.Caption;
                        instacaption.Text = caption.texto;
                    }
                    return instacaption;
                }
                else
                    return null;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
           
        }

        /// <summary>
        /// Devuelbe la lista de Likers
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<InstaUserShort>> ListaLikers(mPost media)
        {
            try
            {
                List<InstaUserShort> finallikers = new List<InstaUserShort>();

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
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(media.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(media.idpost))
                {
                    var objmedia = await insta.MediaProcessor.GetMediaByIdAsync(media.idpost);
                    if (objmedia.Succeeded)
                    {
                        finallikers = objmedia.Value.Likers;
                    }
                    return finallikers;
                }
                else
                    return null;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }         
        }

        /// <summary>
        /// Like aun post x del feed
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> LikeaPostFeed(mPost media) 
        {
            Random objram = new Random();
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
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(media.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var feed = await insta.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

                if (feed.Succeeded)
                {
                    var medias = feed.Value.Medias;
                    
                    for (int i = 0; i < medias.Count; i++)
                    {
                        int post = objram.Next(1, medias.Count);
                        if (!medias[post].HasLiked)
                        {
                            var like = await insta.MediaProcessor.LikeMediaAsync(medias[post - 1].Pk);
                            if (like.Succeeded)
                            {
                                token.Message = "Se le ha dado like al post: " + medias[post - 1].InstaIdentifier;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }

                        }

                    }                    
                    

                }

                token.Message = "No se encontró el post, rectifique el id.";
                return token;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }        
        }   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Engagement(mMethodLike mlikemanypost) 
        {
            List<InstaMedia> milista = new List<InstaMedia>();
            Random valorandon = new Random();
            string PassTem = string.Empty;
            string cadena = "abcde";
            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

            if (!(string.IsNullOrEmpty(mlikemanypost.User) || string.IsNullOrEmpty(mlikemanypost.Pass)))
            {
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";

            }
            session.LoadSession(insta);

            if (!SecurityAPI.Decrypt(insta.GetLoggedUser().Password).Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Contraseña incorrecta";
            }
            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);
            
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userlike), PaginationParameters.MaxPagesToLoad(1));
                log.Add(mlikemanypost.User + " Search_user_to_like " + mlikemanypost.userlike + "-" + instauser.Info.Message);
                if (instauser.Succeeded)
                {
                    var getusuario = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);
                    var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                    if (getusuario.Succeeded)
                    {
                        log.Add(mlikemanypost.User + " Get user to like " + mlikemanypost.userlike + "-" + getusuario.Info.Message);
                        if (media.Succeeded)
                        {
                            log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + media.Info.Message);
                            if (media.Value.Count > 0)
                            {

                                string accion = util.Accion(cadena);
                                for (int i = 0; i < accion.Length; i++)
                                {
                                    switch (accion[i])
                                    {
                                        case 'a':

                                            var mediass = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                                            log.Add(mlikemanypost.User + " Entrando al perfil y obteniendo post de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos " + " - " + mediass.Info.Message);
                                            Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));


                                            break;
                                        case 'b':
                                            if (media.Value.Count > 0)
                                            {

                                                for (int j = 0; j < mlikemanypost.cant(mlikemanypost.vel); j++)
                                                {
                                                    var post = await insta.MediaProcessor.GetMediaByIdAsync(media.Value[valorandon.Next(0, media.Value.Count)].InstaIdentifier);
                                                    log.Add(mlikemanypost.User + " abriendo post de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para abrir otro " + " - " + post.Info.Message);
                                                    Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel) * 1000);
                                                }
                                            }
                                            break;
                                        case 'c':
                                            var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                            log.Add(mlikemanypost.User + " viendo seguidores de " + mlikemanypost.userlike + " - " + seguidores.Info.Message);
                                            break;
                                        case 'd':
                                            var direct = await insta.MessagingProcessor.GetPendingDirectAsync(PaginationParameters.MaxPagesToLoad(mlikemanypost.cantInter(mlikemanypost.vel)));
                                            log.Add(mlikemanypost.User + " abriendo direct de message " + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos" + " - " + direct.Info.Message);
                                            Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                            break;
                                        case 'e':
                                            var historias = await insta.StoryProcessor.GetUserStoryAsync(getusuario.Value.Pk);
                                            if (historias.Succeeded)
                                            {
                                                if (historias.Value.Items.Count > 0)
                                                {
                                                    for (int j = 0; j < mlikemanypost.cantInter(mlikemanypost.vel); j++)
                                                    {
                                                        int posicion = valorandon.Next(0, historias.Value.Items.Count);
                                                        long takenat = historias.Value.TakenAtUnix;
                                                        var historia = await insta.StoryProcessor.MarkStoryAsSeenAsync(historias.Value.Items[posicion].Id, takenat);
                                                        log.Add(mlikemanypost.User + " viendo historia de " + mlikemanypost.userlike + " se esperan " + mlikemanypost.tiempoInter(mlikemanypost.vel) + " segundos para ver otra " + " - " + historia.Info.Message);
                                                        Thread.Sleep(mlikemanypost.tiempoInter(mlikemanypost.vel));
                                                    }
                                                }
                                            }
                                            break;

                                    }
                                    Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.time) * 1000);
                                }
                            return string.Empty;
                        }
                            else
                            {
                                log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + " No tiene publicaciones");
                                return " No tiene publicaciones";

                            }
                        }
                        else
                        {
                            log.Add(mlikemanypost.User + " Get_media_to_like " + mlikemanypost.userlike + "-" + media.Info.Message);
                            return media.Info.Message;

                        }
                    }
                    else
                    {
                        log.Add(mlikemanypost.User + " Get_User_to_like " + mlikemanypost.userlike + "-" + getusuario.Info.Message);
                        return getusuario.Info.Message;

                    }
                
                }
                else
                {
                    log.Add(mlikemanypost.User + " Search_User_to_like " + mlikemanypost.userlike + "-" + instauser.Info.Message);
                    return instauser.Info.Message;
                }         


        }
        #region Metodos prueba
        /// <summary>
        /// Dar un like
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DarLike(mLikeManyPost mlikemanypost)
        {
            Random valorandon = new Random();
            List<InstaMedia> milista = new List<InstaMedia>();
            enResponseToken token = new enResponseToken();
            DateTime Dia = DateTime.Now;
            DateTime hora = DateTime.Now;
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
                return "No hay Proxys disponibles";
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                return "Deben introducir el Proxy completo";


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
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";
            }

            session.LoadSession(insta);
            var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

            if (!pass["Pass"].Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Contraseña incorrecta";
            }

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);
            if (insta.IsUserAuthenticated)
            {
                var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                {
                    if (!media.Value[i].HasLiked)
                    {
                        if (milista.Count <= 6)
                        {
                            milista.Add(media.Value[i]);
                        }
                        else
                            break;
                    }
                }                 
                        var liked = await insta.MediaProcessor.LikeMediaAsync(milista[0].InstaIdentifier);
                        if (liked.Succeeded)
                        {
                            bdprox.Update_Proxy(proxyconnect, -1);
                            log.Add(mlikemanypost.User + " Like_to " + mlikemanypost.userlike + " - " + liked.Info.Message);
                            return mlikemanypost.User + " Like_to " + mlikemanypost.userlike + " - " + liked.Info.Message;
                        }
                        else
                        {
                            log.Add(mlikemanypost.User + " Like_to " + mlikemanypost.userlike + " - " + liked.Info.Message);
                            if (liked.Info.Message.Equals("feedback_required"))
                               return "feedback_required";                           
                        }
                   
                
            }
            else
                return "Debe autenticarse ";
            return string.Empty;
        }
        /// <summary>
        /// Dar varios like
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DarvariosLike(mMethodLike mlikemanypost)
        {
            List<string> Error = new List<string>();
            List<string> userlista = mlikemanypost.ListUser;
            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            Random valorandon = new Random();
            List<InstaMedia> milista = new List<InstaMedia>();
            DateTime Dia = DateTime.Now;
            DateTime hora = DateTime.Now;
            int cicloHora = 1;
            int cicloDia = 1;
            LoginProcessController login = new LoginProcessController();
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
                return "No hay Proxys disponibles";
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                return "Deben introducir el Proxy completo";
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
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";
            }

            session.LoadSession(insta);
            var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

            if (!pass["Pass"].Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Contraseña incorrecta";
            }

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);
            if (insta.IsUserAuthenticated)
            {
                for (int y = 0; y < userlista.Count; y++)
                {
                    milista.Clear();
                    count = 0;
                    cantlike = mlikemanypost.cantLike;
                    var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                    for (int i = 0; i < media.Value.Count; i += valorandon.Next(1, 3))
                    {
                        if (!media.Value[i].HasLiked)
                        {
                            if (milista.Count <= 6)
                            {
                                milista.Add(media.Value[i]);
                            }
                            else
                                break;
                        }
                    }

                    for (int i = 0; i < milista.Count; i++)
                    {
                        if (cantlike > 0)
                        {
                            var liked = await insta.MediaProcessor.LikeMediaAsync(milista[i].InstaIdentifier);
                            Thread.Sleep(mlikemanypost.tiempo(mlikemanypost.vel) * 1000);
                            if (liked.Succeeded)
                            {
                                log.Add(mlikemanypost.User + " Like_to " + userlista[y] + " - " + liked.Info.Message);
                                cantlike--;
                                count++;
                            }
                            else
                            {
                                log.Add(mlikemanypost.User + " Like_to " + userlista[y] + " - " + liked.Info.Message);
                                if (liked.Info.Message.Equals("feedback_required")) 
                                {
                                    var relogin = await login.Relogin(mlikemanypost);
                                    if(relogin.AuthToken == null)
                                        return "feedback_required";
                                }                                    
                            }
                        }
                        else break;
                    }
                    if (y == (mlikemanypost.SleepHora(mlikemanypost.vel) - 1) * cicloHora)
                    {
                        cicloHora++;
                        if ((DateTime.Now - hora).TotalMinutes < 60)
                        {
                            Thread.Sleep(valorandon.Next(45, 61) * 60 * 1000);
                        }
                        hora = DateTime.Now;
                    }
                    if (y >= (mlikemanypost.SleepDia(mlikemanypost.vel) - 1) * cicloDia)
                    {
                        cicloDia++;
                        if ((DateTime.Now - Dia).TotalHours < 24)
                        {
                            Thread.Sleep(3600 * 1000 * 10);
                        }
                        Dia = DateTime.Now;
                    }

                }
            }
            else
                return "Debe autenticarse ";
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> cargarFeed(mLikeManyPost mlikemanypost)
        {
            Random valorandon = new Random();
            List<InstaMedia> milista = new List<InstaMedia>();
            enResponseToken token = new enResponseToken();
            DateTime Dia = DateTime.Now;
            DateTime hora = DateTime.Now;
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
                return "No hay Proxys disponibles";
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                return "Deben introducir el Proxy completo";
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
                var userSessiontemp = new UserSessionData
                {
                    UserName = mlikemanypost.User,
                    Password = mlikemanypost.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";
            }

            session.LoadSession(insta);
            var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

            if (!pass["Pass"].Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Contraseña incorrecta";
            }

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            insta.SetUser(userSession);
            if (insta.IsUserAuthenticated)
            {
                var feed = await insta.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

                if (feed.Succeeded)
                {
                    return feed.Info.Message;
                }
                else
                    return feed.Info.Message;

            }
            else
                return "Debe autenticarse ";
           
        }
        #endregion
    }
}
