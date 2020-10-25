using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// Controlador de Contactos
    /// </summary>
    public class ContacProcessController : ApiController
    {
        /// <summary>
        /// Objeto seccion para salvar y calgar los datos
        /// </summary>
        Session session;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContacProcessController()
        {
            this.session = new Session();
        }

       /* /// <summary>
        /// Metodo para extraer el contacto de un usuario
        /// </summary>
        /// <returns></returns>
       public async Task<List<string>> getContactos(mContacts credencial)
        {
            List<string> listado_contactos = new List<string>();

            if (!string.IsNullOrEmpty(credencial.AddressProxy))
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri(credencial.AddressProxy),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                       userName: credencial.UsernameProxy,
                       password: credencial.PassProxy
                       )

                };
            }
            else
            {
                
            }
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            var device = new AndroidDevice
            {

                AdId = credencial.AdId,
                AndroidBoardName = credencial.AndroidBoardName,
                AndroidBootloader = credencial.AndroidBootloader,
                AndroidVer = credencial.AndroidVer,
                DeviceBrand = credencial.DeviceBrand,
                DeviceGuid = new Guid(credencial.DeviceGuid.ToString()),
                DeviceId = ApiRequestMessage.GenerateDeviceIdFromGuid(new Guid(credencial.DeviceId.ToString())),
                DeviceModel = credencial.DeviceModel,
                DeviceModelBoot = credencial.DeviceModelBoot,
                DeviceModelIdentifier = credencial.DeviceModelIdentifier,
                Dpi = credencial.Dpi,
                Resolution = credencial.Resolution,
                FirmwareFingerprint = credencial.FirmwareFingerprint,
                FirmwareTags = credencial.FirmwareTags,
                FirmwareType = credencial.FirmwareType

            }; 
             var userSession = new UserSessionData
             {
                 UserName = credencial.User,
                 Password = credencial.Pass
             };
            var InstaApi = InstaApiBuilder.CreateBuilder().SetUser(userSession).UseLogger(new DebugLogger(LogLevel.All)).UseHttpClientHandler(httpClientHandler).Build();

            //InstaApi.SetDevice(device);
            session.LoadSession(InstaApi);

            if (InstaApi.IsUserAuthenticated)
            {

            }
            else
            {
                
            }

            return listado_contactos;

        }*/
    }
}
