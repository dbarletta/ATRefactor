using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class EnsayoService
    {
        public static List<Ensayo> Get(int? categoriaId = null, int? productoId = null, int? campanaId = null, int? lugarId = null, string fuente = null, bool isChart = false)
        {
            List<Ensayo> items = new List<Ensayo>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Ensayos_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@productoId", productoId);
                    command.Parameters.AddWithValue("@campanaId", campanaId);
                    command.Parameters.AddWithValue("@lugarId", lugarId);
                    command.Parameters.AddWithValue("@fuente", fuente);
                    command.Parameters.AddWithValue("@isChart", isChart);
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

        public static List<Ensayo> Lookup(int categoriaId, string searchTerm, string empresa = "", string fuente = "", string provincia = "", string localidad = "", string campana = "", List<string> cond_atributo = null)
        {
            List<Ensayo> items = new List<Ensayo>();
            int pos = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("AtributoId", typeof(int));
            dt.Columns.Add("Valor", typeof(string));
            dt.Columns.Add("Equivalencia", typeof(string));
            dt.Columns.Add("Escala", typeof(int));

            if (cond_atributo != null)
            {
                int atributoId = 0;
                foreach (var d in cond_atributo)
                {
                    DataRow row = dt.NewRow();
                    pos = d.IndexOf("--");
                    int.TryParse(d.Substring(0, pos), out atributoId); //Lucas: esto es para parsear a int
                    row[0] = atributoId;
                    row[1] = "";
                    row[2] = d.Substring(pos + 2);
                    row[3] = 0;
                    dt.Rows.Add(row);
                }
            }

            //parameters.SetValue(new SqlParameter { ParameterName = "@details", SqlDbType = System.Data.SqlDbType.Structured, Value = dt }, 0);


            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Ensayos_Lookup", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@searchTerm", searchTerm);
                    command.Parameters.AddWithValue("@empresa", empresa);
                    command.Parameters.AddWithValue("@fuente", fuente);
                    command.Parameters.AddWithValue("@provincia", provincia);
                    command.Parameters.AddWithValue("@localidad", localidad);
                    command.Parameters.AddWithValue("@campana", campana);
                    command.Parameters.AddWithValue("@atributoTbl", dt);
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

        public static List<String> GetFuentes(int categoriaId=0)
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Fuentes_Get", conn))
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

        public static List<String> GetCampanas(int categoriaId=0)
        {
            List<String> items = new List<String>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Campanas_Get", conn))
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
                                items.Add(reader[2].ToString());
                            }
                        }
                        reader.Close();
                    }
                }
            }

            return items;
        }

        public static Ensayo Load(SqlDataReader reader)
        {
            Ensayo item = new Ensayo
            {
                Id = Convert.ToInt32(reader["Id"]),
                CampanaId = Convert.ToInt32(reader["CampanaId"]),
                Campana = reader["Campana"].ToString(),
                ProductoId = Convert.ToInt32(reader["ProductoId"]),
                Producto = reader["Producto"].ToString(),
                Rinde = Convert.ToDecimal(reader["Rinde"]),
                Ranking = Convert.ToInt32(reader["Ranking"]),
                Total = Convert.ToInt32(reader["Total"])
            };
           
            if (reader["LugarId"] != DBNull.Value)
            {
                item.LugarId = Convert.ToInt32(reader["LugarId"]);
            }
            if (reader["CategoriaId"] != DBNull.Value)
            {
                item.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
            }
            if (reader["Categoria"] != DBNull.Value)
            {
                item.Categoria = reader["Categoria"].ToString();
            }
            if (reader["Provincia"] != DBNull.Value)
            {
                item.Provincia = reader["Provincia"].ToString();
            }
            if (reader["Departamento"] != DBNull.Value)
            {
                item.Departamento = reader["Departamento"].ToString();
            }
            if (reader["Localidad"] != DBNull.Value)
            {
                item.Localidad = reader["Localidad"].ToString();
            }
            if (reader["Fuente"] != DBNull.Value)
            {
                item.Fuente = reader["Fuente"].ToString();
            }
            if (reader["Establecimiento"] != DBNull.Value)
            {
                item.Establecimiento = reader["Establecimiento"].ToString();
            }
            if (reader["Archivo"] != DBNull.Value)
            {
                item.Archivo = reader["Archivo"].ToString();
            }
            if (reader["Observaciones"] != DBNull.Value)
            {
                item.Observaciones = reader["Observaciones"].ToString();
            }
            if (reader["FechaSiembra"] != DBNull.Value)
            {
                item.FechaSiembra = Convert.ToDateTime(reader["FechaSiembra"]);
            }
            if (reader["FechaCosecha"] != DBNull.Value)
            {
                item.FechaCosecha = Convert.ToDateTime(reader["FechaCosecha"]);
            }
            if (reader["Indice"] != DBNull.Value)
            {
                item.Indice = Convert.ToInt32(reader["Indice"]);
            }
            
            return item;
        }
    }
}
