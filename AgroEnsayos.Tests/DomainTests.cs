using System;
using AgroEnsayos.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;

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

    }
}
