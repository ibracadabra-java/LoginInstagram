using LoginWithIAS.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace LoginWithIAS.ApiBd
{
    /// <summary>
    /// Tarea para enviar mensajes
    /// </summary>
    public class MasSendingBD
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();


        /// <summary>
        /// Insertar Reporte de los Mensajes enviados
        /// </summary>
        /// <param name="reports_Mess"></param>
        /// <returns></returns>
        public mResultadoBd Insertar_Reportes_Mensages(mReports_Mess reports_Mess)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_THREAD_ID", OracleDbType.Varchar2, reports_Mess.Thread_Id, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ITEM_ID", OracleDbType.Varchar2, reports_Mess.Item_Id, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CLIENTE_ID", OracleDbType.Int32, reports_Mess.Cliente_Id, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CANT_TOTAL", OracleDbType.Int32, reports_Mess.Cant_Total, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CANT_ENV", OracleDbType.Int32, reports_Mess.Cant_Env, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_LIST_IDS", OracleDbType.Varchar2, reports_Mess.List_Ids, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_INSERT_REPORT_MESS", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }

        }

        /// <summary>
        /// Actualizar Cantidad de Reaccines y Cantidad de vistos.
        /// </summary>
        /// <param name="reports_Mess"></param>
        /// <returns></returns>
        public mResultadoBd Update_Reportes_Mensages(mReports_Mess reports_Mess)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_THREAD_ID", OracleDbType.Varchar2, reports_Mess.Thread_Id, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CLIENTE_ID", OracleDbType.Int32, reports_Mess.Cliente_Id, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CANT_VISTOS", OracleDbType.Int32, reports_Mess.Cant_Vistos, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_CANT_REACC", OracleDbType.Int32, reports_Mess.Cant_Reacc, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_UPDATE_REPORT_MESS", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }

        }     

        /// <summary>
        /// Actualizar y mostrar resumen de los mensajes enviados a los clientes
        /// </summary>
        /// <returns></returns>
        public List<mReports_Mess> Get_Reports_Mess(long ID)
        {
            
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_CLIENTE_ID", OracleDbType.Int64, ID, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mReports_Mess> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mReports_Mess reporte = new mReports_Mess();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("THREAD_ID")))
                    reporte.Thread_Id = oracleDataReader["THREAD_ID"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ITEM_ID")))
                    reporte.Item_Id = oracleDataReader["ITEM_ID"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CLIENTE_ID")))
                    reporte.Cliente_Id = Convert.ToInt64(oracleDataReader["CLIENTE_ID"]);

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CANT_TOTAL")))
                    reporte.Cant_Total = Convert.ToInt32(oracleDataReader["CANT_TOTAL"]);

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CANT_ENV")))
                    reporte.Cant_Env = Convert.ToInt32(oracleDataReader["CANT_ENV"]);

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CANT_VISTOS")))
                    reporte.Cant_Vistos = Convert.ToInt32(oracleDataReader["CANT_VISTOS"]);

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CANT_REACC")))
                    reporte.Cant_Reacc = Convert.ToInt32(oracleDataReader["CANT_REACC"]);

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("LIST_IDS")))
                    reporte.List_Ids = oracleDataReader["LIST_IDS"].ToString();
                return reporte;
            });

            return OracleDatabaseHelper.ExecuteToList<mReports_Mess>("PRC_GET_REPORES_MSJ", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);

        }

        /// <summary>
        /// Obtiene la Tarea MasSending que se encargara de enviar un texto a una lista de usuarios
        /// </summary>
        /// <param name="idtarea"></param>
        /// <returns></returns>
        public mMasSending Get_MasSending(int idtarea)
        {
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_ID_TAREA", OracleDbType.Int64, idtarea, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mMasSending> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mMasSending massending = new mMasSending();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIOS")))
                    massending.Usuarios = oracleDataReader["USUARIOS"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("TEXTO")))
                    massending.Texto = oracleDataReader["TEXTO"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIO")))
                    massending.User = oracleDataReader["USUARIO"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PASS")))
                    massending.Pass = oracleDataReader["PASS"].ToString();
                return massending;
            });

            return OracleDatabaseHelper.ExecuteToEntity<mMasSending>("PRC_GET_MASS_SENDING", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);
        }

        /// <summary>
        /// Insertar Reporte de los Mensajes enviados
        /// </summary>
        /// <param name="sending"></param>
        /// <returns></returns>
        public mResultadoBd Insertar_MasSending(mMasSending sending)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_IDMLOGIN", OracleDbType.Varchar2, sending.User, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_USUARIOS", OracleDbType.Varchar2, sending.Usuarios, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_TEXTO", OracleDbType.Int32, sending.Texto, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_MASSENDING_INSERTAR", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }

        }
    }
}