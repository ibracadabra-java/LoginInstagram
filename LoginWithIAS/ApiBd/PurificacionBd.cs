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
    public class PurificacionBd
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="purificador"></param>
        /// <param name="userlist"></param>
        /// <returns></returns>
        public mResultadoBd insertarTareasPurificacion(mPurificador purificador, string userlist)
        {
            mResultadoBd objResultBd = new mResultadoBd();
            List<OracleParameter> parametros = new List<OracleParameter>();            
            parametros.Add(new OracleParameter("X_USUARIO", OracleDbType.Varchar2, purificador.User, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_USUARIOS", OracleDbType.Varchar2, userlist, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_ID_TIPO", OracleDbType.Int32, 2, ParameterDirection.Input));
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

            return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_PURIFICADOR_INSERTAR", parametros, "X_ERROR", rowMapper);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public mPurificador GetTareaPurificacion(int ID)
        {           
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_ID_TAREA", OracleDbType.Int32, ID, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mPurificador> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mPurificador tarea = new mPurificador();
                string usuarios = string.Empty;
                tarea.UserList = new List<string>();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TAREA")))
                    tarea.id_tarea = Convert.ToInt32(oracleDataReader["ID_TAREA"]);
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIO")))
                    tarea.User = oracleDataReader["USUARIO"].ToString();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PASS")))
                    tarea.Pass = oracleDataReader["PASS"].ToString();               
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIOS")))
                    usuarios = oracleDataReader["USUARIOS"].ToString();               
                var user_list = usuarios.Split(',');
                for (int i = 0; i < user_list.Length; i++)
                {
                    tarea.UserList.Add(user_list[i].ToString());

                }
                return tarea;
            });

            return OracleDatabaseHelper.ExecuteToEntity<mPurificador>("PRC_GET_TAREA_PURIFICACION", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);
        }
    }
}