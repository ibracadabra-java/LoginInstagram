using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using Microsoft.Ajax.Utilities;
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
        /// Metodo para enviar un mensaje de texto a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendDirectMessage(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.text))
                        {
                            var resul = await insta.MessagingProcessor.SendDirectTextAsync(user.Value.Pk.ToString(), String.Empty, chat.text);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                           token.Message = "Introdusca el texto a enviar";
                            return token;
                        }
                       
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }
                              
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }  
        }

        /// <summary>
        /// Metodo para enviar foto a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendPhoto(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.foto != null)
                        {
                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectPhotoToRecipientsAsync(chat.foto,recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir la foto a enviar";
                            return token;
                        }                       
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar video a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendVideo(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.video != null)
                        {
                            var videoupload = new InstaVideoUpload
                            {
                                Video = chat.video
                            };

                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectVideoToRecipientsAsync(videoupload, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el video a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar audio a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendAudio(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (chat.audio != null)
                        {
                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            var resul = await insta.MessagingProcessor.SendDirectVoiceToRecipientsAsync(chat.audio, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el audio a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para enviar Url a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendUrl(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.Url))
                        {
                            string[] recipient = new string[1];
                            string[] thread = new string[inboxThreads.Value.Inbox.Threads.Count];
                            recipient[0] = user.Value.Pk.ToString();

                            for (int i = 0; i < inboxThreads.Value.Inbox.Threads.Count; i++)
                            {
                                thread[i] = inboxThreads.Value.Inbox.Threads[i].ThreadId;
                            }

                            var resul = await insta.MessagingProcessor.SendDirectLinkAsync(chat.text,chat.Url,thread,recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el video a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }


        /// <summary>
        /// Metodo para enviar Objetos animados a un usuario
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> SendEmoji(mchat chat)
        {

            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(chat.User) || string.IsNullOrEmpty(chat.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = chat.User,
                        Password = chat.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(chat.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(chat.otheruser);
                if (user.Succeeded)
                {
                    var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                    if (!inboxThreads.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(chat.Giphyid))
                        {
                            
                            string[] thread = new string[1];

                            var resul = await insta.MessagingProcessor.SendDirectAnimatedMediaAsync(chat.Giphyid,thread);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = "Debe introducir el id del Emoticon a enviar a enviar";
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = inboxThreads.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = user.Info.Message;
                    return token;
                }

            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Enviar un hashtag
        /// </summary>
        /// <returns></returns>
        public async Task<enResponseToken> AgregarHashtag(mchat media)
        {
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
                    token.Message= "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(media.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;

                }

                if (!string.IsNullOrEmpty(media.Hashtag) && !string.IsNullOrEmpty(media.text) && !string.IsNullOrEmpty(media.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(media.otheruser);
                    if (user.Succeeded)
                    {
                        var inboxThreads = await insta.MessagingProcessor.GetDirectInboxAsync(InstagramApiSharp.PaginationParameters.MaxPagesToLoad(1));
                        if (!inboxThreads.Succeeded)
                        {

                            string[] recipient = new string[1];
                            recipient[0] = user.Value.Pk.ToString();

                            string[] threads = new string[inboxThreads.Value.Inbox.Threads.Count];
                            for (int i = 0; i < inboxThreads.Value.Inbox.Threads.Count; i++)
                            {
                                threads[i] = inboxThreads.Value.Inbox.Threads[i].ThreadId;

                            }

                            var resul = await insta.MessagingProcessor.SendDirectHashtagAsync(media.text, media.Hashtag,threads, recipient);
                            if (resul.Succeeded)
                            {
                                token.Message = resul.Info.Message;
                                token.AuthToken = session.GenerarToken();
                                return token;
                            }
                            else
                            {
                                token.Message = resul.Info.Message;
                                return token;
                            }
                        }
                        else
                        {
                            token.Message = inboxThreads.Info.Message;
                            return token;
                        }
                    }
                    else
                    {
                        token.Message = user.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = "Tiene que introducir el texto, el hashtag y el usuario al que le desea enviar el hashtag";
                    return token;
                }
                
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }



    }
}
