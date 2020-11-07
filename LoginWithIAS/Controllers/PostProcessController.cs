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

namespace LoginWithIAS.Controllers
{
    
    /// <summary>
    /// Controlador para dar likes
    /// </summary>
    public class PostProcessController : ApiController
    {
        Session session;
        Util util;
        Log log;
        string path = HttpContext.Current.Request.MapPath("~/Logs");
        /// <summary>
        /// constructor de la clase
        /// </summary>
        public PostProcessController()
        {
            
            session = new Session();
            util = new Util();
            log = new Log(path);
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
                    log.Add("Get post from "+ mlikemanypost.userlike+" -"+media.Info.Message);
                    if (media.Succeeded)
                    {
                        for (int i = 0; i < media.Value.Count; i++)
                        {
                            if (cantlike > 0)
                            {
                                var liked = await insta.MediaProcessor.LikeMediaAsync(media.Value[i].InstaIdentifier);
                                log.Add("like to "+ mlikemanypost.userlike +"-" +liked.Info.Message);

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
                            token.Message = "Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike + " y se encontraron los siguientes errores " + LogError ; }                    
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
        [HttpPost]
        public async Task<enResponseToken> SimulationLikeManyPost(mLikeManyPost mlikemanypost)
        {
            List<string> Error = new List<string>();
            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            string LogError = "No hubo errores";
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

            if (string.IsNullOrEmpty(mlikemanypost.userfollow))
            {
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userlike),PaginationParameters.MaxPagesToLoad(1));
                log.Add("Search user to like "+ mlikemanypost.userlike+"-"+ instauser.Info.Message);
                if (instauser.Succeeded)
                {
                    var getusuario = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);

                    if (getusuario.Succeeded)
                    {
                        var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                        log.Add(media.Info.Message);
                        if (media.Succeeded)
                        {
                            List<InstaMedia> milista = new List<InstaMedia>();
                            Random valorandon = new Random();
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
                                    log.Add("Like to "+mlikemanypost.userlike+"-"+liked.Info.Message);
                                    Thread.Sleep(mlikemanypost.time * 1000);

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
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = getusuario.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = instauser.Info.Message;
                    return token;
                }
            }
            else
            {
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(util.Subcadena(mlikemanypost.userfollow),PaginationParameters.MaxPagesToLoad(1));
                log.Add("Search userfollow to like "+ mlikemanypost.userfollow + "-"+ instauser.Info.Message);
                if (instauser.Succeeded)
                {
                    var getusuariofollowing = await insta.UserProcessor.GetUserAsync(mlikemanypost.userfollow);
                    log.Add("Get user follow to like "+ mlikemanypost.userfollow+"-"+ getusuariofollowing.Info.Message);
                    if (getusuariofollowing.Succeeded)
                    {
                        var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userfollow, PaginationParameters.MaxPagesToLoad(1));
                        log.Add("Get follower of "+ mlikemanypost.userfollow + "-" +  seguidores.Info.Message);
                        if (seguidores.Succeeded)
                        {
                            var getuserlike = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);
                            log.Add("Get user to like "+ mlikemanypost.userlike+"-"+getuserlike.Info.Message);
                            if (getuserlike.Succeeded)
                            {
                                var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));
                                log.Add("Get post to like "+ media.Info.Message);
                                if (media.Succeeded)
                                {
                                    List<InstaMedia> milista = new List<InstaMedia>();
                                    Random valorandon = new Random();
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
                                            log.Add("Like to "+ milista[i].InstaIdentifier+"-"+liked.Info.Message);
                                            Thread.Sleep(mlikemanypost.time * 1000);

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
                                    return token;
                                }
                            }
                            else
                            {
                                token.Message = getuserlike.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = seguidores.Info.Message;
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = getusuariofollowing.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = instauser.Info.Message;
                    return token;
                }                
            }
           
                foreach (string error in Error)
                    LogError += " " + error;
                token.Message = "Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike + " y se encontraron los siguientes errores " + LogError; 
            return token;

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
        public async Task<string> deletePost(mPost userpost)
        {
            try
            {
                bool eliminarpost = false;

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

                if (userpost.tipo_media == 1)
                {
                    var post = await insta.MediaProcessor.DeleteMediaAsync(userpost.idpost, InstaMediaType.Image);
                    if (post.Succeeded)
                        eliminarpost = post.Value;

                }
                else if (userpost.tipo_media == 2)
                {
                    var post = await insta.MediaProcessor.DeleteMediaAsync(userpost.idpost, InstaMediaType.Video);
                    if (post.Succeeded)
                        eliminarpost = post.Value;
                }
                else if (userpost.tipo_media == 8)
                {
                    var post = await insta.MediaProcessor.DeleteMediaAsync(userpost.idpost, InstaMediaType.Carousel);
                    if (post.Succeeded)
                        eliminarpost = post.Value;
                }
                if (!eliminarpost)
                    return "No se pudo encontrar el post, corrija el id del post o el tipo de post";
                return "Post eliminado con éxito.";
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
    }
}
