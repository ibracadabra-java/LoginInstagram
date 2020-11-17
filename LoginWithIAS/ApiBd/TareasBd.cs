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
        /// <param name="mmethodlike"></param>
        /// <param name="userlist"></param>
        /// <returns></returns>
        public mResultadoBd insertarTareas(mMethodLike mmethodlike, string userlist)
        {
            mResultadoBd objResultBd = new mResultadoBd();
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_ID_TIPO", OracleDbType.Int32, 1, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_USER", OracleDbType.Varchar2, mmethodlike.User, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_VEL", OracleDbType.Varchar2, mmethodlike.vel, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CANT", OracleDbType.Int32, mmethodlike.cantLike, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_USERLIST", OracleDbType.Varchar2, userlist, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_ERROR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mResultadoBd> rowMapper = (delegate (OracleDataReader oracleDataReader)
          {
              mResultadoBd objEnResultado = new mResultadoBd();

              if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TIPO")))
                  objEnResultado.ID_TIPO = Convert.ToInt32(oracleDataReader["ID_TIPO"]);
              if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_ERROR")))
                  objEnResultado.ID_ERROR = oracleDataReader["ID_ERROR"].ToString();
              if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DES_ERROR")))
                  objEnResultado.DES_ERROR = oracleDataReader["DES_ERROR"].ToString();
              if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("VALOR")))
                  objEnResultado.VALOR = oracleDataReader["VALOR"].ToString();

              return objEnResultado;
          });

            return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_TAREA_INSERTAR", parametros, "X_ERROR", rowMapper);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>     
        public List<mTarea> gGetTareasT( )
        {



            mResultadoBd objResultBd = new mResultadoBd();
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mTarea> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mTarea tarea = new mTarea();
                if(!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TAREA")))
                    tarea.idTarea = Convert.ToInt32(oracleDataReader["ID_TAREA"]);
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TIPO_TAREA")))
                    tarea.id = Convert.ToInt32(oracleDataReader["ID_TIPO_TAREA"]);
                return tarea;
            });

            return OracleDatabaseHelper.ExecuteToList<mTarea>("PRC_GET_OFERTAS", parametros, "X_CURSOR", rowMapper,TipoPaquete.CONS);

        }


    }
}