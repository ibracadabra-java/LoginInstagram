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
using LoginWithIAS.ApiBd;
using LoginWithIAS.Worker;
namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TareasController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TareasDia"></param>
        /// <returns></returns>
        [HttpPost]
        public mResultadoBd InsertarTareas(mTareasDiarias TareasDia) 
        {            
            mTareas tareas = new mTareas();
            mResultadoBd objResultado = new mResultadoBd();
            TareasBd objbd = new TareasBd();
            for (int i = 0; i < TareasDia.TareasDia.Count; i++)
            {
                

                for (int j = 0; j < TareasDia.TareasDia[i].tareas.Count; j++)
                {
                    switch (TareasDia.TareasDia[i].tareas[j].id)
                    {
                        case 1:
                            mMethodLike mlike = new mMethodLike();
                            List<string> Datos = TareasDia.TareasDia[i].tareas[j].info;
                            mlike.vel = Convert.ToInt32(Datos[0]);
                            mlike.User = TareasDia.TareasDia[i].user;
                            mlike.cantLike =Convert.ToInt32(Datos[1]);
                            string userlist = Datos[2];
                            objResultado =  objbd.insertarTareas(mlike, userlist);
                            break;
                    }

                }

            }
            return objResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<mTarea> GetTareas() 
        {
            TareasBd objbd = new TareasBd();
            return objbd.GetTareas();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public mMethodLike GetTareaExpancion(int id)
        {
            TareasBd objbd = new TareasBd();            
            return objbd.GetTareaExpancion(id);
            

        }
        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        public void EjecutarTareas()
        {
            BackGroundWork groundWork = new BackGroundWork();
            groundWork.MainThreadEjecutarTareas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values"></param>
        [HttpPost]
        public void StopTareas(string Values) 
        {
            BackGroundWork tareasobj = new BackGroundWork();
            var Datos = Values.Split(',');
            string Name = Datos[0];
            string Reazon = Datos[1];

            tareasobj.pararTarea(Name,Reazon);

            
        }
       
       
        

    }
}
