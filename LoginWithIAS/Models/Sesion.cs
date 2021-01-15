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
    public class Sesion
    {
        /// <summary>
        /// Método para cargar session
        /// </summary>
        /// <param name="IstanciaApi">Instancia del objeto IInstaApi</param>
        public void LoadSession(IInstaApi IstanciaApi)
        {
            var path = Path.Combine(Helper.AccountPathDirectory, $"{IstanciaApi.GetLoggedUser().UserName}{Helper.SessionExtension}");
            try
            {
                if (File.Exists(path))
                {

                    using (var fs = File.OpenRead(path))
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
        /// 
        /// </summary>
        /// <param name="IstanciaApi"></param>
        public void LoadSessionBackup(IInstaApi IstanciaApi)
        {
            var path = Path.Combine(Helper.AccountPathDirectory + "Backup", $"{IstanciaApi.GetLoggedUser().UserName}{Helper.SessionExtension}");
            try
            {
                if (File.Exists(path))
                {

                    using (var fs = File.OpenRead(path))
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
            Helper.CreateAccountDirectory();
            try
            {
                if (IstanciaApi == null)
                    return;
                var state = IstanciaApi.GetStateDataAsStream();
                var path = Path.Combine(Helper.AccountPathDirectory, $"{IstanciaApi.GetLoggedUser().UserName}{Helper.SessionExtension}");
                using (var fileStream = File.Create(path))
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
        /// 
        /// </summary>
        /// <param name="IstanciaApi"></param>
        public void SaveSessionBackup(IInstaApi IstanciaApi)
        {
            Helper.CreateAccountDirectoryBackup();
            try
            {
                if (IstanciaApi == null)
                    return;
                var state = IstanciaApi.GetStateDataAsStream();
                var path = Path.Combine(Helper.AccountPathDirectory + "Backup", $"{IstanciaApi.GetLoggedUser().UserName}{Helper.SessionExtension}");
                using (var fileStream = File.Create(path))
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