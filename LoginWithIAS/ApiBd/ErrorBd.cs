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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public mResultadoBd update_accion (string action)
        {
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_ACTION", OracleDbType.Int32, Convert.ToInt32(action), ParameterDirection.Input));
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

            return OracleDatabaseHelper.ExecuteToEntity<mResultadoBd>("PRC_GET_TAREA_PURIFICACION", parametros, "X_CURSOR", rowMapper, TipoPaquete.ERROR);

        }
    }
}