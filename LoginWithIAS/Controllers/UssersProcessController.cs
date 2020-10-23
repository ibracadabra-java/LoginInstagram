﻿using InstagramApiSharp;
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
using System.Threading.Tasks;
using System.Web.Http;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador para eliminar
    /// </summary>
    public class UssersProcessController : ApiController
    {
        Session session;
        /// <summary>
        /// constructor de la clase
        /// </summary>
        public UssersProcessController()
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
            var device = new AndroidDevice
            {

                AdId = deletefollower.AdId,
                AndroidBoardName = deletefollower.AndroidBoardName,
                AndroidBootloader = deletefollower.AndroidBootloader,
                AndroidVer = deletefollower.AndroidVer,
                DeviceBrand = deletefollower.DeviceBrand,
                DeviceGuid = new Guid(deletefollower.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(deletefollower.DeviceId.ToString())),
                DeviceModel = deletefollower.DeviceModel,
                DeviceModelBoot = deletefollower.DeviceModelBoot,
                DeviceModelIdentifier = deletefollower.DeviceModelIdentifier,
                Dpi = deletefollower.Dpi,
                Resolution = deletefollower.Resolution,
                FirmwareFingerprint = deletefollower.FirmwareFingerprint,
                FirmwareTags = deletefollower.FirmwareTags,
                FirmwareType = deletefollower.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = deletefollower.User,
                Password = deletefollower.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(deletefollower.otheruser);
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
            var device = new AndroidDevice
            {

                AdId = blokUsser.AdId,
                AndroidBoardName = blokUsser.AndroidBoardName,
                AndroidBootloader = blokUsser.AndroidBootloader,
                AndroidVer = blokUsser.AndroidVer,
                DeviceBrand = blokUsser.DeviceBrand,
                DeviceGuid = new Guid(blokUsser.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(blokUsser.DeviceId.ToString())),
                DeviceModel = blokUsser.DeviceModel,
                DeviceModelBoot = blokUsser.DeviceModelBoot,
                DeviceModelIdentifier = blokUsser.DeviceModelIdentifier,
                Dpi = blokUsser.Dpi,
                Resolution = blokUsser.Resolution,
                FirmwareFingerprint = blokUsser.FirmwareFingerprint,
                FirmwareTags = blokUsser.FirmwareTags,
                FirmwareType = blokUsser.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = blokUsser.User,
                Password = blokUsser.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(blokUsser.otheruser);
            var resul = await insta.UserProcessor.BlockUserAsync(user.Value.Pk);

            return resul.Info.Message;
        }

        /// <summary>
        /// Metodo para dejar de seguir un usuario
        /// </summary>
        /// <param name="unfollowkUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UnFollowUser(mFollower unfollowkUsser)
        {
            var device = new AndroidDevice
            {

                AdId = unfollowkUsser.AdId,
                AndroidBoardName = unfollowkUsser.AndroidBoardName,
                AndroidBootloader = unfollowkUsser.AndroidBootloader,
                AndroidVer = unfollowkUsser.AndroidVer,
                DeviceBrand = unfollowkUsser.DeviceBrand,
                DeviceGuid = new Guid(unfollowkUsser.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(unfollowkUsser.DeviceId.ToString())),
                DeviceModel = unfollowkUsser.DeviceModel,
                DeviceModelBoot = unfollowkUsser.DeviceModelBoot,
                DeviceModelIdentifier = unfollowkUsser.DeviceModelIdentifier,
                Dpi = unfollowkUsser.Dpi,
                Resolution = unfollowkUsser.Resolution,
                FirmwareFingerprint = unfollowkUsser.FirmwareFingerprint,
                FirmwareTags = unfollowkUsser.FirmwareTags,
                FirmwareType = unfollowkUsser.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = unfollowkUsser.User,
                Password = unfollowkUsser.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(unfollowkUsser.otheruser);
            var resul = await insta.UserProcessor.UnFollowUserAsync(user.Value.Pk);

            return resul.Info.Message;
        }

        /// <summary>
        /// Seguir un usuario
        /// </summary>
        /// <param name="followkUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> FollowUser(mFollower followkUsser)
        {
            var device = new AndroidDevice
            {

                AdId = followkUsser.AdId,
                AndroidBoardName = followkUsser.AndroidBoardName,
                AndroidBootloader = followkUsser.AndroidBootloader,
                AndroidVer = followkUsser.AndroidVer,
                DeviceBrand = followkUsser.DeviceBrand,
                DeviceGuid = new Guid(followkUsser.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followkUsser.DeviceId.ToString())),
                DeviceModel = followkUsser.DeviceModel,
                DeviceModelBoot = followkUsser.DeviceModelBoot,
                DeviceModelIdentifier = followkUsser.DeviceModelIdentifier,
                Dpi = followkUsser.Dpi,
                Resolution = followkUsser.Resolution,
                FirmwareFingerprint = followkUsser.FirmwareFingerprint,
                FirmwareTags = followkUsser.FirmwareTags,
                FirmwareType = followkUsser.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = followkUsser.User,
                Password = followkUsser.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserAsync(followkUsser.otheruser);
            var resul = await insta.UserProcessor.FollowUserAsync(user.Value.Pk);

            return resul.Info.Message;
        }

        /// <summary>
        /// Lista de posts
        /// </summary>
        /// <param name="postUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaMediaList> ListPostUser(mFollower postUsser)
        {
            var device = new AndroidDevice
            {

                AdId = postUsser.AdId,
                AndroidBoardName = postUsser.AndroidBoardName,
                AndroidBootloader = postUsser.AndroidBootloader,
                AndroidVer = postUsser.AndroidVer,
                DeviceBrand = postUsser.DeviceBrand,
                DeviceGuid = new Guid(postUsser.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(postUsser.DeviceId.ToString())),
                DeviceModel = postUsser.DeviceModel,
                DeviceModelBoot = postUsser.DeviceModelBoot,
                DeviceModelIdentifier = postUsser.DeviceModelIdentifier,
                Dpi = postUsser.Dpi,
                Resolution = postUsser.Resolution,
                FirmwareFingerprint = postUsser.FirmwareFingerprint,
                FirmwareTags = postUsser.FirmwareTags,
                FirmwareType = postUsser.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = postUsser.User,
                Password = postUsser.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            var user = await insta.UserProcessor.GetUserMediaAsync(postUsser.otheruser, PaginationParameters.MaxPagesToLoad(1));

            InstaMediaList listamedia = new InstaMediaList();
            if (user.Succeeded)
            {
                for (int i = 0; i < user.Value.Count; i++)
                {
                    listamedia.Add(user.Value[i]);
                }              
                
            }
            return listamedia;
        }

        /// <summary>
        /// Obtener Biografia
        /// </summary>
        /// <param name="biografiaUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BiografiaUsser(mFollower biografiaUsser)
        {
            var device = new AndroidDevice
            {

                AdId = biografiaUsser.AdId,
                AndroidBoardName = biografiaUsser.AndroidBoardName,
                AndroidBootloader = biografiaUsser.AndroidBootloader,
                AndroidVer = biografiaUsser.AndroidVer,
                DeviceBrand = biografiaUsser.DeviceBrand,
                DeviceGuid = new Guid(biografiaUsser.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(biografiaUsser.DeviceId.ToString())),
                DeviceModel = biografiaUsser.DeviceModel,
                DeviceModelBoot = biografiaUsser.DeviceModelBoot,
                DeviceModelIdentifier = biografiaUsser.DeviceModelIdentifier,
                Dpi = biografiaUsser.Dpi,
                Resolution = biografiaUsser.Resolution,
                FirmwareFingerprint = biografiaUsser.FirmwareFingerprint,
                FirmwareTags = biografiaUsser.FirmwareTags,
                FirmwareType = biografiaUsser.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = biografiaUsser.User,
                Password = biografiaUsser.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            InstaUserInfo info = new InstaUserInfo();

            var user = await insta.UserProcessor.GetUserAsync(biografiaUsser.otheruser);
            var biografia = await insta.UserProcessor.GetUserInfoByIdAsync(user.Value.Pk);

            return info.Biography;            
        }

        /// <summary>
        /// Cantidad de Seguidores
        /// </summary>
        /// <param name="followin"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> cantFollowinsUsser(mFollower followin)
        {

            var device = new AndroidDevice
            {

                AdId = followin.AdId,
                AndroidBoardName = followin.AndroidBoardName,
                AndroidBootloader = followin.AndroidBootloader,
                AndroidVer = followin.AndroidVer,
                DeviceBrand = followin.DeviceBrand,
                DeviceGuid = new Guid(followin.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followin.DeviceId.ToString())),
                DeviceModel = followin.DeviceModel,
                DeviceModelBoot = followin.DeviceModelBoot,
                DeviceModelIdentifier = followin.DeviceModelIdentifier,
                Dpi = followin.Dpi,
                Resolution = followin.Resolution,
                FirmwareFingerprint = followin.FirmwareFingerprint,
                FirmwareTags = followin.FirmwareTags,
                FirmwareType = followin.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = followin.User,
                Password = followin.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

           
            var user = await insta.UserProcessor.GetUserAsync(followin.User);
            
            var seguidores = await insta.UserProcessor.GetUserFollowingByIdAsync(user.Value.Pk,PaginationParameters.MaxPagesToLoad(1));

            return seguidores.Value.Count;
            
        }

        /// <summary>
        /// Cantidad de seguidos
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> cantFollowersUsser(mFollower followers)
        {

            var device = new AndroidDevice
            {

                AdId = followers.AdId,
                AndroidBoardName = followers.AndroidBoardName,
                AndroidBootloader = followers.AndroidBootloader,
                AndroidVer = followers.AndroidVer,
                DeviceBrand = followers.DeviceBrand,
                DeviceGuid = new Guid(followers.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followers.DeviceId.ToString())),
                DeviceModel = followers.DeviceModel,
                DeviceModelBoot = followers.DeviceModelBoot,
                DeviceModelIdentifier = followers.DeviceModelIdentifier,
                Dpi = followers.Dpi,
                Resolution = followers.Resolution,
                FirmwareFingerprint = followers.FirmwareFingerprint,
                FirmwareTags = followers.FirmwareTags,
                FirmwareType = followers.FirmwareType

            };
            var userSession = new UserSessionData
            {
                UserName = followers.User,
                Password = followers.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);


            var user = await insta.UserProcessor.GetUserAsync(followers.User);

            return user.Value.FollowersCount;

        }

        /// <summary>
        /// Lista de Usuarios Activos.
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaUsserActive(mFollower followers)
        {

            var device = new AndroidDevice
            {

                AdId = followers.AdId,
                AndroidBoardName = followers.AndroidBoardName,
                AndroidBootloader = followers.AndroidBootloader,
                AndroidVer = followers.AndroidVer,
                DeviceBrand = followers.DeviceBrand,
                DeviceGuid = new Guid(followers.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followers.DeviceId.ToString())),
                DeviceModel = followers.DeviceModel,
                DeviceModelBoot = followers.DeviceModelBoot,
                DeviceModelIdentifier = followers.DeviceModelIdentifier,
                Dpi = followers.Dpi,
                Resolution = followers.Resolution,
                FirmwareFingerprint = followers.FirmwareFingerprint,
                FirmwareTags = followers.FirmwareTags,
                FirmwareType = followers.FirmwareType

            };
            var userSession = new UserSessionData
            {
                UserName = followers.User,
                Password = followers.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);


            var user = await insta.UserProcessor.GetUserAsync(followers.User);

            var useractives = await insta.MessagingProcessor.GetUsersPresenceAsync();

            List<string> devolver = new List<string>();
            if (useractives.Succeeded)
            {
                for (int i = 0; i < useractives.Value.Count; i++)
                {
                    if (useractives.Value[i].IsActive)
                    {
                        var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(useractives.Value[i].Pk);
                        string username = userinfo.Value.UserDetail.Username;
                        devolver.Add(username);
                    }
                }
            }


            return devolver;

        }

        /// <summary>
        /// Devolver LIsta de seguidores
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaFollowersUsers(mFollower followers)
        {

            var device = new AndroidDevice
            {

                AdId = followers.AdId,
                AndroidBoardName = followers.AndroidBoardName,
                AndroidBootloader = followers.AndroidBootloader,
                AndroidVer = followers.AndroidVer,
                DeviceBrand = followers.DeviceBrand,
                DeviceGuid = new Guid(followers.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followers.DeviceId.ToString())),
                DeviceModel = followers.DeviceModel,
                DeviceModelBoot = followers.DeviceModelBoot,
                DeviceModelIdentifier = followers.DeviceModelIdentifier,
                Dpi = followers.Dpi,
                Resolution = followers.Resolution,
                FirmwareFingerprint = followers.FirmwareFingerprint,
                FirmwareTags = followers.FirmwareTags,
                FirmwareType = followers.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = followers.User,
                Password = followers.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);


            var userlist = await insta.UserProcessor.GetUserFollowersAsync(followers.User,PaginationParameters.MaxPagesToLoad(1));
            List<string> devolver = new List<string>();

            if (userlist.Succeeded)
            {
                for (int i = 0; i < userlist.Value.Count; i++)
                {
                    devolver.Add(userlist.Value[i].UserName);
                }
            }

            return devolver;

        }

        /// <summary>
        /// Devolver Lista de seguidos
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaFollowinsUsers(mFollower followers)
        {

            var device = new AndroidDevice
            {

                AdId = followers.AdId,
                AndroidBoardName = followers.AndroidBoardName,
                AndroidBootloader = followers.AndroidBootloader,
                AndroidVer = followers.AndroidVer,
                DeviceBrand = followers.DeviceBrand,
                DeviceGuid = new Guid(followers.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(followers.DeviceId.ToString())),
                DeviceModel = followers.DeviceModel,
                DeviceModelBoot = followers.DeviceModelBoot,
                DeviceModelIdentifier = followers.DeviceModelIdentifier,
                Dpi = followers.Dpi,
                Resolution = followers.Resolution,
                FirmwareFingerprint = followers.FirmwareFingerprint,
                FirmwareTags = followers.FirmwareTags,
                FirmwareType = followers.FirmwareType

            };
            var userSession = new UserSessionData
            {
                UserName = followers.User,
                Password = followers.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();
            insta.SetDevice(device);
            session.LoadSession(insta);


            var userlist = await insta.UserProcessor.GetUserFollowingAsync(followers.User, PaginationParameters.MaxPagesToLoad(1));
            List<string> devolver = new List<string>();

            if (userlist.Succeeded)
            {
                for (int i = 0; i < userlist.Value.Count; i++)
                {
                    devolver.Add(userlist.Value[i].UserName);
                }
            }

            return devolver;

        }

        /// <summary>
        /// Obtener nombre de usuario a traves de un id
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Getusername(mFollower users)
        {
            var device = new AndroidDevice
            {

                AdId = users.AdId,
                AndroidBoardName = users.AndroidBoardName,
                AndroidBootloader = users.AndroidBootloader,
                AndroidVer = users.AndroidVer,
                DeviceBrand = users.DeviceBrand,
                DeviceGuid = new Guid(users.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(users.DeviceId.ToString())),
                DeviceModel = users.DeviceModel,
                DeviceModelBoot = users.DeviceModelBoot,
                DeviceModelIdentifier = users.DeviceModelIdentifier,
                Dpi = users.Dpi,
                Resolution = users.Resolution,
                FirmwareFingerprint = users.FirmwareFingerprint,
                FirmwareTags = users.FirmwareTags,
                FirmwareType = users.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = users.User,
                Password = users.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);
            session.LoadSession(insta);

            var usuario = await insta.UserProcessor.GetUserInfoByIdAsync(users.pk_otheruser);
            string resultado = "";
            if (usuario.Succeeded)
                resultado = usuario.Value.Username;

            return resultado;
        }


    }
}
