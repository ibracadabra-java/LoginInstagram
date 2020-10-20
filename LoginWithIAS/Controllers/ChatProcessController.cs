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
    /// 
    /// </summary>
    public class ChatProcessController : ApiController
    {
        Session session;

        /// <summary>
        /// 
        /// </summary>
        public ChatProcessController()
        {
            session = new Session();
        }

        /// <summary>
        /// Metodo para dejar de seguir un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SendDirectMessage(mchat chat)
        {
            var userSession = new UserSessionData
            {
                UserName = chat.userauthenticated,
                Password = chat.passworduserauthenticated
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);

            var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
            if (!inboxThreads.Succeeded)
            {
                return inboxThreads.Info.Message;
            }
            var resul = await insta.MessagingProcessor.SendDirectTextAsync(user.Value.Pk.ToString(),String.Empty,chat.text);

            return resul.Info.Message;            
        }
    }
}
