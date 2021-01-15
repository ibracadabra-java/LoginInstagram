using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using LoginWithIAS.Models;

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
        /// <summary>
        /// Comparar lista de seguidos y mutuos
        /// </summary>
        /// <param name="mutuos"></param>
        /// <param name="seguidos"></param>
        /// <returns>Los usuarios que sigues pero no te siguen</returns>
        public List<long> compararListas(List<long> mutuos,List<long> seguidos) 
        {
            List<long> eliminar = new List<long>();
            bool find;
            for (int i = 0; i < seguidos.Count; i++)
            {
                find = false;
                for (int j = 0; j < mutuos.Count; j++)
                {
                    if (seguidos[i] == mutuos[j])
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    eliminar.Add(seguidos[i]);
                }
            }
            return eliminar;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxis"></param>
        /// <param name="pais"></param>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public mProxy ChoseProxy(List<mProxy> proxis,string pais,int tarea)
        {
            mProxy proxy = new mProxy();
            proxis = proxis.OrderBy(x => x.CurrentTask).ToList();            
            for (int i = 0; i < proxis.Count; i++)
            {
                if(proxis[i].Pais.Equals(pais) && proxis[i].TareaPrioridad == tarea  ) 
                {                    
                    return proxis[i];
                }                
            }
            for (int i = 0; i < proxis.Count; i++)
            {
                if(proxis[i].TareaPrioridad == tarea)
                {
                    return proxis[i];
                }
            }
            if (tarea == 1)
            {
                return proxis[0];
            }
            else
            {
                proxy.ErrorResult = true;
                return proxy;
            }
            
        }
    }
}