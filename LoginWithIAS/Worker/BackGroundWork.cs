using System;
using System.Collections.Generic;
using System.ComponentModel;
using LoginWithIAS.Controllers;
using LoginWithIAS.Models;
using LoginWithIAS.ApiBd;
using System.Linq;
using System.Web;
using System.Threading;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using InstagramApiSharp.Classes;
using LoginWithIAS.Utiles;
using InstagramApiSharp;

namespace LoginWithIAS.Worker
{
    /// <summary>
    /// Traajos En segundo Plano
    /// </summary>
    public class BackGroundWork
    {
        Session session;
        List<Thread> threads = new List<Thread>();
        Log log;
        Util util;
        string path = HttpContext.Current.Request.MapPath("~/Logs");
        public BackGroundWork()
        {

            session = new Session();
            util = new Util();
            log = new Log(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="follower"></param>
        public void ejecutarTareas(int id, string name,mFollower follower) 
        {
            switch (id)
            {
                case 1: Thread hilo = new Thread(AudienceAlgorithm);
                    hilo.Name = name;
                    hilo.IsBackground = true;
                    hilo.Start(follower);
                    threads.Add(hilo);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="razon"></param>
        public void pararTarea(string name, string razon) 
        {
            for (int i = 0; i < threads.Count; i++)
            {
                if (threads[i].Name == name)
                    threads[i].Abort(razon);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void AudienceAlgorithm(object data) 
        {
            var follower = (mFollower)data;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public async void SimulationHumanLikeManyPost(object data)
        {
            var mlikemanypost = (mMethodLike) data;
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
                return;
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(mlikemanypost.Pass))
            {
                log.Add("Contraseña incorrecta");
                return;
            }
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
                                                    if (liked.Info.Equals("feedback_required"))
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
                            Thread.Sleep(86400 * 1000 * 10);
                        }                        
                        Dia = DateTime.Now;
                    }

                }
            }
            else { log.Add("Debe autenticarse primero"); }
        }
      
    }
}