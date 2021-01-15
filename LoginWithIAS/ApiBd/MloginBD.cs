﻿using InstagramApiSharp.Classes.Android.DeviceInfo;
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
        /// <param name="device"></param>
        /// <returns></returns>
        public mResultadoBd Insertar_Mlogin(mLogin login,AndroidDevice device)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_CODENAME", OracleDbType.Varchar2, login.AndroidVer.Codename, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_VERSIONUMBER", OracleDbType.Varchar2, login.AndroidVer.VersionNumber, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_APILEVEL", OracleDbType.Varchar2, login.AndroidVer.APILevel, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ADID", OracleDbType.Varchar2, device.AdId.ToString(), ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ANDBOARDNAME", OracleDbType.Varchar2, device.AndroidBoardName, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_ANDBOOTLOADER", OracleDbType.Varchar2, device.AndroidBootloader, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEBRAND", OracleDbType.Varchar2, device.DeviceBrand, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEGUID", OracleDbType.Varchar2, device.DeviceGuid.ToString(), ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEID", OracleDbType.Varchar2, device.DeviceId, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODEL", OracleDbType.Varchar2, device.DeviceModel, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODELBOOT", OracleDbType.Varchar2, device.DeviceModelBoot, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DEVICEMODELIDENTIFIER", OracleDbType.Varchar2, device.DeviceModelIdentifier, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_DPI", OracleDbType.Varchar2, device.Dpi, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_RESOLUTION", OracleDbType.Varchar2, device.Resolution, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWAREFINGERPRINT", OracleDbType.Varchar2, device.FirmwareFingerprint, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWARETAGS", OracleDbType.Varchar2, device.FirmwareTags, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_FIRMWARETYPE", OracleDbType.Varchar2, device.FirmwareType, ParameterDirection.Input));                
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

        /// <summary>
        /// Eliminar Usuario Autenticado
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public mResultadoBd Eliminar_Mlogin(string login)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                
                parametros.Add(new OracleParameter("X_PK", OracleDbType.Varchar2, login, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_MLOGIN_DELETE", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception s)
            {

                throw new Exception(s.Message);
            }

        }

        /// <summary>
        /// Obtener Usuario Autenticado
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public mLogin Get_Mlogin(string login)
        {
            List<OracleParameter> parametros = new List<OracleParameter>();
            parametros.Add(new OracleParameter("X_PK", OracleDbType.Varchar2, login, ParameterDirection.Input));
            parametros.Add(new OracleParameter("X_ERROR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            OracleDatabaseHelper.RowMapper<mLogin> rowMapper = (delegate (OracleDataReader oracleDataReader)
            {
                mLogin logeado = new mLogin();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("USUARIO")))
                    logeado.User = oracleDataReader["USUARIO"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PASS")))
                    logeado.Pass = oracleDataReader["PASS"].ToString();                

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICE")))
                    logeado.DeviceId = oracleDataReader["DEVICE"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("PK")))
                    logeado.PK = oracleDataReader["PK"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ADID")))
                    logeado.AdId = new Guid(oracleDataReader["ADID"].ToString());

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ANDROIDBOARDNAME")))
                    logeado.AndroidBoardName = oracleDataReader["ANDROIDBOARDNAME"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("ANDROIDBOOTLOADER")))
                    logeado.AndroidBootloader = oracleDataReader["ANDROIDBOOTLOADER"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICEBRAND")))
                    logeado.DeviceBrand = oracleDataReader["DEVICEBRAND"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICEGUID")))
                    logeado.DeviceGuid = new Guid(oracleDataReader["DEVICEGUID"].ToString());

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICEMODEL")))
                    logeado.DeviceModel = oracleDataReader["DEVICEMODEL"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICEMODELBOOT")))
                    logeado.DeviceModelBoot = oracleDataReader["DEVICEMODELBOOT"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DEVICEMODELIDENTIFIER")))
                    logeado.DeviceModelIdentifier = oracleDataReader["DEVICEMODELIDENTIFIER"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("DPI")))
                    logeado.Dpi = oracleDataReader["DPI"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("FIRMWAREFINGERPRINT")))
                    logeado.FirmwareFingerprint = oracleDataReader["FIRMWAREFINGERPRINT"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("FIRMWARETAGS")))
                    logeado.FirmwareTags = oracleDataReader["FIRMWARETAGS"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("FIRMWARETYPE")))
                    logeado.FirmwareType = oracleDataReader["FIRMWARETYPE"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("RESOLUTION")))
                    logeado.Resolution = oracleDataReader["RESOLUTION"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("RESOLUTION")))
                    logeado.Resolution = oracleDataReader["RESOLUTION"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("APILEVEL")))
                    logeado.AndroidVer.APILevel = oracleDataReader["APILEVEL"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("CODENAME")))
                    logeado.AndroidVer.Codename = oracleDataReader["CODENAME"].ToString();

                if (!oracleDataReader.IsDBNull(oracleDataReader.GetOrdinal("VERSIONNUMBER")))
                    logeado.AndroidVer.VersionNumber = oracleDataReader["VERSIONNUMBER"].ToString();
                

                return logeado;
            });

            return OracleDatabaseHelper.ExecuteToEntity<mLogin>("PRC_GET_MLOGIN", parametros, "X_CURSOR", rowMapper, TipoPaquete.CONS);
        }
        /// <summary>
        /// insertar login
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public mResultadoBd InsertarLogin(mLogin usuario)
        {
            try
            {
                mResultadoBd objResultBd = new mResultadoBd();
                List<OracleParameter> parametros = new List<OracleParameter>();
                parametros.Add(new OracleParameter("X_USUARIO", OracleDbType.Varchar2, usuario.User, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PASS", OracleDbType.Varchar2, usuario.Pass, ParameterDirection.Input));
                parametros.Add(new OracleParameter("X_PAIS", OracleDbType.Varchar2, usuario.Country, ParameterDirection.Input));
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

                return objResultBd = OracleDatabaseHelper.ExecuteToEntityMant("PRC_INSERT_TAREA_LOGIN", parametros, "X_ERROR", rowMapper);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



    }
}