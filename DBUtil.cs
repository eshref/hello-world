using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

    public static class DBUtil
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }

        //public static int GetRowsAffected(string sql)
        //{
        //    //FileLogger.LogText(Utils.GetJsonString(sql));
        //    OracleConnection conn = new OracleConnection(ConnectionString);
        //    OracleCommand cmd = new OracleCommand(sql, conn);
        //    int count = 0;
        //    try
        //    {
        //        conn.Open();
        //        count = cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception exc)
        //    {
        //        DbLogger.LogException(exc, sql);
        //        count = -1;
        //        conn.Close();
        //        throw exc;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //    return count;
        //}

        public static int GetRowsAffected(OracleCommand cmd)
        {
            //FileLogger.LogText(cmd.CommandText);
            OracleConnection conn = new OracleConnection(ConnectionString);
            cmd.Connection = conn;
            int count = 0;
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                DbLogger.LogException(exc, cmd.CommandText);
                count = -1;
                conn.Close();
                throw exc;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleDataAdapter da = new OracleDataAdapter(sql, conn);
            try
            {
                conn.Open();
                da.Fill(dt);
            }
            catch (Exception exc)
            {
                DbLogger.LogException(exc, sql);
                conn.Close();
                throw exc;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static DataTable GetData(OracleCommand cmd)
        {
            DataTable dt = new DataTable();
            OracleConnection conn = new OracleConnection(ConnectionString);
            cmd.Connection = conn;
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(dt);
            }
            catch (Exception exc)
            {
                DbLogger.LogException(exc, cmd.CommandText);
                conn.Close();
                throw exc;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static object GetResult(string sql)
        {
            object res = new object();
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand(sql, conn);
            try
            {
                conn.Open();
                res = cmd.ExecuteScalar();
            }
            catch (Exception exc)
            {
                DbLogger.LogException(exc, sql);
                conn.Close();
                throw exc;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static object GetResult(OracleCommand cmd)
        {
            object res = new object();
            OracleConnection conn = new OracleConnection(ConnectionString);
            cmd.Connection = conn;
            try
            {
                conn.Open();
                res = cmd.ExecuteScalar();
            }
            catch (Exception exc)
            {
                DbLogger.LogException(exc, cmd.CommandText);
                conn.Close();
                throw exc;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

        public static int GetSequence(string SequenceName)
        {
            return int.Parse(GetResult("SELECT " + SequenceName + ".NEXTVAL FROM DUAL").ToString());
        }

        public static object IfEmpty(string value)
        {
            if (value == "")
                return DBNull.Value;
            else
                return value;
        }

        public static void ExecuteProcedure(string procName, CmdParameter[] parameters)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand(procName, conn);
            cmd.CommandTimeout = 30000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procName;
            foreach (CmdParameter cp in parameters)
                cmd.Parameters.Add(cp.Parameter);
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
        }

        public static void ExecuteNativeSql(string sql, CmdParameter[] parameters)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.CommandTimeout = 30000;
            foreach (CmdParameter cp in parameters)
                cmd.Parameters.Add(cp.Parameter);
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
        }

        public static void ExecuteNativeSql(string sql)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.CommandTimeout = 30000;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static DataTable Select(string tableName, string select, CmdParameter[] parameters)
        {
            DataTable retTable = new DataTable(tableName);
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleDataAdapter adapter = new OracleDataAdapter(select, conn);
            foreach (CmdParameter cp in parameters)
                adapter.SelectCommand.Parameters.Add(cp.Parameter);
            adapter.SelectCommand.CommandTimeout = 30000;
            conn.Open();
            adapter.Fill(retTable);
            adapter.SelectCommand.Parameters.Clear();
            conn.Close();
            return retTable;
        }

        public static DataTable Select(string select, CmdParameter[] parameters)
        {
            DataTable retTable = new DataTable();
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleDataAdapter adapter = new OracleDataAdapter(select, conn);
            foreach (CmdParameter cp in parameters)
                adapter.SelectCommand.Parameters.Add(cp.Parameter);
            adapter.SelectCommand.CommandTimeout = 30000;
            conn.Open();
            adapter.Fill(retTable);
            adapter.SelectCommand.Parameters.Clear();
            conn.Close();
            return retTable;
        }

        public static DataTable Select(string select)
        {
            DataTable retTable = new DataTable();
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleDataAdapter adapter = new OracleDataAdapter(select, conn);
            adapter.SelectCommand.CommandTimeout = 30000;
            conn.Open();
            adapter.Fill(retTable);
            conn.Close();
            return retTable;
        }
    }
