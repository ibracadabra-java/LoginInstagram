using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
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
    /// History Controler
    /// </summary>
    public class StoryProcessController : ApiController
    {
        Sesion session;
        /// <summary>
        /// Constructor del Controlador History
        /// </summary>
        public StoryProcessController()
        {
            session = new Sesion();
        }

        /// <summary>
        /// Compartir una Historia
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> ShareStory(mStory credenciales)
        {
            try
            {
                int cantidad = credenciales.cantidad;
                enResponseToken token = new enResponseToken();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(credenciales.User) || string.IsNullOrEmpty(credenciales.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = credenciales.User,
                        Password = credenciales.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(credenciales.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(credenciales.Other_user))
                {
                    var user = await insta.UserProcessor.GetUserAsync(credenciales.Other_user);                    
                    if (user.Succeeded)
                    {
                        long[] recipient = new long[1];
                        recipient[0] = user.Value.Pk;
                        var story = await insta.StoryProcessor.GetUserStoryFeedAsync(user.Value.Pk);
                        
                        if (story.Succeeded)
                        {
                            Random next = new Random();
                            
                            for (int i = 0; i < story.Value.Items.Count; i+= next.Next(1, 2))
                            {
                                if (cantidad > 0)
                                {
                                    var share = await insta.StoryProcessor.ShareStoryAsync(story.Value.ReelType,story.Value.Items[i].Id,null,recipient,credenciales.Text_share);
                                    cantidad--;
                                }
                            }
                        }
                        else
                        {
                            token.Message = "No existe el usuario del que quiere obtener sus historias";
                        }
                    }
                    else
                    {
                        token.Message = "No tiene sesiones de Login Activas o el usuario del que quiere obtener sus historias no existe";
                        return token;
                    }
                    return token;
                }
                else
                {
                    token.Message = "Debe introducir el usuario del que quiere obtener sus historias";
                    return token;
                }
               
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Ver historias de un seguidor
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<InstaStoryItem>> VerStory(mStory credenciales)
        {
            try
            {
                int cantidad = credenciales.cantidad;
                List<InstaStoryItem> listado_historias = new List<InstaStoryItem>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(credenciales.User) || string.IsNullOrEmpty(credenciales.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = credenciales.User,
                        Password = credenciales.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(credenciales.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(credenciales.Other_user))
                {
                    var user = await insta.UserProcessor.GetUserAsync(credenciales.Other_user);
                    //var user_autenticated = await insta.UserProcessor.GetUserAsync(credenciales.User);
                    if (user.Succeeded)
                    {
                        long[] recipient = new long[1];
                        recipient[0] = user.Value.Pk;
                        var story = await insta.StoryProcessor.GetUserStoryAsync(user.Value.Pk);

                        if (story.Succeeded)
                        {
                            
                            Random next = new Random();

                            for (int i = 0; i < story.Value.Items.Count; i += next.Next(1, 2))
                            {
                                if (!story.Value.Items[i].HasLiked)
                                {
                                    listado_historias.Add(story.Value.Items[i]);
                                    story.Value.Items[i].HasLiked = true;

                                }
                            }
                            return listado_historias;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<InstaStoryItem>> VerunaStory(mStory credenciales)
        {
            try
            {
                int cantidad = credenciales.cantidad;
                List<InstaStoryItem> listado_historias = new List<InstaStoryItem>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(credenciales.User) || string.IsNullOrEmpty(credenciales.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = credenciales.User,
                        Password = credenciales.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(credenciales.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(credenciales.Other_user))
                {
                    var user = await insta.UserProcessor.GetUserAsync(credenciales.Other_user);
                    //var user_autenticated = await insta.UserProcessor.GetUserAsync(credenciales.User);
                    if (user.Succeeded)
                    {
                        long[] recipient = new long[1];
                        recipient[0] = user.Value.Pk;
                        var story = await insta.StoryProcessor.GetUserStoryAsync(user.Value.Pk);

                        if (story.Succeeded)
                        {

                            Random next = new Random();

                            for (int i = 0; i < 1; i ++)
                            {
                                if (!story.Value.Items[i].HasLiked)
                                {
                                    listado_historias.Add(story.Value.Items[i]);
                                    story.Value.Items[i].HasLiked = true;

                                }
                            }
                            return listado_historias;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }


    }
}
