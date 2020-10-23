using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
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
    public class MessageProcessController : ApiController
    {
        Session session;

        /// <summary>
        /// 
        /// </summary>
        public MessageProcessController()
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
            var device = new AndroidDevice
            {

                AdId = chat.AdId,
                AndroidBoardName = chat.AndroidBoardName,
                AndroidBootloader = chat.AndroidBootloader,
                AndroidVer = chat.AndroidVer,
                DeviceBrand = chat.DeviceBrand,
                DeviceGuid = new Guid(chat.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(chat.DeviceId.ToString())),
                DeviceModel = chat.DeviceModel,
                DeviceModelBoot = chat.DeviceModelBoot,
                DeviceModelIdentifier = chat.DeviceModelIdentifier,
                Dpi = chat.Dpi,
                Resolution = chat.Resolution,
                FirmwareFingerprint = chat.FirmwareFingerprint,
                FirmwareTags = chat.FirmwareTags,
                FirmwareType = chat.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = chat.User,
                Password = chat.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            insta.SetDevice(device);
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

        /*[HttpPost]
        public async Task<string> SendDirectMessageFromSearch(mchat chat) 
        {
            return "";
        }*/

    }
}
