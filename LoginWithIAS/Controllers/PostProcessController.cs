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

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador para dar likes
    /// </summary>
    public class PostProcessController : ApiController
    {
        Session session;

        /// <summary>
        /// constructor de la clase
        /// </summary>
        public PostProcessController()
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

            var device = new AndroidDevice
            {

                AdId = mlikemanypost.AdId,
                AndroidBoardName = mlikemanypost.AndroidBoardName,
                AndroidBootloader = mlikemanypost.AndroidBootloader,
                AndroidVer = mlikemanypost.AndroidVer,
                DeviceBrand = mlikemanypost.DeviceBrand,
                DeviceGuid = new Guid(mlikemanypost.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(mlikemanypost.DeviceId.ToString())),
                DeviceModel = mlikemanypost.DeviceModel,
                DeviceModelBoot = mlikemanypost.DeviceModelBoot,
                DeviceModelIdentifier = mlikemanypost.DeviceModelIdentifier,
                Dpi = mlikemanypost.Dpi,
                Resolution = mlikemanypost.Resolution,
                FirmwareFingerprint = mlikemanypost.FirmwareFingerprint,
                FirmwareTags = mlikemanypost.FirmwareTags,
                FirmwareType = mlikemanypost.FirmwareType

            };
            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };

            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

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
        /// 
        /// </summary>
        /// <param name="mlikemanypost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SimulationLikeManyPost(mLikeManyPost mlikemanypost)
        {

            int cantlike = mlikemanypost.cantLike;
            int count = 0;

           /* var proxy = new WebProxy()
            {
                Address = new Uri($"" + mlikemanypost.AddressProxy + ""),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    userName: mlikemanypost.UsernameProxy,
                    password: mlikemanypost.PassProxy
                    )

            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };*/

            /*  var device = new AndroidDevice
              {

                  AdId = mlikemanypost.AdId,
                  AndroidBoardName = mlikemanypost.AndroidBoardName,
                  AndroidBootloader = mlikemanypost.AndroidBootloader,
                  AndroidVer = mlikemanypost.AndroidVer,
                  DeviceBrand = mlikemanypost.DeviceBrand,
                  DeviceGuid = new Guid(mlikemanypost.DeviceGuid.ToString()),
                  DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(mlikemanypost.DeviceId.ToString())),
                  DeviceModel = mlikemanypost.DeviceModel,
                  DeviceModelBoot = mlikemanypost.DeviceModelBoot,
                  DeviceModelIdentifier = mlikemanypost.DeviceModelIdentifier,
                  Dpi = mlikemanypost.Dpi,
                  Resolution = mlikemanypost.Resolution,
                  FirmwareFingerprint = mlikemanypost.FirmwareFingerprint,
                  FirmwareTags = mlikemanypost.FirmwareTags,
                  FirmwareType = mlikemanypost.FirmwareType

              };*/

            var userSession = new UserSessionData
            {
                UserName = mlikemanypost.User,
                Password = mlikemanypost.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All))/*.UseHttpClientHandler(httpClientHandler)*/.Build();

          //  insta.SetDevice(device);

            session.LoadSession(insta);

            if (mlikemanypost.userfollow == null)
            {
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(Subcadena(mlikemanypost.userlike),PaginationParameters.MaxPagesToLoad(1));

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
                var instauser = await insta.DiscoverProcessor.SearchPeopleAsync(Subcadena(mlikemanypost.userfollow),PaginationParameters.MaxPagesToLoad(1));

                var getusuariofollowing = await insta.UserProcessor.GetUserAsync(mlikemanypost.userfollow);

                var seguidores = await insta.UserProcessor.GetUserFollowersAsync(mlikemanypost.userfollow, PaginationParameters.MaxPagesToLoad(1));

                var getuserlike = await insta.UserProcessor.GetUserAsync(mlikemanypost.userlike);

                var media = await insta.UserProcessor.GetUserMediaAsync(mlikemanypost.userlike, PaginationParameters.MaxPagesToLoad(1));


                if (media.Succeeded)
                {
                    List<InstaMedia> milista = new List<InstaMedia>();
                    Random valorandon = new Random();
                    for (int i = 0; i < media.Value.Count; i+= valorandon.Next(1,3))
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

        /// <summary>
        /// Obtener una subcadena
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        private string Subcadena(string cadena)
        {
            if (cadena.Length < 2)
                return cadena;
            else
                return cadena.Substring(0, cadena.Length - 2);
        }

        /// <summary>
        /// Obtener un post
        /// </summary>
        /// <param name="userpost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaMedia> extraerPost(mPost userpost)
        {
            var device = new AndroidDevice
            {

                AdId = userpost.AdId,
                AndroidBoardName = userpost.AndroidBoardName,
                AndroidBootloader = userpost.AndroidBootloader,
                AndroidVer = userpost.AndroidVer,
                DeviceBrand = userpost.DeviceBrand,
                DeviceGuid = new Guid(userpost.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(userpost.DeviceId.ToString())),
                DeviceModel = userpost.DeviceModel,
                DeviceModelBoot = userpost.DeviceModelBoot,
                DeviceModelIdentifier = userpost.DeviceModelIdentifier,
                Dpi = userpost.Dpi,
                Resolution = userpost.Resolution,
                FirmwareFingerprint = userpost.FirmwareFingerprint,
                FirmwareTags = userpost.FirmwareTags,
                FirmwareType = userpost.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = userpost.User,
                Password = userpost.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var post = await insta.MediaProcessor.GetMediaByIdAsync(userpost.idpost);

            InstaMedia obj = new InstaMedia();
            if (post.Succeeded)
                obj = post.Value;

            return obj;

        }

        /// <summary>
        /// Eliminar un post
        /// </summary>
        /// <param name="userpost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> deletePost(mPost userpost)
        {
            var device = new AndroidDevice
            {

                AdId = userpost.AdId,
                AndroidBoardName = userpost.AndroidBoardName,
                AndroidBootloader = userpost.AndroidBootloader,
                AndroidVer = userpost.AndroidVer,
                DeviceBrand = userpost.DeviceBrand,
                DeviceGuid = new Guid(userpost.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(userpost.DeviceId.ToString())),
                DeviceModel = userpost.DeviceModel,
                DeviceModelBoot = userpost.DeviceModelBoot,
                DeviceModelIdentifier = userpost.DeviceModelIdentifier,
                Dpi = userpost.Dpi,
                Resolution = userpost.Resolution,
                FirmwareFingerprint = userpost.FirmwareFingerprint,
                FirmwareTags = userpost.FirmwareTags,
                FirmwareType = userpost.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = userpost.User,
                Password = userpost.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);
            bool eliminarpost=false;            
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

        /// <summary>
        /// Extraer un Caption
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaCaption> ExtraerCaption(mPost post) 
        {
            InstaCaption caption = new InstaCaption();

            var device = new AndroidDevice
            {

                AdId = post.AdId,
                AndroidBoardName = post.AndroidBoardName,
                AndroidBootloader = post.AndroidBootloader,
                AndroidVer = post.AndroidVer,
                DeviceBrand = post.DeviceBrand,
                DeviceGuid = new Guid(post.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(post.DeviceId.ToString())),
                DeviceModel = post.DeviceModel,
                DeviceModelBoot = post.DeviceModelBoot,
                DeviceModelIdentifier = post.DeviceModelIdentifier,
                Dpi = post.Dpi,
                Resolution = post.Resolution,
                FirmwareFingerprint = post.FirmwareFingerprint,
                FirmwareTags = post.FirmwareTags,
                FirmwareType = post.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = post.User,
                Password = post.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var media = await insta.MediaProcessor.GetMediaByIdAsync(post.idpost);
            if (media.Succeeded)
            {
                 caption = media.Value.Caption;
            }
            return caption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaCaption> EditarCaption(mcaption caption)
        {
            InstaCaption instacaption = new InstaCaption();

            var device = new AndroidDevice
            {

                AdId = caption.AdId,
                AndroidBoardName = caption.AndroidBoardName,
                AndroidBootloader = caption.AndroidBootloader,
                AndroidVer = caption.AndroidVer,
                DeviceBrand = caption.DeviceBrand,
                DeviceGuid = new Guid(caption.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(caption.DeviceId.ToString())),
                DeviceModel = caption.DeviceModel,
                DeviceModelBoot = caption.DeviceModelBoot,
                DeviceModelIdentifier = caption.DeviceModelIdentifier,
                Dpi = caption.Dpi,
                Resolution = caption.Resolution,
                FirmwareFingerprint = caption.FirmwareFingerprint,
                FirmwareTags = caption.FirmwareTags,
                FirmwareType = caption.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = caption.User,
                Password = caption.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var media = await insta.MediaProcessor.GetMediaByIdAsync(caption.Idmedia);
            if (media.Succeeded)
            {
                instacaption = media.Value.Caption;
                instacaption.Text = caption.texto;
            }
            return instacaption;
        }

        /// <summary>
        /// Devuelbe la lista de Likers
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<InstaUserShort>> ListaLikers(mPost media)
        {
            List<InstaUserShort> finallikers = new List<InstaUserShort>();

            var device = new AndroidDevice
            {

                AdId = media.AdId,
                AndroidBoardName = media.AndroidBoardName,
                AndroidBootloader = media.AndroidBootloader,
                AndroidVer = media.AndroidVer,
                DeviceBrand = media.DeviceBrand,
                DeviceGuid = new Guid(media.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(media.DeviceId.ToString())),
                DeviceModel = media.DeviceModel,
                DeviceModelBoot = media.DeviceModelBoot,
                DeviceModelIdentifier = media.DeviceModelIdentifier,
                Dpi = media.Dpi,
                Resolution = media.Resolution,
                FirmwareFingerprint = media.FirmwareFingerprint,
                FirmwareTags = media.FirmwareTags,
                FirmwareType = media.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = media.User,
                Password = media.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var objmedia = await insta.MediaProcessor.GetMediaByIdAsync(media.idpost);
            if (objmedia.Succeeded)
            {
                finallikers = objmedia.Value.Likers;
            }
            return finallikers;
        }

        /// <summary>
        /// Like aun post x del feed
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> LikeaPostFeed(mPost media) 
        {
            var device = new AndroidDevice
            {

                AdId = media.AdId,
                AndroidBoardName = media.AndroidBoardName,
                AndroidBootloader = media.AndroidBootloader,
                AndroidVer = media.AndroidVer,
                DeviceBrand = media.DeviceBrand,
                DeviceGuid = new Guid(media.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(media.DeviceId.ToString())),
                DeviceModel = media.DeviceModel,
                DeviceModelBoot = media.DeviceModelBoot,
                DeviceModelIdentifier = media.DeviceModelIdentifier,
                Dpi = media.Dpi,
                Resolution = media.Resolution,
                FirmwareFingerprint = media.FirmwareFingerprint,
                FirmwareTags = media.FirmwareTags,
                FirmwareType = media.FirmwareType

            };

            var userSession = new UserSessionData
            {
                UserName = media.User,
                Password = media.Pass
            };
            var insta = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).Build();

            insta.SetDevice(device);

            session.LoadSession(insta);

            var feed =await insta.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

            if (feed.Succeeded) 
            {
                var medias = feed.Value.Medias;
                for (int i = 0; i < medias.Count; i++)
                {
                    if (medias[i].Pk == media.idpost && !medias[i].HasLiked)
                    {
                        var like = await insta.MediaProcessor.LikeMediaAsync(medias[i].Pk);
                        if (like.Succeeded)
                            return "Se le ha dado like al post: "+medias[i].InstaIdentifier;
                    }
                }

            }

            return "No se encontró el post, rectifique el id.";
        }

    }
}
