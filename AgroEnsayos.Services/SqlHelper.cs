namespace AgroEnsayos.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;

    public sealed class SqlHelper
    {
        #region Members

        private string connectionString;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SqlHelper class.
        /// </summary>
        /// <param name="connectionString">Entity Data Model ConnectionString</param>
        public SqlHelper(string connectionString)
        {
            this.Initialize(connectionString, true);
        }

        public SqlHelper(string connectionString, bool entityConnectionString)
        {
            this.Initialize(connectionString, entityConnectionString);
        }

        private void Initialize(string connectionString, bool entityConnectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Execute Command to underliying database
        /// </summary>
        /// <param name="command">Command text to execute</param>
        /// <returns>Rows affected</returns>
        public int ExecuteCommand(string command)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = command;
                cmd.CommandTimeout = 600; //10 minutes

                if ((conn.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteCommand(string command, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                cmd.CommandText = command;
                cmd.CommandTimeout = 60; //1 minutes

                if ((conn.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public SqlDataReader GetDataReader(string command)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = command;
                cmd.CommandTimeout = 600; //10 minutes

                if ((conn.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteReader();
            }
        }

        public int ExecuteCommand(string command, SqlConnection conn, SqlTransaction tx)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = command;
            cmd.CommandTimeout = 600;
            cmd.Transaction = tx;

            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string command, SqlConnection conn, SqlTransaction tx)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = command;
            cmd.CommandTimeout = 600;
            cmd.Transaction = tx;

            return cmd.ExecuteScalar();
        }

        public object ExecuteScalar(string command, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                cmd.CommandText = command;

                if ((conn.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteScalar();
            }
        }

        public object ExecuteScalar(string command)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = command;

                if ((conn.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteScalar();
            }
        }

        public DataTable GetTable(string name, string sql, SqlConnection conn, SqlTransaction tx)
        {
            DataTable table = new DataTable(name);
            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Transaction = tx;
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(table);
            }

            return table;
        }

        public DataTable GetTable(string name, string sql)
        {
            DataTable table = new DataTable(name);
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(table);
                }

                conn.Close();
            }

            return table;
        }

        public DataSet Fill(DataSet dataSet, string sql)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.CommandTimeout = 600;
                    da.Fill(dataSet);
                }

                conn.Close();
            }

            return dataSet;
        }

        #endregion
    }
}