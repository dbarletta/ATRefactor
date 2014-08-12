using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AgroEnsayos.Entities;
using AgroEnsayos.Services;

namespace AgroEnsayos.Services
{
    public static class ProductoService
    {
        public static List<Producto> Get(int? categoriaId, bool todos = false, int productoId = 0)
        {
            List<Producto> items = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Productos_Get", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@productoId", productoId);
                    command.Parameters.AddWithValue("@todos", todos);
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

        public static List<Producto> Lookup(int categoriaId, string searchTerm, string empresa = "", string antiguedad = "", string region = "", List<string> cond_atributo = null,int limit_ini=0,int limit_fin=0)
        {
            List<Producto> items = new List<Producto>();
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
                using (SqlCommand command = new SqlCommand("Productos_Lookup", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@searchTerm", searchTerm);
                    command.Parameters.AddWithValue("@empresa", empresa);
                    command.Parameters.AddWithValue("@antiguedad", antiguedad);
                    command.Parameters.AddWithValue("@region", region);
                    command.Parameters.AddWithValue("@atributoTbl", dt);

                  //  command.Parameters.AddWithValue("@limit_ini", limit_ini);
                  //  command.Parameters.AddWithValue("@limit_fin", limit_fin);

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

        public static Producto Load(SqlDataReader reader)
        {
            Producto item = new Producto
            {
                Id = Convert.ToInt32(reader["Id"]),
                CategoriaId = Convert.ToInt32(reader["CategoriaId"]),
                Categoria = reader["Categoria"].ToString(),
                EmpresaId = Convert.ToInt32(reader["EmpresaId"]),
                Empresa = reader["Empresa"].ToString(),
                Nombre = reader["Nombre"].ToString()
            };

            if (reader["EsNuevo"] != DBNull.Value)
            {
                item.EsNuevo = Convert.ToBoolean(reader["EsNuevo"]);
            }
            if (reader["Alta"] != DBNull.Value)
            {
                item.Alta = Convert.ToInt32(reader["Alta"]);
            }
            if (reader["FechaCarga"] != DBNull.Value)
            {
                item.FechaCarga = Convert.ToDateTime(reader["FechaCarga"]);
            }

            if (reader["Material"] != DBNull.Value)
            {
                item.Material = reader["Material"].ToString();
            }
            if (reader["Ciclo"] != DBNull.Value)
            {
                item.Ciclo = reader["Ciclo"].ToString();
            }
            if (reader["EsHibrido"] != DBNull.Value)
            {
                item.EsHibrido = Convert.ToBoolean(reader["EsHibrido"]);
            }
            if (reader["EsConvencional"] != DBNull.Value)
            {
                item.EsConvencional = Convert.ToBoolean(reader["EsConvencional"]);
            }
            if (reader["DiasFloracion"] != DBNull.Value)
            {
                item.DiasFloracion = Convert.ToInt32(reader["DiasFloracion"]);
            }
            if (reader["DiasMadurez"] != DBNull.Value)
            {
                item.DiasMadurez = Convert.ToInt32(reader["DiasMadurez"]);
            }
            if (reader["AlturaPlanta"] != DBNull.Value)
            {
                item.AlturaPlanta = Convert.ToInt32(reader["AlturaPlanta"]);
            }
            if (reader["DescripcionPG"] != DBNull.Value)
            {
                item.DescripcionPG = reader["DescripcionPG"].ToString();
            }

            return item;
        }

        public static List<Producto> GetAll()
        {
            List<Producto> items = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Productos_TraerHabilitados", conn))
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

        public static void Add(Producto producto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Productos_Insert", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        cmd.Parameters.Add(new SqlParameter("@CategoriaId", producto.CategoriaId));
                        cmd.Parameters.Add(new SqlParameter("@EmpresaId", producto.EmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", producto.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@DescripcionPG", producto.DescripcionPG));
                        cmd.Parameters.Add(new SqlParameter("@Material", producto.Material));
                        cmd.Parameters.Add(new SqlParameter("@Ciclo", producto.Ciclo));
                        cmd.Parameters.Add(new SqlParameter("@DiasFloracion", producto.DiasFloracion));
                        cmd.Parameters.Add(new SqlParameter("@DiasMadurez", producto.DiasMadurez));
                        cmd.Parameters.Add(new SqlParameter("@AlturaPlanta", producto.AlturaPlanta));
                        if (producto.EsConvencional == null)
                            producto.EsConvencional = false;
                        cmd.Parameters.Add(new SqlParameter("@EsConvencional", producto.EsConvencional));
                        if (producto.EsHibrido == null)
                            producto.EsHibrido = false;
                        cmd.Parameters.Add(new SqlParameter("@EsHibrido", producto.EsHibrido));
                        if (producto.EsNuevo == null)
                            producto.EsNuevo = false;
                        cmd.Parameters.Add(new SqlParameter("@EsNuevo", producto.EsNuevo));
                        cmd.Parameters.Add(new SqlParameter("@Alta", producto.Alta ?? DateTime.Now.Year));
                        cmd.Parameters.Add(new SqlParameter("@FechaCarga", producto.FechaCarga ?? DateTime.Now));
                        cmd.Parameters.Add(new SqlParameter("@Deshabilitado", producto.Deshabilitado));

                        var idParameter = new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idParameter);
                        cmd.ExecuteNonQuery();
                        producto.Id = (int)idParameter.Value;

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Edit(Producto prod)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Productos_Edit", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        cmd.Parameters.Add(new SqlParameter("@CategoriaId", prod.CategoriaId));
                        cmd.Parameters.Add(new SqlParameter("@EmpresaId", prod.EmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", prod.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@DescripcionPG", prod.DescripcionPG));
                        cmd.Parameters.Add(new SqlParameter("@Material", prod.Material));
                        cmd.Parameters.Add(new SqlParameter("@EsHibrido", prod.EsHibrido));
                        cmd.Parameters.Add(new SqlParameter("@Ciclo", prod.Ciclo));
                        cmd.Parameters.Add(new SqlParameter("@EsConvencional", prod.EsConvencional));
                        cmd.Parameters.Add(new SqlParameter("@DiasFloracion", prod.DiasFloracion));
                        cmd.Parameters.Add(new SqlParameter("@DiasMadurez", prod.DiasMadurez));
                        cmd.Parameters.Add(new SqlParameter("@AlturaPlanta", prod.AlturaPlanta));
                        cmd.Parameters.Add(new SqlParameter("@EsNuevo", prod.EsNuevo));
                        cmd.Parameters.Add(new SqlParameter("@Alta", prod.Alta));
                        //cmd.Parameters.Add(new SqlParameter("@FechaCarga", prod.FechaCarga));
                        cmd.Parameters.Add(new SqlParameter("@Deshabilitado", prod.Deshabilitado));
                        cmd.Parameters.Add(new SqlParameter("@Id", prod.Id));

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DisableProduct(int Id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Productos_Delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveProductoLugares(List<ProductoLugares> lista, int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ProductoLugares_Delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        cmd.Parameters.Add(new SqlParameter("@productid", productId));
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    if (lista != null)
                    {
                        foreach (var pl in lista)
                        {

                            using (SqlCommand cmd = new SqlCommand("ProductoLugares_Edit", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                conn.Open();
                                cmd.Parameters.Add(new SqlParameter("@productid", productId));
                                cmd.Parameters.Add(new SqlParameter("@lugarid", pl.LugarId));
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveProductoAtributos(List<ProductoAtributo> pas)
        {
            if (pas != null && pas.Any())
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("ProductoAtributos_Delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        cmd.Parameters.Add(new SqlParameter("@ProductId", pas.First().ProductoId));
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    foreach (var pa in pas.Where(x => x.Valor != null))
                    {
                        using (SqlCommand cmd = new SqlCommand("ProductoAtributos_Insert", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            cmd.Parameters.Add(new SqlParameter("@ProductoId", pa.ProductoId));
                            cmd.Parameters.Add(new SqlParameter("@AtributoId", pa.AtributoId));
                            cmd.Parameters.Add(new SqlParameter("@Valor", pa.Valor));
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
        }



    }
}
