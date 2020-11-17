using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace LoginWithIAS.ApiBd
{
    internal class OracleDatabaseHelper
    {
       
        private static string packageName_mant = "APIINST.PKG_API_MANT";
        

        public delegate T RowMapper<T>(OracleDataReader oracleDataReader) where T : class;
        public delegate void RowMapperList<T>(OracleDataReader oracleDataReader, List<T> entityList) where T : class;

        private static OracleConnection OpenConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexBD"].ConnectionString;
            OracleConnection oracleConnection = new OracleConnection();
            oracleConnection.ConnectionString = connectionString;
            oracleConnection.Open();
            return oracleConnection;
        }

        private static void CloseConnection(OracleConnection oracleConnection)
        {
            if (oracleConnection != null)
            {
                oracleConnection.Close();
                oracleConnection.Dispose();
            }
        }

        private static void CloseCommand(OracleCommand oracleCommand)
        {
            if (oracleCommand != null)
            {
                oracleCommand.Dispose();
            }
        }

        private static void CloseDataReader(OracleDataReader oracleDataReader)
        {
            if (oracleDataReader != null)
            {
                oracleDataReader.Close();
                oracleDataReader.Dispose();
            }
        }

        private static OracleCommand PrepareCommand(string procedureName, List<OracleParameter> parameters, OracleConnection oracleConnection, TipoPaquete tipoPaquete)
        {
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandType = CommandType.StoredProcedure;
            //oracleCommand.Transaction = oracleConnection.BeginTransaction();
           
           if (tipoPaquete == TipoPaquete.MANT)
            {
                oracleCommand.CommandText = packageName_mant + "." + procedureName;
            }           
            else
            {
                throw new Exception("Tipo de paquete no definido");
            }

            foreach (OracleParameter parameter in parameters)
            {
                oracleCommand.Parameters.Add(parameter);
            }

            return oracleCommand;
        }

        public static List<T> ExecuteToList<T>(string procedureName, OracleParameter parameter, string cursorName, RowMapper<T> rowMapper) where T : class
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(parameter);
            return ExecuteToList<T>(procedureName, parameters, cursorName, rowMapper);
        }

        public static List<T> ExecuteToList<T>(string procedureName, List<OracleParameter> parameters, string cursorName, RowMapper<T> rowMapper, TipoPaquete tipoPaquete = TipoPaquete.MANT) where T : class
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;
            OracleDataReader oracleDataReader = null;

            List<T> listEntity = new List<T>();
            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, tipoPaquete);
                oracleCommand.ExecuteNonQuery();
                oracleDataReader = ((OracleRefCursor)oracleCommand.Parameters[cursorName].Value).GetDataReader();

                while (oracleDataReader.Read())
                {
                    T entity = rowMapper(oracleDataReader);
                    listEntity.Add(entity);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(oracleDataReader);
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }

            return listEntity;
        }

        public static List<T> ExecuteToList<T>(string procedureName, OracleParameter parameter, string cursorName, RowMapperList<T> rowMapper) where T : class
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(parameter);
            return ExecuteToList<T>(procedureName, parameters, cursorName, rowMapper);
        }

        public static List<T> ExecuteToList<T>(string procedureName, List<OracleParameter> parameters, string cursorName, RowMapperList<T> rowMapper) where T : class
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;
            OracleDataReader oracleDataReader = null;

            List<T> listEntity = new List<T>();
            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, TipoPaquete.MANT);
                oracleCommand.ExecuteNonQuery();
                oracleDataReader = ((OracleRefCursor)oracleCommand.Parameters[cursorName].Value).GetDataReader();

                while (oracleDataReader.Read())
                {
                    rowMapper(oracleDataReader, listEntity);
                    //listEntity.Add(entity);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(oracleDataReader);
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }

            return listEntity;
        }

        public static T ExecuteToEntity<T>(string procedureName, List<OracleParameter> parameters, string cursorName, RowMapper<T> rowMapper, TipoPaquete tipoPaquete = TipoPaquete.MANT) where T : class
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;
            OracleDataReader oracleDataReader = null;

            T entity = null;
            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, tipoPaquete);
                oracleCommand.ExecuteNonQuery();
                oracleDataReader = ((OracleRefCursor)oracleCommand.Parameters[cursorName].Value).GetDataReader();

                if (oracleDataReader.Read())
                {
                    entity = rowMapper(oracleDataReader);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(oracleDataReader);
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }

            return entity;
        }

        public static T ExecuteToEntityMant<T>(string procedureName, List<OracleParameter> parameters, string cursorName, RowMapper<T> rowMapper) where T : class
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;
            OracleDataReader oracleDataReader = null;

            T entity = null;
            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, TipoPaquete.MANT);
                oracleCommand.ExecuteNonQuery();
                oracleDataReader = ((OracleRefCursor)oracleCommand.Parameters[cursorName].Value).GetDataReader();

                if (oracleDataReader.Read())
                {
                    entity = rowMapper(oracleDataReader);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(oracleDataReader);
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }

            return entity;
        }

        public static void Execute(string procedureName, OracleParameter parameter)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(parameter);

            Execute(procedureName, parameters);
        }

        public static void Execute(string procedureName, List<OracleParameter> parameters)
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;

            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, TipoPaquete.MANT);
                oracleCommand.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }
        }

        public static void Execute(string procedureName, OracleParameter parameter, string parameterOutput, ref object parameterOutputValue, TipoPaquete tipoPaquete)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(parameter);

            Execute(procedureName, parameters, parameterOutput, ref parameterOutputValue, tipoPaquete);
        }

        public static void Execute(string procedureName, List<OracleParameter> parameters, string parameterOutput, ref object parameterOutputValue, TipoPaquete tipoPaquete = TipoPaquete.MANT)
        {
            OracleConnection oracleConnection = null;
            OracleCommand oracleCommand = null;

            try
            {
                oracleConnection = OpenConnection();
                oracleCommand = PrepareCommand(procedureName, parameters, oracleConnection, tipoPaquete);
                oracleCommand.ExecuteNonQuery();
                parameterOutputValue = oracleCommand.Parameters[parameterOutput].Value;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                CloseCommand(oracleCommand);
                CloseConnection(oracleConnection);
            }
        }

    }
    /*    private static List<T>  execute(string region){
     List<T> list = new List<T>;

 }*/

    enum TipoPaquete
    {              MANT
        
    }
}