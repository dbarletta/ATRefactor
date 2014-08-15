using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using RefactorThis.GraphDiff;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface ITestRepository : IRepository<Test> 
    {
        List<Test> Lookup(int categoriaId, string searchTerm, string empresa = "", string fuente = "", string provincia = "", string localidad = "", string campana = "", List<string> cond_atributo = null);
    }

    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(IDataContextFactory factory)
            : base(factory)
        {

        }

        public List<Test> Lookup(int categoriaId, string searchTerm, string empresa = "", string fuente = "", string provincia = "", string localidad = "", string campana = "", List<string> cond_atributo = null)
        {
            List<Test> tests = null;

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

            using(var ctx = _factory.Create() as DbAgrotool)
            {
                var stored = ctx.Database.SqlQuery<Test>("EXEC Tests_Lookup @categoriaId, @searchTerm, @empresa, @fuente, @provincia, @localidad, @campana, @atributoTbl",
                    new SqlParameter("@categoriaId", categoriaId),
                    new SqlParameter("@searchTerm", searchTerm),
                    new SqlParameter("@empresa", empresa),
                    new SqlParameter("@fuente", fuente),
                    new SqlParameter("@provincia", provincia),
                    new SqlParameter("@localidad", localidad),
                    new SqlParameter("@campana", campana),
                    new SqlParameter("@atributoTbl", dt) { TypeName = "AtributoEquivalenciasType" });

                tests = stored.ToList();
            }

            return tests;
        }
    }
}
