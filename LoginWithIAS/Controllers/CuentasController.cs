using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using LoginWithIAS.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LoginWithIAS.Controllers
{
    public class CuentasController : ApiController
    {
        Session session;
        Util util;

        public CuentasController()
        {
            this.session = new Session();
            this.util = new Util();
        }

        /// <summary>
        /// Este metodo no permitira definir si la cuenta de un usurio es falsa o poco inactiva
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
                                            if (media.Value.Count >= 30 && dias<=60)
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
                        else if (cant_followers <1000)
                        {
                            int pagina = Treinta_Porciento(int.Parse(cant_followers.ToString()))/100+1;
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
                    token.Message = "La Cuenta del usuario:" + cuentas.Other_User+ " es Falsa";
                    return token;
                }
                else
                {
                    token.Message = "La Cuenta del usuario:" + cuentas.Other_User+ " es poco activa";
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
                if (lista[i].Pk == pk )
                {
                    return true;
                }
            }
            return false;
        }
    }
}
