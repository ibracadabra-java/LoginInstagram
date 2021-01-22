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
    /// 
    /// </summary>
    public class ProxyBD
    {
        /// <summary>
        /// INSERTAR NUEVO PROXY
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public mResultadoBd Insertar_Proxy(mProxy proxy)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_ADDRESSPROXY", OracleDbType.Varchar2, proxy.AddressProxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_USERPROXY", OracleDbType.Varchar2, proxy.UsernameProxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PASSPROXY", OracleDbType.Varchar2, proxy.PassProxy, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_PROXY_INSERTAR", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
        /// <summary>
        /// Cargar proxy
        /// </summary>       
        /// <returns></returns>
        public List<mProxy> CargarProxy()
        {

            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<mProxy> proxys = new List<mProxy>();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

                OracleDatabaseHelper.RowMapper<mProxy> rowMapper = (delegate (OracleDataReader oracleDataReader)
                {
                    mProxy proxy = new mProxy();

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ID_PROXY")))
                        proxy.Id_proxy = Convert.ToInt32(oracleDataReader["ID_PROXY"]);

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USERPROXY")))
                        proxy.UsernameProxy = oracleDataReader["USERPROXY"].ToString();

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PASSPROXY")))
                        proxy.PassProxy = oracleDataReader["PASSPROXY"].ToString();

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ADDRESSPROXY")))
                        proxy.AddressProxy = oracleDataReader["ADDRESSPROXY"].ToString();

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PAIS")))
                        proxy.Pais = oracleDataReader["PAIS"].ToString();

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CURRENT_TASK")))
                        proxy.CurrentTask = Convert.ToInt32(oracleDataReader["CURRENT_TASK"]);

                    if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("TAREA_PRIORIDAD")))
                        proxy.TareaPrioridad = Convert.ToInt32(oracleDataReader["TAREA_PRIORIDAD"]);
                    return proxy;
                });
                return OracleDatabaseHelper.ExecuteToList<mProxy>("PRC_GET_PROXY", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// actualizar disponibilidad del proxy
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public mResultadoBd Update_Proxy(mProxy proxy, int count)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_id_proxy", OracleDbType.Int32, proxy.Id_proxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_count", OracleDbType.Int32, count, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_UPDATE_DISP_PROXY", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }
        }
    }
}