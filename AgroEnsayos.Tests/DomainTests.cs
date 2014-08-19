using System;
using AgroEnsayos.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using System.Data;
using System.Collections.Generic;
using AgroEnsayos.Domain.Entities;
using System.Data.SqlClient;

namespace AgroEnsayos.Tests
{
    [TestClass]
    public class DomainTests
    {
        private IDataContextFactory factory = new EFDataContextFactory();

        [TestMethod]
        public void ProductsTest()
        {


            using (var db = new DbAgrotool())
            {
                var q = db.Products
                          .Where(p => p.Category.Name.Equals("Maiz", StringComparison.InvariantCultureIgnoreCase) && p.Cycle.Contains("Corto"))
                          .Include(p => p.Category)
                          .Include(p => p.AttributeMappings);

                var result = q.ToList();

            }
        }

        [TestMethod]
        public void AttributeRepositoryTest()
        {
            var repo = new AttributeRepository(factory);
            
            var all = repo.GetAll();
            var one = repo.Single(x => x.Id == 3);
            var query = repo.Get(x => x.Family.Contains("Tecno"));

        }

        [TestMethod]
        public void PabloTest()
        {
            using (var ctx = new DbAgrotool())
            {
                var q = from p in ctx.Products
                        join c in ctx.Companies on p.CompanyId equals c.Id
                        where p.Category.Name.Equals("Soja", StringComparison.InvariantCultureIgnoreCase)
                            && p.Cycle.ToLower().Contains("cor")
                        orderby p.Id descending
                        select new { p.Id, p.Name };

                var res = q.ToList();
            }
        }

        [TestMethod]
        public void StoredProcedureTest()
        {
            //method params
            int categoriaId = 4;
            string searchTerm = "";
            string empresa = "";
            string fuente = "";
            string provincia = "";
            string localidad = "";
            string campana = "";
            List<string> cond_atributo = null;
            
            //table value
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

            using (var ctx = new DbAgrotool())
            {
                var result = ctx.Database.SqlQuery<Test>("EXEC Ensayos_Lookup @categoriaId, @searchTerm, @empresa, @fuente, @provincia, @localidad, @campana, @atributoTbl", 
                    new SqlParameter("@categoriaId", categoriaId),
                    new SqlParameter("@searchTerm", searchTerm),
                    new SqlParameter("@empresa", empresa),
                    new SqlParameter("@fuente", fuente),
                    new SqlParameter("@provincia", provincia),
                    new SqlParameter("@localidad", localidad),
                    new SqlParameter("@campana", campana), 
                    new SqlParameter("@atributoTbl", dt) { TypeName = "AtributoEquivalenciasType" });

                var r = result.ToList();
            }
        }

        [TestMethod]
        public void RepoTest()
        {
            var _userRepository = new UserRepository(new EFDataContextFactory());

            var user = _userRepository.Single(u => u.Name == "admin", inc => inc.CategoriesOfIntrest)
                                      .CategoriesOfIntrest
                                      .Select(c => c.Id);
        }

    }
}
