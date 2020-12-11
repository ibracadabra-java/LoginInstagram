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
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Authorization";
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                if (ValidateToken(tokenValue))
                {
                    UserLogged userLogged = new UserLogged();

                    Dictionary<string, object> payload = SecurityAPI.Decrypt(tokenValue);

                    userLogged.User =payload["User"].ToString();

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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}