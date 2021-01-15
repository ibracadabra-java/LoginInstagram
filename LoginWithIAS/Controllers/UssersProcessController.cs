using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes.Models.Business;
using InstagramApiSharp.Enums;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LoginWithIAS.Utiles;
using System.Web;
using System.Web.Http.Results;
using System.Threading;
using LoginWithIAS.ApiBd;
using LoginWithIAS.App_Start;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador para eliminar
    /// </summary>
    public class UssersProcessController : ApiController
    {
        ActComBd actcom;
        Sesion session;
        Log log;
        Log logacti;
        ProxyBD prbd;
        Util util;
        MloginBD bd;
        ProxyBD bdprox;
        string path = HttpContext.Current.Request.MapPath("~/Logs");

        /// <summary>
        /// constructor de la clase
        /// </summary>
        public UssersProcessController()
        {
            session = new Sesion();
            log = new Log(path);
            actcom = new ActComBd();
            util = new Util();
            prbd = new ProxyBD();
            bd = new MloginBD();
            bdprox = new ProxyBD();
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
        public async Task<string> UnFollowUser(mFollower unfollowkUsser)
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
                    return "Deben introducir Usuario y Contraseña";
                }

                session.LoadSession(insta);

                var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

                if (!pass["Pass"].Equals(unfollowkUsser.Pass))
                {
                    log.Add("Contraseña incorrecta");
                    return "Contraseña incorrecta";
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
                            return resul.Info.Message;
                        }
                        else
                        {
                            token.Message = resul.Info.Message;
                            return resul.Info.Message;
                        }
                    }
                    else
                    {
                        token.Message = user.Info.Message;
                        return user.Info.Message;
                    }

                }
                else
                {
                    token.Message = "Debe Introducir el Otro Usuario";
                    return "Debe Introducir el Otro Usuario";
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
        /// devuelve la cantidad de post de un usuario
        /// </summary>
        /// <param name="postUsser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> cantPostUser(mFollower postUsser)
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
                    return -1;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(postUsser.Pass))
                {
                    return -1;
                }

                if (!string.IsNullOrEmpty(postUsser.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserAsync(postUsser.otheruser);
                    
                    if (user.Succeeded)
                    {
                        var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                        if (userinfo.Succeeded) 
                        {
                            var cantPost = userinfo.Value.UserDetail.MediaCount;
                            return cantPost;
                        }
                        else 
                        {
                            return -1;
                        }
                    }
                    else
                        return -1;
                }
                else
                {
                    return -1;
                }
               
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
        public async Task<long> cantFollowinsUsser(mFollower followin)
        {
            try
            {
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
                    return -1;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followin.Pass))
                {
                    return -1;
                }

                if (!string.IsNullOrEmpty(followin.User))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followin.otheruser);
                    if (user.Succeeded)
                    {
                        var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);

                        if (userinfo.Succeeded)
                        {
                            var seguidores = userinfo.Value.UserDetail.FollowingCount;

                            return seguidores;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else 
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
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
        public async Task<long> cantFollowersUsser(mFollower followers)
        {
            try
            {
                
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
                    return -1;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(followers.Pass))
                {
                    return -1;
                }

                if (!string.IsNullOrEmpty(followers.User))
                {
                    var user = await insta.UserProcessor.GetUserAsync(followers.otheruser);
                    if (user.Succeeded)
                    {
                        var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                        if (userinfo.Succeeded) 
                        {
                            var seguidos = userinfo.Value.UserDetail.FollowerCount;
                            return seguidos;
                        }
                        else 
                        {
                            return -1;
                        }
                       
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
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
            string pathact  = HttpContext.Current.Request.MapPath("~/Logs/"+followers.User);
            logacti = new Log(pathact);
            DateTime now = DateTime.Now;
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
                        while ((DateTime.Now - now).TotalHours < 24)
                        {
                            var useractives = await insta.MessagingProcessor.GetUsersPresenceAsync();
                            if (useractives.Succeeded)
                            {
                                devolver.Clear();
                                if (useractives.Value.Count > 0)
                                {
                                    for (int i = 0; i < useractives.Value.Count; i++)
                                    {
                                        if (useractives.Value[i].IsActive)
                                        {
                                            var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(useractives.Value[i].Pk);
                                            string username = userinfo.Value.UserDetail.UserName + " horario " + DateTime.Now.ToString();
                                            logacti.Add(" usuarios activos de " + followers.User + " " + username);
                                            devolver.Add(username);

                                        }

                                    }
                                }
                                if (devolver.Count == 0)
                                    logacti.Add(" Ningun usuario activo de " + followers.User);
                                logacti.Add(" Esperando 1 minuto para volver a comprobar");
                                Thread.Sleep(60 * 1000);
                            }
                            else
                                return null;
                        }
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
            int count = 1;
            PaginationParameters pagination  = PaginationParameters.MaxPagesToLoad(1) ;           
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
                   
                    var user = await insta.UserProcessor.GetUserAsync(followers.otheruser);
                    var userInfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                    var cantFoller = userInfo.Value.UserDetail.FollowerCount;
                    var lengt = cantFoller / 100;
                    for (int i = 0; i < lengt; i++)
                    {
                        if (devolver.Count >= 5000 * count)
                        {
                            count++;
                            Thread.Sleep(1200000);
                        }

                        var userlist = await insta.UserProcessor.GetUserFollowersAsync(followers.otheruser, pagination, "", false, "2");
                        log.Add(" Scrapeando usuarios de " + followers.User + "cantidad actual" + devolver.Count);
                        if (userlist.Succeeded)
                        {
                         
                           for (int j = 0; j < userlist.Value.Count; j++)
                                {
                                    devolver.Add(userlist.Value[j].UserName);
                                }
                            
                        }
                        else
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

        /// <summary>
        /// Devolver LIsta de seguidores menores de 24 horas, los que me siguen en menos de 1 dia
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> UltimosFollowers(mFollower followers)
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
                    var userlist = await insta.UserProcessor.GetRecentFollowersAsync();
                    if (userlist.Succeeded)
                    {
                        if (userlist.Succeeded)
                        {
                            for (int i = 0; i < userlist.Value.Users.Count; i++)
                            {
                                devolver.Add(userlist.Value.Users[i].UserName);
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
        /// Extraer estadisticas del Cliente(Cuenta de Negocios)
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<IResult<InstaFullMediaInsights>>> Estadistica(mFollower cliente)
        {
            try
            {
                List<IResult<InstaFullMediaInsights>> devolver = new List<IResult<InstaFullMediaInsights>>();

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(cliente.User) || string.IsNullOrEmpty(cliente.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = cliente.User,
                        Password = cliente.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    return null;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(cliente.Pass))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(cliente.otheruser))
                {
                    var user = await insta.UserProcessor.GetUserInfoByUsernameAsync(cliente.otheruser);
                    
                    if (user.Succeeded)
                    {
                        if (user.Value.IsBusiness && user.Value.AccountType == InstaAccountType.Business)
                        {
                            var media = await insta.UserProcessor.GetUserMediaAsync(user.Value.UserName,PaginationParameters.MaxPagesToLoad(1));

                            if (media.Succeeded)
                            {
                                for (int i = 0; i < media.Value.Count; i++)
                                {
                                    var insight = await insta.BusinessProcessor.GetFullMediaInsightsAsync(media.Value[i].InstaIdentifier);
                                    if (insight.Succeeded)
                                    {
                                        devolver.Add(insight);
                                    }
                                }
                            }
                        }
                    }
                }
                return devolver;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mFollow"></param>
        [HttpPost]
        public void VisitPerfil(mFollower mFollow)
        {
            try
            {
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(mFollow.User) || string.IsNullOrEmpty(mFollow.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = mFollow.User,
                        Password = mFollow.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    log.Add("Deben introducir Usuario y Contraseña");
                    return;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(mFollow.Pass))
                {
                    log.Add("Contraseña incorrecta");
                    return;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinesCount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaStatistics> GetEstadistica(mLogin BusinesCount) 
        {
            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

            if (!(string.IsNullOrEmpty(BusinesCount.User) || string.IsNullOrEmpty(BusinesCount.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = BusinesCount.User,
                    Password = BusinesCount.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return null;
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(BusinesCount.Pass))
            {
                log.Add("Contraseña incorrecta");
                return null;
            }
            if (insta.IsUserAuthenticated)
            {
                var busines = await insta.BusinessProcessor.GetStatisticsAsync();
                if (busines.Succeeded) 
                {
                    return busines.Value;
                }
                else 
                {
                    log.Add(busines.Info.Message);
                    return null;
                }
            }
            else 
            {
                log.Add("No estas autenticado, logueese");
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinesCount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<InstaMediaInsights> GetEstadisticaPost(mLogin BusinesCount)
        {
            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();            

            if (!(string.IsNullOrEmpty(BusinesCount.User) || string.IsNullOrEmpty(BusinesCount.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = BusinesCount.User,
                    Password = BusinesCount.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return null;
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(BusinesCount.Pass))
            {
                log.Add("Contraseña incorrecta");
                return null;
            }
            if (insta.IsUserAuthenticated)
            {
                var media = await insta.UserProcessor.GetUserMediaAsync(BusinesCount.User,PaginationParameters.MaxPagesToLoad(1));

                var businesmedia = await insta.BusinessProcessor.GetMediaInsightsAsync(media.Value[3].Pk);
                if (businesmedia.Succeeded)
                {
                    return businesmedia.Value;
                }
                else
                {
                    log.Add(businesmedia.Info.Message);
                    return null;
                }
            }
            else
            {
                log.Add("No estas autenticado, logueese");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Act_Comunidad(mFollower followers) 
        {
            
            DateTime now = DateTime.Now;
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
                        while ((DateTime.Now - now).TotalDays < 28)
                        {
                            var useractives = await insta.MessagingProcessor.GetUsersPresenceAsync();
                            if (useractives.Succeeded)
                            {
                                devolver.Clear();
                                if (useractives.Value.Count > 0)
                                {
                                    for (int i = 0; i < useractives.Value.Count; i++)
                                    {
                                        if (useractives.Value[i].IsActive)
                                        {
                                            var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(useractives.Value[i].Pk);
                                            string username = userinfo.Value.UserDetail.UserName + " horario " + DateTime.Now.ToString();                                            
                                            devolver.Add(username);

                                        }

                                    }
                                }
                                actcom.ActInsertar(user.Value.Pk.ToString(),DateTime.Now.Hour.ToString(),DateTime.Now.DayOfWeek.ToString(),devolver.Count);
                                Thread.Sleep(3600 * 1000);
                            }
                            else
                                return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return null;

                return "Concluido estudio de la comunidad del usuario:"+followers.User;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
        /// <summary>
        /// Eliminar usuarios que no te siguen
        /// </summary>
        /// <param name="credencial"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UnfollowNoMutual(mLogin credencial) 
        {
            PaginationParameters pagination = PaginationParameters.MaxPagesToLoad(1);
            List<long> Listid = new List<long>();
            List<long> Listmutuos = new List<long>();
            List<long> eliminar = new List<long>();
            int count = 0;
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();


            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, credencial.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(credencial);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                return "No hay Proxys disponibles";
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {


                return "Deben introducir el Proxy completo";


            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )


            };
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };


            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            if (!(string.IsNullOrEmpty(credencial.User) || string.IsNullOrEmpty(credencial.Pass)))
            {
                var userSessiontemp = new UserSessionData
                {
                    UserName = credencial.User,
                    Password = credencial.Pass
                };
                insta.SetUser(userSessiontemp);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
                return "Deben introducir Usuario y Contraseña";
            }

            session.LoadSession(insta);
            var pass = SecurityAPI.Decrypt(insta.GetLoggedUser().Password);

            if (!pass["Pass"].Equals(credencial.Pass))
            {
                log.Add("Contraseña incorrecta");
                return "Deben introducir Usuario y Contraseña";
            }

            var userSession = new UserSessionData
            {
                UserName = credencial.User,
                Password = credencial.Pass
            };
            insta.SetUser(userSession);
            if (insta.IsUserAuthenticated)
            {
                var user = await insta.UserProcessor.GetUserAsync(credencial.User);
                if (user.Succeeded)
                {
                    var mutualfollower = await insta.UserProcessor.GetMutualFriendsOrSuggestionAsync(user.Value.Pk);
                    if (mutualfollower.Succeeded)
                    {
                        for (int i = 0; i < mutualfollower.Value.MutualFollowers.Count; i++)
                        {
                            Listmutuos.Add(mutualfollower.Value.MutualFollowers[i].Pk);
                        }
                        var userinfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                        if (userinfo.Succeeded)
                        {
                            var following = userinfo.Value.UserDetail.FollowingCount;
                            long countpag = following / 100;
                            for (int i = 0; i < countpag + 1; i++)
                            {
                                var listfollowing = await insta.UserProcessor.GetUserFollowingByIdAsync(user.Value.Pk, pagination);
                                if (listfollowing.Succeeded)
                                {
                                    for (int j = 0; j < listfollowing.Value.Count; j++)
                                    {
                                        Listid.Add(listfollowing.Value[i].Pk);
                                    }
                                }
                            }
                            eliminar = util.compararListas(Listmutuos, Listid);
                            for (int i = 0; i < eliminar.Count; i++)
                            {
                                var eliminarid = insta.UserProcessor.GetUserInfoByIdAsync(eliminar[i]);
                                if (eliminarid.Result.Value.FollowerCount > 2000)
                                {
                                    if (userinfo.Value.UserDetail.FollowerCount * 3 < eliminarid.Result.Value.FollowerCount)
                                    {
                                        var eliminado = await insta.UserProcessor.UnFollowUserAsync(eliminar[i], InstaMediaSurfaceType.None);
                                        if (eliminado.Succeeded)
                                        {
                                            count++;
                                        }
                                    }
                                }
                                else
                                    if (userinfo.Value.UserDetail.FollowerCount * 6 < eliminarid.Result.Value.FollowerCount)
                                {
                                    var eliminado = await insta.UserProcessor.UnFollowUserAsync(eliminar[i], InstaMediaSurfaceType.None);
                                    if (eliminado.Succeeded)
                                    {
                                        count++;
                                    }
                                }
                            }
                            return "Fueron eliminados " + count + " usuarios seguidos que no te seguian de un total de " + eliminar.Count;
                        }
                        else
                            return userinfo.Info.Message;
                    }
                    else
                        return mutualfollower.Info.Message;
                }
                else
                    return user.Info.Message;
            }
            else
                return "Debe autenticarse primero";
        }

        /// <summary>
        /// Obtiene la sumatoria de post y de seguidores de un usuario.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> Cantidad_Post_Segudires(mLogin cliente)
        {
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();

            long cantidad = 0;

            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, cliente.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(cliente);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                log.Add("No hay Proxys disponibles");
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                log.Add("Deben introducir el Proxy completo");
            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )
            };

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            if (!(string.IsNullOrEmpty(cliente.User) || string.IsNullOrEmpty(cliente.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = cliente.User,
                    Password = cliente.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(cliente.Pass))
            {
                log.Add("Contraseña incorrecta");
            }

            var user = await insta.UserProcessor.GetUserInfoByUsernameAsync(cliente.User);

            if (user.Succeeded)
            {
                var userfull = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);

                if (userfull.Succeeded)
                {
                    cantidad = user.Value.FollowingCount + user.Value.MediaCount;
                }
            }
            return cantidad;
        }

        /// <summary>
        /// Obtiene la informacion Completa de un usuario
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<mClients> GetDetailsUser(mLogin cliente)
        {
            
            mProxy proxyconnect = new mProxy();
            List<mProxy> proxys = new List<mProxy>();
            mClients cliente_info = new mClients();
            
            proxys = prbd.CargarProxy();
            proxyconnect = util.ChoseProxy(proxys, cliente.Country, 1);
            if (proxyconnect.ErrorResult)
            {
                //insertar en la pila de errores de tareas de login pendientes pendientes
                bd.InsertarLogin(cliente);
                //devolver el tipo de error a la app para que notifique al cliente push notification al cliente
                //para esperar unos minutos.
                log.Add("No hay Proxys disponibles");
            }
            else
            {
                //update disponibilidad de los proxys. 
                bdprox.Update_Proxy(proxyconnect, 1);
            }
            if (string.IsNullOrEmpty(proxyconnect.AddressProxy) || string.IsNullOrEmpty(proxyconnect.UsernameProxy) || string.IsNullOrEmpty(proxyconnect.PassProxy))
            {
                log.Add("Deben introducir el Proxy completo");
            }
            var proxy = new WebProxy()
            {
                Address = new Uri(proxyconnect.AddressProxy),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                 userName: proxyconnect.UsernameProxy,
                 password: proxyconnect.PassProxy
                 )
            };

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            if (!(string.IsNullOrEmpty(cliente.User) || string.IsNullOrEmpty(cliente.Pass)))
            {
                var userSession = new UserSessionData
                {
                    UserName = cliente.User,
                    Password = cliente.Pass
                };
                insta.SetUser(userSession);
            }
            else
            {
                log.Add("Deben introducir Usuario y Contraseña");
            }

            session.LoadSession(insta);

            if (!insta.GetLoggedUser().Password.Equals(cliente.Pass))
            {
                log.Add("Contraseña incorrecta");
            }

            var user = await insta.UserProcessor.GetUserInfoByUsernameAsync(cliente.User);

            if (user.Succeeded)
            {
                var userfull = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);

                if (userfull.Succeeded)
                {
                    cliente_info.Info.clientid = userfull.Value.UserDetail.Pk.ToString();
                    cliente_info.Info.clientusername = userfull.Value.UserDetail.UserName;
                    cliente_info.Info.clientname = userfull.Value.UserDetail.FullName;
                    cliente_info.Info.clientcantpost = userfull.Value.UserDetail.MediaCount;
                    cliente_info.Info.clientcantfollowers = userfull.Value.UserDetail.FollowerCount;
                    cliente_info.Info.clientcantfollowing = userfull.Value.UserDetail.FollowingCount;
                    cliente_info.Info.clientemail = userfull.Value.UserDetail.PublicEmail;
                    cliente_info.Info.clientphone = userfull.Value.UserDetail.PublicPhoneNumber;
                    cliente_info.Info.clientcity = userfull.Value.UserDetail.CityName;
                    cliente_info.Info.clientcountry = userfull.Value.UserDetail.CityName;//Este tengo que revisarlo con raul;
                    cliente_info.Info.ClientAccounType.clientaccounttypecategory = userfull.Value.UserDetail.AccountType.ToString();//esto tengo que verlo con raul
                    cliente_info.Info.ClientAccounType.clientaccountype = userfull.Value.UserDetail.Category;
                    cliente_info.Tags.country = userfull.Value.UserDetail.CityName; //esto es ambiguo hay que verlo con raul
                    cliente_info.Tags.countrytarget = "No se de donde sacar esto";
                    cliente_info.Tags.industry = "No se de donde sacar esto";
                    cliente_info.Tags.industrysub = "No se de donde sacar esto";


                    //Aqui saco toda la informacion que necesito de los post del cliente
                    var post = await insta.UserProcessor.GetUserMediaAsync(cliente.User, PaginationParameters.MaxPagesToLoad(1));
                    if (post.Succeeded)
                    {
                        for (int i = 0; i < post.Value.Count; i++)
                        {
                            mClientPost clientPost = new mClientPost();
                            clientPost.cantlike = post.Value[i].LikesCount;
                            clientPost.postid = post.Value[i].Pk;
                            for (int j = 0; j < post.Value[i].Likers.Count; j++)
                            {
                                clientPost.likers.Add(post.Value[i].Likers[j].Pk.ToString());
                            }
                            cliente_info.posts.Add(clientPost);        
                        }
                    }

                }
            }
            return cliente_info;
        }

    }
}
