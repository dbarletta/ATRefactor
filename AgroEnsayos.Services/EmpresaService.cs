using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Services
{
    public static class EmpresaService
    {
        public static List<Empresa> Get(int empresaId = 0, int categoriaId = 0)
        {
            List<Empresa> items = new List<Empresa>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Empresas_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@empresaId", empresaId);
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
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


        private static Empresa Load(SqlDataReader reader)
        {
            Empresa item = new Empresa
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString()
            };

            if (reader["domicilio"] != DBNull.Value)
                item.Domicilio = reader["domicilio"].ToString();
            if (reader["codigo_postal"]!= DBNull.Value)
                item.CodigoPostal = reader["codigo_postal"].ToString();
            if (reader["email"]!= DBNull.Value)
                item.Email = reader["email"].ToString();
            if (reader["fax"]!= DBNull.Value)
                item.Fax = reader["fax"].ToString();
            if (reader["telefono"]!= DBNull.Value)
                item.Telefono = reader["telefono"].ToString();
            if (reader["localidad"]!= DBNull.Value)
                item.Localidad = reader["localidad"].ToString();
            if (reader["pais"]!= DBNull.Value)
                item.Pais = reader["pais"].ToString();
            if (reader["url_logo"]!= DBNull.Value)
                item.Logo = reader["url_logo"].ToString();
                

            return item;
        }

    }
}
