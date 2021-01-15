using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LoginWithIAS.App_Start
{
<<<<<<< HEAD
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Token = "Authorization";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
=======
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Authorization";
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                if (ValidateToken(tokenValue))
                {
                    UserLogged userLogged = new UserLogged();

                    Dictionary<string, object> payload = SecurityAPI.Decrypt(tokenValue);

<<<<<<< HEAD
                    userLogged.User =payload["IdUsuario"].ToString();
=======
                    userLogged.User =payload["User"].ToString();
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b

                    HttpContext.Current.User = userLogged;
                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        
        }

<<<<<<< HEAD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
=======
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
        public bool ValidateToken(string token)
        {
            try
            {
                Dictionary<string, object> payload = SecurityAPI.Decrypt(token);

                if (payload.ContainsKey("ExpirationDate"))
                {
                    if (Convert.ToDateTime(payload["ExpirationDate"]) > DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
<<<<<<< HEAD
            catch (Exception)
=======
            catch (Exception ex)
>>>>>>> 070ec1789425e24df7ed7bb62e34c9e7dde4516b
            {
                return false;
            }
        }
    }
}