using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class AuthenticationService
    {
        /*
        public static bool ValidateUser(string name, string password)
        {
            SqlHelper sqlHelper = new SqlHelper(Configuration.ConnectionString);
            string sql = string.Format("Select TOP 1 userId, name, role, siteId From lt_user Where name = '{0}' and password = '{1}' and isDisabled = 0", name, password);
            if (sqlHelper.ExecuteScalar(sql) != null)
            {
                return true;
            }
            return false;
        }
        */

        public static string[] GetRoleForUser(string username)
        {
            List<string> roles = new List<string>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                string sql = string.Format("Select TOP 1 role From Users Where name = '{0}' and isDisabled = 0", username);
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                roles.Add(reader["role"].ToString());
                            }
                        }

                        reader.Close();
                    }
                }
            }
            return roles.ToArray();
        }

        public static List<int> GetUserCategorias(string username)
        {
            List<int> categorias = new List<int>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                string sql = string.Format("Select c.CategoriaId From Users u inner join UsersCategoria c on (u.Id = c.userId) Where u.name = '{0}'", username);
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                categorias.Add(Convert.ToInt32(reader["CategoriaId"]));
                            }
                        }

                        reader.Close();
                    }
                }
            }
            return categorias;
        }

        public static User ValidateUser(string name, string password)
        {
            User user = null;
            //TODO: usar encriptacion MD5
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                string sql = string.Format("Select TOP 1 Id, Name, Role, EmpresaId, Nombre , Apellido , Email , Localidad, Empresa, Provincia From Users Where Name = '{0}' and Password = '{1}' and isDisabled = 0", name, password);
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user = LoadUser(reader);
                            }
                        }
                        
                        reader.Close();
                    }
                }
            }

            return user;
        }

        public static Boolean UserExists(string name)
        {
            //TODO: usar encriptacion MD5
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                string sql = string.Format("Select 1 From Users Where Name = '{0}'", name);
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }

                        reader.Close();
                    }
                }
            }

            return false;
        }

        public static int User_Insert(string name, string password, string nombre, string apellido, string localidad, string provincia, string empresa, string email, string role)
        {
            try
            {
                SqlHelper sqlHelper = new SqlHelper(Configuration.ConnectionString);
                SqlParameter[] parameters = new SqlParameter[9];
                parameters.SetValue(new SqlParameter { ParameterName = "@name", SqlDbType = System.Data.SqlDbType.VarChar, Value = name }, 0);
                parameters.SetValue(new SqlParameter { ParameterName = "@password", SqlDbType = System.Data.SqlDbType.VarChar, Value = password }, 1);
                parameters.SetValue(new SqlParameter { ParameterName = "@nombre", SqlDbType = System.Data.SqlDbType.VarChar, Value = nombre }, 2);
                parameters.SetValue(new SqlParameter { ParameterName = "@apellido", SqlDbType = System.Data.SqlDbType.VarChar, Value = apellido }, 3);
                parameters.SetValue(new SqlParameter { ParameterName = "@empresa", SqlDbType = System.Data.SqlDbType.VarChar, Value = empresa }, 4);
                parameters.SetValue(new SqlParameter { ParameterName = "@provincia", SqlDbType = System.Data.SqlDbType.VarChar, Value = provincia }, 5);
                parameters.SetValue(new SqlParameter { ParameterName = "@localidad", SqlDbType = System.Data.SqlDbType.VarChar, Value = localidad }, 6);
                parameters.SetValue(new SqlParameter { ParameterName = "@email", SqlDbType = System.Data.SqlDbType.VarChar, Value = email }, 7);
                parameters.SetValue(new SqlParameter { ParameterName = "@role", SqlDbType = System.Data.SqlDbType.VarChar, Value = role }, 8);
                
                object result = sqlHelper.ExecuteScalar("User_Save", parameters);
                int newid = 0;
                if (result != null)
                {
                    int.TryParse(result.ToString(), out newid);
                }
                return newid;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UserCategoria_Insert(int userId, int categoriaId)
        {
            try
            {
                SqlHelper sqlHelper = new SqlHelper(Configuration.ConnectionString);
                SqlParameter[] parameters = new SqlParameter[2];
                parameters.SetValue(new SqlParameter { ParameterName = "@userId", SqlDbType = System.Data.SqlDbType.Int, Value = userId }, 0);
                parameters.SetValue(new SqlParameter { ParameterName = "@categoriaId", SqlDbType = System.Data.SqlDbType.Int, Value = categoriaId }, 1);

                object result = sqlHelper.ExecuteScalar("UserCategoria_Save", parameters);
                int newid = 0;
                if (result != null)
                {
                    int.TryParse(result.ToString(), out newid);
                }
                return newid;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private static User LoadUser(SqlDataReader reader)
        {
            User item = new User
            {
                Name = reader["Name"].ToString(),
                Id = Convert.ToInt32(reader["Id"]),
                Role = reader["Role"].ToString()
            }; 

            if (reader["EmpresaId"] != DBNull.Value)
            {
                item.EmpresaId = Convert.ToInt32(reader["EmpresaId"]);
            }

            if (reader["Nombre"] != DBNull.Value)
            {
                item.Nombre = reader["Nombre"].ToString();
            }

            if (reader["Apellido"] != DBNull.Value)
            {
                item.Apellido = reader["Apellido"].ToString();
            }

            if (reader["Email"] != DBNull.Value)
            {
                item.Email = reader["Email"].ToString();
            }

            if (reader["Localidad"] != DBNull.Value)
            {
                item.Localidad = reader["Localidad"].ToString();
            }

            if (reader["Provincia"] != DBNull.Value)
            {
                item.Provincia = reader["Provincia"].ToString();
            }

            if (reader["Empresa"] != DBNull.Value)
            {
                item.Empresa = reader["Empresa"].ToString();
            }

            return item;
        }
    }
}
