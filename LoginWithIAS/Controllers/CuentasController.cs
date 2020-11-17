using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Enums;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using LoginWithIAS.Utiles;
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
    /// 
    /// </summary>
    public class CuentasController : ApiController
    {
        Session session;
        Util util;
        /// <summary>
        /// 
        /// </summary>
        public CuentasController()
        {
            this.session = new Session();
            this.util = new Util();
        }

        /// <summary>
        /// Este metodo nos permitira definir si la cuenta de un usurio es falsa o poco inactiva
        /// </summary>
        /// <returns></returns>
        public async Task<enResponseToken> Cuentas_Ainactivas(mcuentas cuentas)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                int cont = 0;

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(cuentas.User) || string.IsNullOrEmpty(cuentas.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = cuentas.User,
                        Password = cuentas.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(cuentas.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserAsync(cuentas.Other_User);
                if (user.Succeeded)
                {
                    var userfull = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);

                    if (userfull.Succeeded)
                    {
                        long cant_followers = userfull.Value.UserDetail.FollowerCount;
                        if (cant_followers >= 1000)
                        {
                            var listafollowers = await insta.UserProcessor.GetUserFollowersAsync(cuentas.Other_User, PaginationParameters.MaxPagesToLoad(10));
                            if (listafollowers.Succeeded)
                            {


                                for (int i = 0; i < listafollowers.Value.Count; i++)
                                {
                                    var media = await insta.UserProcessor.GetUserMediaAsync(listafollowers.Value[i].UserName, PaginationParameters.MaxPagesToLoad(1));
                                    if (media.Succeeded)
                                    {
                                        for (int j = 0; j < media.Value.Count; j++)
                                        {
                                            int dias = Cantidad_Dias(media.Value[j].TakenAt);
                                            if (media.Value.Count >= 30 && dias <= 60)
                                            {
                                                if (Buscar(media.Value[j].Likers, user.Value.Pk))
                                                {
                                                    cont++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                token.Message = user.Info.Message;
                                return token;
                            }

                        }
                        else if (cant_followers < 1000)
                        {
                            int pagina = Treinta_Porciento(int.Parse(cant_followers.ToString())) / 100 + 1;
                            var listafollowers = await insta.UserProcessor.GetUserFollowersAsync(cuentas.Other_User, PaginationParameters.MaxPagesToLoad(pagina));

                            if (listafollowers.Succeeded)
                            {


                                for (int i = 0; i < listafollowers.Value.Count; i++)
                                {
                                    var media = await insta.UserProcessor.GetUserMediaAsync(listafollowers.Value[i].UserName, PaginationParameters.MaxPagesToLoad(1));
                                    if (media.Succeeded)
                                    {
                                        for (int j = 0; j < media.Value.Count; j++)
                                        {
                                            int dias = Cantidad_Dias(media.Value[j].TakenAt);
                                            if (media.Value.Count >= 30 && dias <= 60)
                                            {
                                                if (Buscar(media.Value[j].Likers, user.Value.Pk))
                                                {
                                                    cont++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                token.Message = user.Info.Message;
                                return token;
                            }
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
                    token.Message = user.Info.Message;
                    return token;
                }

                if (cont == 0)
                {
                    token.Message = "La Cuenta del usuario:" + cuentas.Other_User + " es Falsa";
                    return token;
                }
                else
                {
                    token.Message = "La Cuenta del usuario:" + cuentas.Other_User + " es poco activa";
                    return token;
                }
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        private int Treinta_Porciento(int cantidad)
        {
            if (cantidad >= 1000)
                return 1000;
            else
            {
                return cantidad * 30 / 100;
            }
        }

        private int Cantidad_Dias(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;
            return timeSpan.Days;
        }

        private bool Buscar(InstaUserShortList lista, long pk)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Pk == pk)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determina si un usuario ha sido eliminado por Instagram o la cuenta ha sido deshabilidata o
        /// ha bloqueado a nuestro cliente
        /// </summary>
        /// <param name="otrocliente">Por parametro se logean en el sistema con otro cliente y se pasa por parametro el identificador del usuario que 
        /// desean saber si fue elimiando por instagram p simplemente bloqueo o dejo de seguir a nuestro cliente principal</param>
        /// <returns></returns>
        public async Task<enResponseToken> Detector_Cuentas(mcuentas otrocliente)
        {
            try
            {
                enResponseToken token = new enResponseToken();


                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(otrocliente.User) || string.IsNullOrEmpty(otrocliente.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = otrocliente.User,
                        Password = otrocliente.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(otrocliente.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.UserProcessor.GetUserInfoByIdAsync(otrocliente.Pk_Other_User);
                if (user.Succeeded)
                {
                    token.Message = "El usuario dejo de seguir a nuestro cliente";

                    return token;
                }
                else
                {
                    token.Message = "El usuario no existe o ha sido eliminado";
                    return token;
                }
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

        /// <summary>
        /// Este método se encargará de eliminar seguidores, específicamente seguidores falsos
        /// </summary>
        /// <param name="mLogin"></param>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        public async Task<enResponseToken> Purificador(mLogin mLogin, List<string> usuarios)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                List<string> devolver = new List<string>();
                PaginationParameters pagination = PaginationParameters.MaxPagesToLoad(1);

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(mLogin.User) || string.IsNullOrEmpty(mLogin.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = mLogin.User,
                        Password = mLogin.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(mLogin.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }
                //Cargar lista de los seguidores del cliente
                int count = 0;
                var userlist = await insta.UserProcessor.GetUserFollowersAsync(mLogin.User, PaginationParameters.MaxPagesToLoad(1));
                if (userlist.Succeeded)
                {
                    for (int i = 0; i < usuarios.Count; i++)
                    {
                        var user = await insta.UserProcessor.GetUserAsync(usuarios[i]);
                        if (user.Succeeded)
                        {
                            var userInfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
                            if (userInfo.Succeeded)
                            {
                                await insta.UserProcessor.RemoveFollowerAsync(userInfo.Value.UserDetail.Pk);
                                count++;
                            }
                        }
                    }
                }
                token.Message = "Fueron eliminados un total de:" + count.ToString() + ", de seguidores.";
                return token;
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }


        /// <summary>
        /// Reporte de los usuarios que te han dejado de seguir
        /// </summary>
        /// <param name="mLogin"></param>.
        /// <returns></returns>
        public async Task<enResponseToken> Reporte(mLogin mLogin)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                List<string> devolver = new List<string>();
                PaginationParameters pagination = PaginationParameters.MaxPagesToLoad(1);

                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(mLogin.User) || string.IsNullOrEmpty(mLogin.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = mLogin.User,
                        Password = mLogin.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(mLogin.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }
                //Cargar lista de los seguidores del cliente
                int count = 0;

                var user = await insta.UserProcessor.GetUserAsync(mLogin.User);
                if (user.Succeeded)
                {
                    var userInfo = await insta.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);

                    if (userInfo.Succeeded)
                    {
                        var resultado = insta.UserProcessor.UnFollowUserAsync(userInfo.Value.UserDetail.Pk);

                       
                    }
                    else
                        return null;
                }
                else
                    return null;
                
                return token;
            }
            catch (Exception s)
            {
                throw new Exception(s.Message);
            }
        }

    }
}
