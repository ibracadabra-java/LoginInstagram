using LoginWithIAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoginWithIAS.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GestorTareasController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tareasDia"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveTask(mTareasDiarias tareasDia) 
        {
            for (int i = 0; i < tareasDia.TareasDia.Count; i++)
            {
                for (int j = 0; j < tareasDia.TareasDia[i].tareas.Count; j++)
                {


                    switch (tareasDia.TareasDia[i].tareas[j].id)
                    {
                        default:
                            break;
                    }
                }
            }
            return "";
        }
    }
}
