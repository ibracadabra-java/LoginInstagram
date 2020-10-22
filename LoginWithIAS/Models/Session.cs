using InstagramApiSharp.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Session
    {
        /// <summary>
        /// 
        /// </summary>
        public const string StateFile = "state.bin";

        /// <summary>
        /// Método para cargar session
        /// </summary>
        /// <param name="IstanciaApi">Instancia del objeto IInstaApi</param>
       public void LoadSession(IInstaApi IstanciaApi)
        {
            try
            {
                if (File.Exists(StateFile))
                {

                    using (var fs = File.OpenRead(StateFile))
                    {
                        IstanciaApi.LoadStateDataFromStream(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Método para guardar la session
        /// </summary>
        /// <param name="IstanciaApi">Instancia del objeto IInstaApi</param>
        public void SaveSession(IInstaApi IstanciaApi)
        {
            try
            {
                if (IstanciaApi == null)
                    return;
                var state = IstanciaApi.GetStateDataAsStream();
                using (var fileStream = File.Create(StateFile))
                {
                    state.Seek(0, SeekOrigin.Begin);
                    state.CopyTo(fileStream);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        /// <summary>
        /// Metodo para generar Token de salida al loguarse
        /// </summary>   
        /// <returns>Retorna un objeto enResponseToken con el token y un mensaje descriptivo</returns>
        public string GenerarToken()
        {
            string token = string.Empty;
            Random obj = new Random();
            string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string sNuevacadena = string.Empty;
            char cletra;


            for (int i = 0; i < 20; i++)
            {
                cletra = sCadena[obj.Next(sCadena.Length)];
                sNuevacadena += cletra.ToString();
            }
            token = sNuevacadena;

            return token;

        } 
    }
}