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
        /// 
        /// </summary>
        /// <returns></returns>     
        public List<mTarea> GetTareas( )
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

            return OracleDatabaseHelper.ExecuteToList<mTarea>("PRC_GET_TAREAS", parametros, "X_CURSOR", rowMapper,TipoPaquete.CONS);

        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public mMethodLike GetTareaExpancion(int ID)
        {
            mResultadoBd objResultBd = new mResultadoBd();
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_ID_TAREA", OracleDbType.Int32, ID, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mMethodLike> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mMethodLike tarea = new mMethodLike();
                string usuarios = string.Empty;
                tarea.ListUser = new List<string>();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_TAREA")))
                    tarea.id_tarea = Convert.ToInt32(oracleDataReader["ID_TAREA"]);
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIO")))
                    tarea.User = oracleDataReader["USUARIO"].ToString();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PASS")))
                    tarea.Pass = oracleDataReader["PASS"].ToString();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("VELOCIDAD")))
                    tarea.vel = Convert.ToInt32(oracleDataReader["VELOCIDAD"]);
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIOS")))
                     usuarios = oracleDataReader["USUARIOS"].ToString();
                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CANTLIKE")))
                    tarea.cantLike = Convert.ToInt32(oracleDataReader["CANTLIKE"]);
                var user_list = usuarios.Split(',');
                for (int i = 0; i < user_list.Length; i++)
                {
                    tarea.ListUser.Add(user_list[i].ToString());

                }
                return tarea;
            });

            return OracleDatabaseHelper.ExecuteToEntity<mMethodLike>("PRC_GET_TAREA_EXPANCION", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);
        }

        /// <summary>
        /// Actualizar y mostrar resumen de los mensajes enviados a los clientes
        /// </summary>
        /// <returns></returns>
        public List<mReports_Mess> Get_Reports_Mess(long ID)
        {
            mResultadoBd objResultBd = new mResultadoBd();
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
    }
}