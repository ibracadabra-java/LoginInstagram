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
        public async Task<enResponseToken> DeleteFollower(mFollower deletefollower)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(deletefollower.User) || string.IsNullOrEmpty(deletefollower.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = deletefollower.User,
                        Password = deletefollower.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(deletefollower.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(deletefollower.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(deletefollower.otheruser);

                    if (user.Succeeded)
                    {
                        var resuldel = await insta.UserProcessor.RemoveFollowerAsync(user.Value.Pk);
                        if (resuldel.Succeeded)
                        {
                            token.Message = resuldel.Info.Message;
                            token.AuthToken = session.GenerarToken();
                            return token;
                        }
                        else
                        {
                            token.Message = resuldel.Info.Message;
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
                    token.Message = "Debe Introducir el nombre del usuario a eliminar";
                    return token;
                }
                
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para bloquear usuario
        /// </summary>
        /// <param name="blokUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> BlokUser(mFollower blokUsser)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(blokUsser.User) || string.IsNullOrEmpty(blokUsser.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = blokUsser.User,
                        Password = blokUsser.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(blokUsser.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(blokUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(blokUsser.otheruser);

                    if (user.Succeeded)
                    {
                        var resul = await insta.UserProcessor.BlockUserAsync(user.Value.Pk);
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
                        token.Message = user.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = "Debe introducir el usuario a bloquear";
                    return token;
                }
                
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Metodo para dejar de seguir un usuario
        /// </summary>
        /// <param name="unfollowkUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> UnFollowUser(mFollower unfollowkUsser)
        {
            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(unfollowkUsser.User) || string.IsNullOrEmpty(unfollowkUsser.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = unfollowkUsser.User,
                        Password = unfollowkUsser.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(unfollowkUsser.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(unfollowkUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(unfollowkUsser.otheruser);
                    if (user.Succeeded)
                    {
                        var resul = await insta.UserProcessor.UnFollowUserAsync(user.Value.Pk);
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
                        token.Message = user.Info.Message;
                        return token;
                    }

                }
                else
                {
                    token.Message = "Debe Introducir el Otro Usuario";
                    return token;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }        
            
        }

        /// <summary>
        /// Seguir un usuario
        /// </summary>
        /// <param name="followkUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> FollowUser(mFollower followkUsser)
        {
            try
            {
                enResponseToken token = new enResponseToken();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followkUsser.User) || string.IsNullOrEmpty(followkUsser.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followkUsser.User,
                        Password = followkUsser.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followkUsser.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                if (!string.IsNullOrEmpty(followkUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followkUsser.otheruser);
                    if (user.Succeeded)
                    {
                        var resul = await insta.UserProcessor.FollowUserAsync(user.Value.Pk);
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
                        token.Message = user.Info.Message;
                        return token;
                    }
                }
                else
                {
                    token.Message = "Debe introducir el usuario a seguir";
                    return token;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
            
        }

        /// <summary>
        /// Lista de posts de un usuario
        /// </summary>
        /// <param name="postUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaMediaList> ListPostUser(mFollower postUsser)
        {
            try
            {
                InstaMediaList listamedia = new InstaMediaList();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(postUsser.User) || string.IsNullOrEmpty(postUsser.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = postUsser.User,
                        Password = postUsser.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(postUsser.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(postUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserMediaAsync(postUsser.otheruser, PaginationParameters.MaxPagesToLoad(1));
                    if (user.Succeeded)
                    {
                        for (int i = 0; i < user.Value.Count; i++)
                        {
                            listamedia.Add(user.Value[i]);
                        }
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }

                return listamedia;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Obtener Biografia
        /// </summary>
        /// <param name="biografiaUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaUserInfo> BiografiaUsser(mFollower biografiaUsser)
        {

            try
            {
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(biografiaUsser.User) || string.IsNullOrEmpty(biografiaUsser.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = biografiaUsser.User,
                        Password = biografiaUsser.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(biografiaUsser.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(biografiaUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(biografiaUsser.otheruser);
                    if (user.Succeeded)
                    {
                        var biografia = await insta.UserProcessor.GetUserInfoByIdAsync(user.Value.Pk);

                        if (biografia.Succeeded)
                        {
                            return biografia.Value;
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
        /// Cantidad de Seguidores
        /// </summary>
        /// <param name="followin"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> cantFollowinsUsser(mFollower followin)
        {
            try
            {
                int cantidad = 0;
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followin.User) || string.IsNullOrEmpty(followin.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followin.User,
                        Password = followin.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return cantidad;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followin.Pass))
                {
                    return cantidad;
                }

                if (!string.IsNullOrEmpty(followin.User))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followin.User);

                    if (user.Succeeded)
                    {
                        var seguidores = await insta.UserProcessor.GetUserFollowingByIdAsync(user.Value.Pk, PaginationParameters.MaxPagesToLoad(1));

                        if (seguidores.Succeeded)
                        {
                            return seguidores.Value.Count;
                        }
                        else
                        {
                            return cantidad;
                        }
                    }
                    else
                    {
                        return cantidad;
                    }
                }
                else
                {
                    return cantidad;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Cantidad de seguidos
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> cantFollowersUsser(mFollower followers)
        {
            try
            {
                int cantidad = 0;
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followers.User) || string.IsNullOrEmpty(followers.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followers.User,
                        Password = followers.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return cantidad;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followers.Pass))
                {
                    return cantidad;
                }

                if (!string.IsNullOrEmpty(followers.User))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followers.User);
                    if (user.Succeeded)
                    {
                        return user.Value.FollowersCount;
                    }
                    else
                    {
                        return cantidad;
                    }
                }
                else
                {
                    return cantidad;
                }
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Lista de Usuarios Activos.
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaUsserActive(mFollower followers)
        {
            try
            {
                List<string> devolver = new List<string>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followers.User) || string.IsNullOrEmpty(followers.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followers.User,
                        Password = followers.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followers.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(followers.User))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followers.User);

                    if (user.Succeeded)
                    {
                        var useractives = await insta.MessagingProcessor.GetUsersPresenceAsync();
                        if (useractives.Succeeded)
                        {
                            for (int i = 0; i < useractives.Value.Count; i++)
                            {
                                if (useractives.Value[i].IsActive)
                                {
                                    var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(useractives.Value[i].Pk);
                                    string username = userinfo.Value.UserDetail.UserName;
                                    devolver.Add(username);
                                }
                            }
                        }
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return null;

                return devolver;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Devolver LIsta de seguidores
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaFollowersUsers(mFollower followers)
        {
            try
            {
                List<string> devolver = new List<string>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followers.User) || string.IsNullOrEmpty(followers.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followers.User,
                        Password = followers.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followers.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(followers.User))
                {
                    var userlist = await insta.UserProcessor.GetUserFollowersAsync(followers.User, PaginationParameters.MaxPagesToLoad(1));
                    if (userlist.Succeeded)
                    {
                        if (userlist.Succeeded)
                        {
                            for (int i = 0; i < userlist.Value.Count; i++)
                            {
                                devolver.Add(userlist.Value[i].UserName);
                            }
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
                return devolver;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Devolver Lista de seguidos
        /// </summary>
        /// <param name="followins"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> ListaFollowinsUsers(mFollower followins)
        {
            try
            {
                List<string> devolver = new List<string>();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(followins.User) || string.IsNullOrEmpty(followins.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = followins.User,
                        Password = followins.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followins.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(followins.User))
                {
                    var userlist = await insta.UserProcessor.GetUserFollowingAsync(followins.User, PaginationParameters.MaxPagesToLoad(1));

                    if (userlist.Succeeded)
                    {
                        for (int i = 0; i < userlist.Value.Count; i++)
                        {
                            devolver.Add(userlist.Value[i].UserName);
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
                return devolver;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Obtener nombre de usuario a traves de un id
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Getusername(mFollower users)
        {
            try
            {
                string resultado = "";
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(users.User) || string.IsNullOrEmpty(users.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = users.User,
                        Password = users.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return "Debe introducir usuario y contraseñ";
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(users.Pass))
                {
                    return "Contraseña incorrecta";
                }

                if (users.pk_otheruser > 0)
                {
                    var usuario = await insta.UserProcessor.GetUserInfoByIdAsync(users.pk_otheruser);
                    if (usuario.Succeeded)
                    {
                        if (usuario.Succeeded)
                            resultado = usuario.Value.UserName;
                    }
                    else
                        return "El identificador de usuario que introdujo no existe";
                }
                return resultado;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
    }
}
