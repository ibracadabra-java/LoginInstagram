using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
