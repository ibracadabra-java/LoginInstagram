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
    /// Controlador para eliminar
    /// </summary>
    public class FollowProcessController : ApiController
    {
        Session session;
        /// <summary>
        /// constructor de la clase
        /// </summary>
        public FollowProcessController()
        {
            session = new Session();
        }

        /// <summary>
        /// Método para eliminar seguidor
        /// </summary>
        /// <param name="deletefollower"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DeleteFollower(mFollower deletefollower)
        {
            var userSession = new UserSessionData
            {
                UserName = deletefollower.user,
                Password = deletefollower.pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(deletefollower.userdel);
            var resuldel = await insta.UserProcessor.RemoveFollowerAsync(user.Value.Pk);

            return resuldel.Info.Message;
        }

        /// <summary>
        /// Metodo para bloquear usuario
        /// </summary>
        /// <param name="blokUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BlokUser(mFollower blokUsser)
        {
            var userSession = new UserSessionData
            {
                UserName = blokUsser.user,
                Password = blokUsser.pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(blokUsser.userdel);
            var resuldel = await insta.UserProcessor.BlockUserAsync(user.Value.Pk);

            return resuldel.Info.Message;
        }

        /// <summary>
        /// Metodo para dejar de seguir un usuario
        /// </summary>
        /// <param name="unfollowkUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UnFollowUser(mFollower unfollowkUsser)
        {
            var userSession = new UserSessionData
            {
                UserName = unfollowkUsser.user,
                Password = unfollowkUsser.pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(unfollowkUsser.userdel);
            var resuldel = await insta.UserProcessor.UnFollowUserAsync(user.Value.Pk);

            return resuldel.Info.Message;
        }


    }
}
