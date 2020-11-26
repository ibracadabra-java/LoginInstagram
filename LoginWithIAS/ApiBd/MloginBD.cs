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
    /// Clase para la persistencia de datos de los clientes autenticados en la aplicacion
    /// </summary>
    public class MloginBD
    {
        string conexion = ConfigurationManager.ConnectionStrings["ConexBD"].ToString();

        /// <summary>
        /// Insertar Clientes en el sistema
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public mResultadoBd Insertar_Mlogin(mLogin login)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_CODENAME", OracleDbType.Varchar2, login.AndroidVer.Codename, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_VERSIONUMBER", OracleDbType.Varchar2, login.AndroidVer.VersionNumber, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_APILEVEL", OracleDbType.Varchar2, login.AndroidVer.APILevel, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ADID", OracleDbType.Varchar2, login.AdId.ToString(), ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ANDBOARDNAME", OracleDbType.Varchar2, login.AndroidBoardName, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ANDBOOTLOADER", OracleDbType.Varchar2, login.AndroidBootloader, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEBRAND", OracleDbType.Varchar2, login.DeviceBrand, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEGUID", OracleDbType.Varchar2, login.DeviceGuid.ToString(), ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEID", OracleDbType.Varchar2, login.DeviceId, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODEL", OracleDbType.Varchar2, login.DeviceModel, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODELBOOT", OracleDbType.Varchar2, login.DeviceModelBoot, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODELIDENTIFIER", OracleDbType.Varchar2, login.DeviceModelIdentifier, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DPI", OracleDbType.Varchar2, login.Dpi, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_RESOLUTION", OracleDbType.Varchar2, login.Resolution, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWAREFINGERPRINT", OracleDbType.Varchar2, login.FirmwareFingerprint, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWARETAGS", OracleDbType.Varchar2, login.FirmwareTags, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWARETYPE", OracleDbType.Varchar2, login.FirmwareType, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ADDRESSPROXY", OracleDbType.Varchar2, login.AddressProxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_USERPROXY", OracleDbType.Varchar2, login.UsernameProxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PASSPROXY", OracleDbType.Varchar2, login.PassProxy, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_USUARIO", OracleDbType.Varchar2, login.User, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PASS", OracleDbType.Varchar2, login.Pass, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PK", OracleDbType.Varchar2, login.PK, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_MLOGIN_INSERTAR", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }

        }


    }
}