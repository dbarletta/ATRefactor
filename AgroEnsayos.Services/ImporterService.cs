using AgroEnsayos.Entities;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Services
{
    public static class ImporterService
    {
        public static object IsUploaded()
        {
            bool Empty = true;
            string filename = null;
             DirectoryInfo di = new DirectoryInfo(Configuration.ImportersPath);
             foreach (var fileInfo in di.GetFiles())
             {
                 Empty = false;
                 filename = string.Format("Archivo actual en la carpeta: {0} , Modificado el {1}", fileInfo.Name, fileInfo.LastAccessTimeUtc);
             }
             if (filename == null)
                 filename = "No hay ningun archivo excel cargado";
             return new {IsEmpty = Empty, Name = filename };
        }


        public static void TruncateTemp()
        {
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("temp_ensayos_truncate", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<ImporterResult> ImportEnsayosStep1() //verifica si el formato de los headers es el correcto
        {
            List<ImporterResult> importerResults = new List<ImporterResult>();
            int count = 0;
            int rowCount = 0;
            int tableCount = 0;
            string validHeader = "provincia|localidad||establecimiento|campaña|fecha siembra|fecha cosecha|producto||rinde|indice %|quebrado|vuelco|altura de la planta|humedad|espromedio?|plantas x hectárea|días a floración (integer)|emergencia a floración|observaciones|archivo asociado|";
            IExcelDataReader excelReader = null;
            try
            {
                //Check Prerequisites
                //1. Chequeo que esté el archivo
                string sourceDirectoryPath = Configuration.ImportersPath;

                DirectoryInfo di = new DirectoryInfo(sourceDirectoryPath);
                FileInfo[] files = di.GetFiles("*.xls*", SearchOption.TopDirectoryOnly);

                if (files.Length == 0)
                {
                    importerResults.Add(new ImporterResult
                    {
                        TipoError = TipoError.SinArchivo,
                        Severity = Entities.Severity.Error,
                        Description = "No hay archivos a importar en el servidor.",
                    });
                }

                foreach (FileInfo info in files)
                {
                    FileStream stream = File.Open(info.FullName, FileMode.Open, FileAccess.Read);

                    if (info.Extension.Equals(".xls"))
                    {
                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (info.Extension.Equals(".xlsx"))
                    {
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    excelReader.IsFirstRowAsColumnNames = false;
                    DataSet result = excelReader.AsDataSet();
                    for (int t = 0; t < result.Tables.Count; t++)
                    {
                        tableCount = t;
                        rowCount = 0;
                        string header = string.Empty;

                        for (int c = 1; c < 22; c++)
                        {
                            header += result.Tables[t].Rows[rowCount][c].ToString().Trim().ToLower() + "|";
                        }

                        header = header.Replace('\n', ' ');

                        if (!header.Equals(validHeader))
                        {
                            count++;
                            importerResults.Add(new ImporterResult
                            {
                                TipoError = TipoError.FormatoIncorrecto,
                                Severity = Entities.Severity.Error,
                                Description = (string.Format("La planilla {0} no tiene el formato correcto!. Verificar el nombre de las columnas del encabezado,", info.Name) + Environment.NewLine + String.Format("Formato adecuado: {0}" , validHeader)),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                count++;

                importerResults.Add(new ImporterResult
                {
                    Severity = Entities.Severity.Error,
                    Description = ex.Message,
                });
            }
            finally
            {
                //7. Free resources (IExcelDataReader is IDisposable)
                if (excelReader != null)
                {
                    excelReader.Close();
                }
            }

            return importerResults; // si el count es 0 el header esta bien, sino especifica que tipo de error fue.
        }

        public static List<ImporterResult> ImportEnsayosStep2() //Crea tabla temporal e importar datos
        {
            List<ImporterResult> errors = new List<ImporterResult>();

            int count = 0;
            int rowCount = 0;
            int tableCount = 0;
            int columnCount = 0;
            IExcelDataReader excelReader = null;
            try
            {
                string sourceDirectoryPath = Configuration.ImportersPath;

                DirectoryInfo di = new DirectoryInfo(sourceDirectoryPath);
                FileInfo[] files = di.GetFiles("*.xls*", SearchOption.TopDirectoryOnly);

                

                DataTable sourceTable = new DataTable();
                sourceTable.Columns.Add(new DataColumn("Fuente", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Provincia", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Localidad", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Mal", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Establecimiento", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Campana", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("FechaSiembra", typeof(DateTime)));
                sourceTable.Columns.Add(new DataColumn("FechaCosecha", typeof(DateTime)));
                sourceTable.Columns.Add(new DataColumn("Producto", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("ColumnaVacia", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Rinde", typeof(float)));
                sourceTable.Columns.Add(new DataColumn("Indice", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Quebrado", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Vuelco", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("AlturaPlanta", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Humedad", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("EsPromedio", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("PlantasXHectarea", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("DiasFloracion", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("EmergenciaFloracion", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Observaciones", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Archivo", typeof(string)));
                sourceTable.Columns.Add(new DataColumn("Row", typeof(string)));

                foreach (FileInfo info in files)
                {
                    FileStream stream = File.Open(info.FullName, FileMode.Open, FileAccess.Read);

                    if (info.Extension.Equals(".xls"))
                    {
                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (info.Extension.Equals(".xlsx"))
                    {
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    excelReader.IsFirstRowAsColumnNames = false;
                    DataSet xlsResults = excelReader.AsDataSet();

                    for (int t = 0; t < xlsResults.Tables.Count; t++)
                    {
                        tableCount = t;

                        for (int r = 1; r < xlsResults.Tables[t].Rows.Count; r++)
                        {
                            rowCount = r + 1;
                            DataRow row = sourceTable.NewRow();

                            for (int c = columnCount; c <= 21; c++)
                            {
                                if (c == 6 || c == 7)  //Si viene con formato fecha en excel, lo cambio
                                {
                                    string stringDate = (xlsResults.Tables[t].Rows[r][c]).ToString();
                                    if (!string.IsNullOrEmpty(stringDate))
                                    {
                                        double doubleDate = double.Parse(stringDate);
                                        row[c] = DateTime.FromOADate(doubleDate);
                                    }
                                    else
                                    {
                                        row[c] = xlsResults.Tables[t].Rows[r][c];
                                    }
                                }
                                else
                                {
                                    row[c] = xlsResults.Tables[t].Rows[r][c];
                                }
                            }
                            row[22] = rowCount;

                            sourceTable.Rows.Add(row);
                        }
                    }
                }

                // new method: SQLBulkCopy:
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    conn.Open();
                    using (SqlBulkCopy s = new SqlBulkCopy(conn))
                    {
                        s.DestinationTableName = "temp_ensayos";
                        s.BulkCopyTimeout = 120;
                        s.WriteToServer(sourceTable);
                        s.Close();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                count++;
                ImporterResult error = new ImporterResult
               {
                   Description = string.Format("Error: {0}. Fila {1}, Columna {2}", ex.Message, rowCount, columnCount),
                   TipoError = TipoError.FormatoIncorrecto,
                   Severity = Severity.Error,
               };
                errors.Add(error);
            }
            finally
            {
                //7. Free resources (IExcelDataReader is IDisposable)
                if (excelReader != null)
                {
                    excelReader.Close();
                }

            }
            return errors;
        }

        public static List<ImporterResult> ImportEnsayosStep3(int categoriaId) //verificacion de errores de la tabla temporal
        {
            List<ImporterResult> errores = new List<ImporterResult>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("temp_ensayos_Validations", conn))
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
                                errores.Add(Load(reader));
                            }
                        }

                        reader.Close();
                    }
                }
            }
            //var errorsNotAllNulls = (List<ImporterResult>)errores.Where(e => e.Description != "Alguno de los datos obligatorios está faltando (Fuente, Provincia, Localidad, Campaña, Producto o Rinde)");
            //return errorsNotAllNulls;
            return errores;
        }

        public static object ImportEnsayosStep4(int categoriaId) //Hacer UPSERT en tabla Ensayos
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("temp_ensayos_UpsertExcel", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@categoriaId", categoriaId);
                        command.CommandTimeout = 120;
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }

                //Muevo los archivos al folder de archive

                DirectoryInfo di = new DirectoryInfo(Configuration.ImportersPath);
                foreach (var fileInfo in di.GetFiles("*.xls*", SearchOption.TopDirectoryOnly))
                {
                    //System.IO.File.Move(fileInfo.FullName.ToString(), di.FullName + "\\" + fileInfo.Name.Replace(" ", "") + DateTime.Now.ToString() + fileInfo.Extension);
                    string oldFolder = Path.Combine(fileInfo.Directory.FullName, "archive");
                    string newFile = Path.Combine(oldFolder, fileInfo.Name);

                    if (System.IO.File.Exists(newFile))
                    {
                        fileInfo.Delete();
                    }
                    else
                    {
                        fileInfo.MoveTo(newFile);
                    }
                }
            }
            catch (Exception ex)
            {
                return new {success= false, description = ex.Message,};
            }
            return new { success = true, description = "ok"};
            
        }

        public static ImporterResult Load(SqlDataReader reader)
        {
            try
            {
                ImporterResult item = new ImporterResult
                {
                    Description = reader["Description"].ToString(),
                    Param1 = reader["Param1"].ToString(),
                    Param2 = reader["Param2"].ToString(),
                    Param3 = reader["Param3"].ToString(),
                    Param4 = reader["Param4"].ToString(),
                    Param5 = reader["Param5"].ToString(),
                    Row = Convert.ToInt32(reader["Row"]),
                    TipoError = (TipoError)Convert.ToInt32(reader["TipoError"]),
                    Severity = (Severity)Convert.ToInt32(reader["Severity"]),
                };

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
