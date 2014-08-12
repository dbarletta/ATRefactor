using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class CategoriaService
    {
        public static List<Categoria> Get()
        {
            List<Categoria> items = new List<Categoria>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Categorias_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
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

        public static Categoria Load(SqlDataReader reader)
        {
            Categoria item = new Categoria
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString()
            };

            if (reader["Padre"] != DBNull.Value)
            {
                item.Padre = reader["Padre"].ToString();
            }
            if (reader["PadreId"] != DBNull.Value)
            {
                item.PadreId = Convert.ToInt32(reader["PadreId"]);
            }

            return item;
        }

    }
}
