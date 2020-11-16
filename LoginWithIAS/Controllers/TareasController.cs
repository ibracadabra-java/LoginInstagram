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
            for (int i = 0; i < TareasDia.TareasDia.Count; i++)
            {

            }
            return objResultado;
        }
    }
}
