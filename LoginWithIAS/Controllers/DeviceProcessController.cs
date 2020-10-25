using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web.Http;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador de Dispositivos
    /// </summary>
    public class DeviceProcessController : ApiController
    {
        Session session;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        public DeviceProcessController()
        {
            this.session = new Session();
        }

        /// <summary>
        /// Eliminar Dispositivos
        /// </summary>
        /// <param name="deletedevices"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<enResponseToken> DeleteDevice(mdevice deletedevices)
        {
            try
            {
                enResponseToken token = new enResponseToken();
                var insta = InstaApiBuilder.CreateBuilder().UseLogger(new DebugLogger(LogLevel.All)).Build();

                if (!(string.IsNullOrEmpty(deletedevices.User) || string.IsNullOrEmpty(deletedevices.Pass)))
                {
                    var userSession = new UserSessionData
                    {
                        UserName = deletedevices.User,
                        Password = deletedevices.Pass
                    };
                    insta.SetUser(userSession);
                }
                else
                {
                    token.Message = "Deben introducir Usuario y Contraseña";
                    return token;
                }

                session.LoadSession(insta);

                if (!insta.GetLoggedUser().Password.Equals(deletedevices.Pass))
                {
                    token.Message = "Contraseña incorrecta";
                    return token;
                }

                var user = await insta.AccountProcessor.GetLoginSessionsAsync();

                if (user.Succeeded)
                {
                    for (int i = 0; i < user.Value.Sessions.Count; i++)
                    {
                        if (user.Value.Sessions[i].Device.Equals(deletedevices.DeviceId))
                        {
                            var device = insta.AccountProcessor.RemoveTrustedDeviceAsync(deletedevices.DeviceId);
                            token.Message = "Dispositivo Eliminado correctamente";
                            token.AuthToken = session.GenerarToken();
                            return token;
                        }
                        else
                        {
                            token.Message = "Identificador del Dispositivo incorrecto";
                            return token;
                        }
                    }
                }
                else
                {
                    token.Message = "No tiene sesiones de Login Activas";
                    return token;
                }
                return token;
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
    }
}
