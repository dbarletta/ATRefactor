using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class LugarService
    {
        public static List<Lugar> Get(int lugarId = 0)
        {
            List<Lugar> items = new List<Lugar>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Lugares_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@lugarId", lugarId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(Load(reader));
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static List<String> GetRegiones(int categoriaId = 0)
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Regiones_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(reader[0].ToString());
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static List<String> GetLocalidades(int categoriaId = 0)
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Localidades_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(reader[0].ToString());
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static List<String> GetLocalidades_x_prov(string prov = "Capital Federal")
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Localidades_GetProv", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@provincia", prov);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(reader[0].ToString());
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }


        public static List<String> GetProvincias(int categoriaId = 0)
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Provincias_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(reader[0].ToString());
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

        private static Lugar Load(SqlDataReader reader)
        {
            Lugar item = new Lugar
            {
                Id = Convert.ToInt32(reader["Id"]),
                Region = reader["region"].ToString(),
                Departamento = reader["departamento"].ToString(),
                Provincia = reader["provincia"].ToString()
                
            };

            return item;
        }

        //Pablo
        private static ProductoLugares LoadPL(SqlDataReader reader)
        {

               
            try
            {
                ProductoLugares item = new ProductoLugares
                {
                    ProductoId = Convert.ToInt32(reader["ProductoId"]),
                    ProductoNombre = reader["ProductoNombre"].ToString(),
                    LugarId = Convert.ToInt32(reader["LugarId"]),
                    Region = reader["Region"].ToString(),
                };
                return item;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        private static Lugar LoadRegion(SqlDataReader reader)
        {
            Lugar item = new Lugar
            {
                Id = Convert.ToInt32(reader["Id"]),
                Region = reader["Region"].ToString(),

            };

            return item;
        }

        public static List<ProductoLugares> GetProductoLugares(int productoid)
        {
            List<ProductoLugares> items = new List<ProductoLugares>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("ProductosLugares_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@productoid", productoid);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(LoadPL(reader));
                            }
                        }
                        reader.Close();
                    }
                }
            }
            if (items != null)
                return items;

            else
                return new List<ProductoLugares>();
        }

        public static List<Lugar> GetAllRegiones()
        {
            List<Lugar> items = new List<Lugar>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Lugares_GetAllRegiones", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(LoadRegion(reader));
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

    }
}
