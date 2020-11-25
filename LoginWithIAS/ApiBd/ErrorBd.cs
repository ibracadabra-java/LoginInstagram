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
    public class ErrorBd
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc_error"></param>
        /// <returns></returns>
        public mError SysError(string desc_error)
        {
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_DESC_ERROR", OracleDbType.Varchar2, desc_error, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });


            OracleDatabaseHelper.RowMapper<mError> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mError objerror = new mError();                               
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TIPO")))
                    objerror.id_tipo = Convert.ToInt32(oracleDataReader["ID_TIPO"]);
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DESC_ERROR")))
                    objerror.desc_error = oracleDataReader["DESC_ERROR"].ToString();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ACTION")))
                    objerror.action = Convert.ToInt32(oracleDataReader["ACTION"]);

                return objerror;
            });

            return OracleDatabaseHelper.ExecuteToEntity<mError>("PRC_GET_TAREA_PURIFICACION", parametros, "X_CURSOR", rowMapper, TipoPaquete.ERROR);

        } 
    }
}