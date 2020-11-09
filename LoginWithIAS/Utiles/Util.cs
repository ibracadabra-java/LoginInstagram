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
        /// <param name="cadena"></param>
        /// <returns></returns>
        public string Accion(string cadena)
        {
            Random objram = new Random();
            List<string> acciones = new List<string>();
            for (int i = 0; i < cadena.Length; i++)
            {
                for (int j = i+1; j < cadena.Length; j++)
                {
                    acciones.Add(cadena[i].ToString() + cadena[j].ToString());
                }

            }
            return acciones[objram.Next(0,acciones.Count)];
        }
        
    }
}