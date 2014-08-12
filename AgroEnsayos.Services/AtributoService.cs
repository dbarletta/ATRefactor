using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class AtributoService
    {
        public static List<Atributo> Get(int categoriaId)
        {
            List<Atributo> items = new List<Atributo>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Atributos_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoriaId", categoriaId);
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

        public static Atributo GetById(int id)
        {
            Atributo attr = null;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Atributos_GetById", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            attr = Load(reader);
                        }
                    }
                }
            }

            return attr;
        }

        public static List<string> GetValores(int atributoId)
        {
            List<string> items = new List<string>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Atributos_GetValores", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@atributoId", atributoId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(reader["Valor"].ToString());
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static int Save(Atributo atributo)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                int IdOriginal = atributo.Id;
                using (SqlCommand command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Rubro", atributo.Rubro);
                    command.Parameters.AddWithValue("@Nombre", atributo.Nombre);
                    command.Parameters.AddWithValue("@TipoDato", atributo.TipoDato);
                    command.Parameters.AddWithValue("@Tags", atributo.Tags);
                    command.Parameters.AddWithValue("@UsarComoFiltro", atributo.UsarComoFiltro);

                    if (atributo.Id == 0)
                    {
                        command.CommandText = "Atributos_Insert";
                        atributo.Id = (int)(decimal)command.ExecuteScalar();
                    }
                    else
                    {
                        command.CommandText = "Atributos_Update";
                        command.Parameters.AddWithValue("@Id", atributo.Id);
                        command.ExecuteNonQuery();
                    }
                }

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoriaId", atributo.CategoriaId);
                    command.Parameters.AddWithValue("@Id", atributo.Id);
                    command.CommandText = atributo.Id != IdOriginal ? "AtributoCategorias_insert" : "AtributoCategorias_update";
                    command.ExecuteNonQuery();
                }

                return atributo.Id;
            }
        }

        public static void DisableAttributte(int id)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    command.CommandText = "Atributo_Deshabilitar";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<AtributoEquivalencia> GetEquivalencias(int atributoId)
        {
            List<AtributoEquivalencia> items = new List<AtributoEquivalencia>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Atributos_GetEquivalencias", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@atributoId", atributoId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(LoadEquivalencia(reader));
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static void SaveEquivalencia(List<AtributoEquivalencia> equivalencias)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("AtributoId", typeof(int));
                dt.Columns.Add("Valor", typeof(string));
                dt.Columns.Add("Equivalencia", typeof(string));
                dt.Columns.Add("Escala", typeof(byte));

                foreach (var e in equivalencias)
                {
                    DataRow row = dt.NewRow();
                    row[0] = e.AtributoId;
                    row[1] = e.Valor;
                    row[2] = e.Equivalencia;
                    row[3] = e.Escala;
                    dt.Rows.Add(row);
                }

                SqlHelper sqlHelper = new SqlHelper(Configuration.ConnectionString);
                SqlParameter[] parameters = new SqlParameter[1];
                parameters.SetValue(new SqlParameter { ParameterName = "@details", SqlDbType = System.Data.SqlDbType.Structured, Value = dt }, 0);
                object result = sqlHelper.ExecuteScalar("Atributos_SaveEquivalencia", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Atributo> Filter_Get(int categoriaId, int filtro = 0)
        {
            List<Atributo> items = new List<Atributo>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Filtros_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@filtro", filtro);
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

        public static List<Atributo> ProductoAtributo_Get(int productoId = 0)
        {
            List<Atributo> items = new List<Atributo>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("ProductosAtributos_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@productoId", productoId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(AtributoService.Load(reader));
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static List<Atributo> Atributo_GetWithOriginalValues(int productoId)
        {
            List<Atributo> items = new List<Atributo>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Atributos_GetAllWithProductValues", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductoId", productoId);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                items.Add(AtributoService.Load(reader));
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return items;
        }


        private static Atributo Load(SqlDataReader reader)
        {
            Atributo item = new Atributo
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString(),
                TipoDato = Convert.ToByte(reader["TipoDato"])
            };

            if (reader["Tags"] != DBNull.Value)
            {
                item.Tags = reader["Tags"].ToString();
            }

            if (reader["Rubro"] != DBNull.Value)
            {
                item.Rubro = reader["Rubro"].ToString();
            }

            try
            {
                item.UsarComoFiltro = Convert.ToBoolean(reader["UsarComoFiltro"]);
                if (reader["CategoriaId"] != DBNull.Value)
                {
                    item.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
                }

                if (reader["Categoria"] != DBNull.Value)
                {
                    item.Categoria = reader["Categoria"].ToString();
                }
            }
            catch { }

            item.equivalencia_valor = "";
            if (reader["valor"] != DBNull.Value)
            {
                item.equivalencia_valor = reader["valor"].ToString();
            }

            return item;
        }

        private static AtributoEquivalencia LoadEquivalencia(SqlDataReader reader)
        {
            AtributoEquivalencia item = new AtributoEquivalencia
            {
                AtributoId = Convert.ToInt32(reader["AtributoId"]),
                Atributo = reader["Atributo"].ToString(),
                Valor = reader["Valor"].ToString(),
                Equivalencia = reader["Equivalencia"].ToString(),
                Escala = Convert.ToByte(reader["Escala"])
            };

            return item;
        }
    }
}
