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
    public class ActComBd
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="hora"></param>
        /// <param name="dia"></param>
        /// <param name="cant"></param>
        public mResultadoBd ActInsertar(string pk, string hora, string dia, int cant) 
        {
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_PK_USUARIO", OracleDbType.Varchar2, pk, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_HORA_ACT", OracleDbType.Varchar2, hora, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_DIA_ACT", OracleDbType.Varchar2, dia, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CANT_USER", OracleDbType.Varchar2, cant, ParameterDirection.Input));
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

            return OracleDatabaseHelper.ExecuteToEntity<mResultadoBd>("PRC_INSERT_ACT", parametros, "X_ERROR", rowMapper, TipoPaquete.MANT);


        }
    }
}