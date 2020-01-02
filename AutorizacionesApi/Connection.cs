using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi
{
    public class Connection
    {
        private string Usuario = "desaosoc";
        private string Pass = "osoc2013";

        private OracleConnection Connect()
        {
            var oradb ="Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (COMMUNITY = tcp.world)(PROTOCOL = TCP)(HOST = 10.5.5.28)(PORT = 1521)))(CONNECT_DATA = (SID = CM12c))); User Id = "+ Usuario+"; Password ="+ Pass + "; ";
            oradb = string.Format(oradb, Usuario, Pass);
            var conn = new OracleConnection();
            conn.ConnectionString = oradb;
            return conn;
        }

        public DataTable Query(string query)
        {
            try
            {
                var c = new Connection();
                var conn = c.Connect();

                var cmd = new OracleCommand(query, conn) { CommandType = CommandType.Text };
                DataTable dt = new DataTable();
                var da = new OracleDataAdapter(cmd);

                conn.Open();
                da.Fill(dt);

                cmd.Dispose();
                conn.Dispose();

                return dt;
            }
            catch (Exception ex)
            {
                ///Todo Enviar Email
                var sdfds = ex;
                return null;
            }
        }

        public object QueryObject(string query)
        {
            try
            {
                var c = new Connection();
                var conn = c.Connect();

                var cmd = new OracleCommand(query, conn) { CommandType = CommandType.Text };
                conn.Open();

                object lResultado = cmd.ExecuteScalar();

                cmd.Dispose();
                conn.Close();

                return lResultado;
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", query, query);
            }
            return null;
        }

        public void QueryNoResult(string query)
        {
            try
            {
                var c = new Connection();
                var conn = c.Connect();

                var cmd = new OracleCommand(query, conn) { CommandType = CommandType.Text };
                conn.Open();
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", query, query);
            }
        }

    }
}
