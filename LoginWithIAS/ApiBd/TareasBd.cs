using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Configuration;
using LoginWithIAS.Models;
using System.Data;

namespace LoginWithIAS.ApiBd
{
    /// <summary>
    /// 
    /// </summary>
    public class TareasBd
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TareasDia"></param>
        /// <returns></returns>
      /*  public mResultadoBd insertarTareas(mTareasDiarias TareasDia) 
        {
            for (int i = 0; i < TareasDia.TareasDia.Count; i++)
            {

                for (int j = 0; j < TareasDia.TareasDia[i].tareas.Count; j++)
                {


                    switch (TareasDia.TareasDia[i].tareas[j].id)
                    {
                        default:
                            break;
                    }
                    mResultadoBd objResultBd = new mResultadoBd();
                    List<OracleParameter> parametros = new List<OracleParameter>();
                    parametros.Add(new OracleParameter("X_ID_TIPO", OracleDbType.Int32, TareasDia.TareasDia[1].tareas[1].id, ParameterDirection.Input));
                    parametros.Add(new OracleParameter("X_USER", OracleDbType.Varchar2, TareasDia.TareasDia[1].user, ParameterDirection.Input));
                    parametros.Add(new OracleParameter("X_TAREA_INFO", OracleDbType.Varchar2, TareasDia.TareasDia[1].tareas[1].info, ParameterDirection.Input));
                }
            }
            return objResultadoBd;
            
            
        }*/

    }
}