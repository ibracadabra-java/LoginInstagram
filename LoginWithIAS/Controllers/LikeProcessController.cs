using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
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

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador para dar likes
    /// </summary>
    public class LikeProcessController : ApiController
    {
        Session session;

        /// <summary>
        /// constructor de la clase
        /// </summary>
        public LikeProcessController()
        {
            session = new Session();
        }

        /// <summary>
        /// Método para dar x cantidad de likes a un Usuario y
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LikeManyPost(mLikeManyPost mlikemanypost)
        {
            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.user,
                Password = mlikemanypost.pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));

            if (media.Succeeded)
            {
                for (int i = 0; i < media.Value.Count; i++)
                {
                    if (cantlike > 0)
                    {
                        var liked = await insta.MediaProcessor.LikeMediaAsync(media.Value[i].InstaIdentifier);

                        if (liked.Succeeded)
                        {
                            cantlike--;
                            count++;
                        }

                    }
                    else break;

                }
                return "Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike;
            }
            return media.Info.Message;

        }

        /// <summary>
        /// Metodo para dar likes aleatorios.
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SimulationLikeManyPost(mLikeManyPost mlikemanypost)
        {

            int cantlike = mlikemanypost.cantLike;
            int count = 0;
            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.user,
                Password = mlikemanypost.pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            if (mlikemanypost.userfollow == "")
            {
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(Subcadena(mlikemanypost.userlike));

                var getusuario = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);

                var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));

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
                            Thread.Sleep(mlikemanypost.time * 1000);

                            if (liked.Succeeded)
                            {
                                cantlike--;
                                count++;
                            }

                        }
                        else break;

                    }
                }
            }
            else
            {
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(Subcadena(mlikemanypost.userfollow));

                var getusuariofollowing = await insta.UserProcessor.GetUserAsync(mlikemanypost.userfollow);

                var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userfollow, PaginationParameters.MaxPagesToLoad(1));

                var getuserlike = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);

                var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));


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
                            Thread.Sleep(mlikemanypost.time * 1000);

                            if (liked.Succeeded)
                            {
                                cantlike--;
                                count++;
                            }

                        }
                        else break;

                    }
                }
            }

            return "Se le dio like a " + count + " post del usuario " + mlikemanypost.userlike;

        }

        private string Subcadena(string cadena)
        {
            if (cadena.Length < 2)
                return cadena;
            else
                return cadena.Substring(0, cadena.Length - 2);
        }
    }
}
