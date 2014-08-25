using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Entities.Dto;
using AgroEnsayos.Domain.Infraestructure.EF;
using RefactorThis.GraphDiff;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface ITestRepository : IRepository<Test>
    {
        List<Test> Lookup(TestSearchParamsDto searchParams, int skip, int take, string orderby, string thenby, IAttributeRepository attributeRepo);
        int LookupCount(TestSearchParamsDto searchParams, IAttributeRepository attributeRepo);

        List<Test> GetChartData(int categoriaId, int campanaId, int lugarId, string fuente);

        List<string> GetSources();
    }

    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(IDataContextFactory factory)
            : base(factory)
        {

        }

        public List<Test> Lookup(TestSearchParamsDto searchParams, int skip, int take, string orderBy, string thenBy, IAttributeRepository attributeRepo)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                //Predicate and conditions
                var query = ApplyConditions(ctx.Tests, searchParams, attributeRepo);

                //Includes
                query = query.Include(t => t.Campaign)
                             .Include(t => t.Place)
                             .Include(t => t.Product);

                //Group and Sorting
                query = OrderByThenBy(query, orderBy, thenBy);

                //Paging
                query = query.Skip(skip)
                             .Take(take);

                //Execute
                var tests = query.ToList();

                return tests;
            }
        }

        public int LookupCount(TestSearchParamsDto searchParams, IAttributeRepository attributeRepo)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                //Predicate and conditions
                var query = ApplyConditions(ctx.Tests, searchParams, attributeRepo);

                return query.Count();
            }
        }

        public List<string> GetSources()
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                var tests = ctx.Tests.Select(t => t.Source)
                                     .Distinct()
                                     .ToList();

                return tests;
            }
        }
        
        public List<Test> GetChartData(int categoriaId, int campanaId, int lugarId, string fuente)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.Tests.Where(t => (categoriaId == 0 || t.Product.CategoryId == categoriaId)
                                              && (campanaId == 0 || t.CampaignId == campanaId)
                                              && (lugarId == 0 || t.PlaceId == lugarId)
                                              && (string.IsNullOrEmpty(fuente) || t.Source == fuente))
                                     .Include(t => t.Product);

                var top5 = query.OrderByDescending(t => t.Yield).Take(5);

                var last = query.OrderBy(t => t.Yield).First();
                last.Product.Name = "ULTIMO";

                var average = new Test()
                {
                    Product = new Product { Name = "PROMEDIO" },
                    Yield = query.Average(x => x.Yield)
                };

                var result = new List<Test>();

                result.AddRange(top5.ToList());
                result.Add(last);
                result.Add(average);

                return result;
            }
        }



        private IQueryable<Test> OrderByThenBy(IQueryable<Test> tests, string sortBy, string thenBy)
        {
            IOrderedQueryable<Test> ordered = null;

            if(string.IsNullOrEmpty(sortBy))
            {
                sortBy = thenBy.Replace("Asc", "");
            }

            switch (sortBy)
            {
                case "fuente": ordered = tests.OrderBy(t => t.Source); break;
                case "campana": ordered = tests.OrderBy(t => t.Campaign.Name); break;
                case "producto": ordered = tests.OrderBy(t => t.Product.Name); break;
                case "provincia": ordered = tests.OrderBy(t => t.Place.Province); break;
                case "localidad": ordered = tests.OrderBy(t => t.Place.Locality); break;
                case "rinde": ordered = tests.OrderBy(t => t.Yield); break;

                case "fuenteDesc": ordered = tests.OrderByDescending(t => t.Source); break;
                case "campanaDesc": ordered = tests.OrderByDescending(t => t.Campaign.Name); break;
                case "productoDesc": ordered = tests.OrderByDescending(t => t.Product.Name); break;
                case "provinciaDesc": ordered = tests.OrderByDescending(t => t.Place.Province); break;
                case "localidadDesc": ordered = tests.OrderByDescending(t => t.Place.Locality); break;
                case "rindeDesc": 
                default: ordered = tests.OrderByDescending(t => t.Yield); break;
            }

            switch (thenBy)
            {
                case "fuenteAsc": tests = ordered.ThenBy(t => t.Source); break;
                case "campanaAsc": tests = ordered.ThenBy(t => t.Campaign.Name); break;
                case "productoAsc": tests = ordered.ThenBy(t => t.Product.Name); break;
                case "provinciaAsc": tests = ordered.ThenBy(t => t.Place.Province); break;
                case "localidadAsc": tests = ordered.ThenBy(t => t.Place.Locality); break;
                case "rindeAsc": tests = ordered.ThenBy(t => t.Yield); break;

                case "fuenteDesc": tests = ordered.ThenByDescending(t => t.Source); break;
                case "campanaDesc": tests = ordered.ThenByDescending(t => t.Campaign.Name); break;
                case "productoDesc": tests = ordered.ThenByDescending(t => t.Product.Name); break;
                case "provinciaDesc": tests = ordered.ThenByDescending(t => t.Place.Province); break;
                case "localidadDesc": tests = ordered.ThenByDescending(t => t.Place.Locality); break;
                case "rindeDesc": tests = ordered.ThenByDescending(t => t.Yield); break;

                default: tests = ordered.ThenByDescending(t => t.Yield); break;
            }

            return tests;
        }

        private IQueryable<Test> ApplyConditions(IQueryable<Test> tests, TestSearchParamsDto searchParams, IAttributeRepository attributeRepo)
        {
            //attribute filters
            var attributesPredicate = PredicateBuilder.True<Test>();
            foreach (var attr in searchParams.AttributeFilters)
            {
                var mappingIds = attributeRepo.Single(a => a.Id == attr.Key).AttributeMappings.Where(m => m.MappedValue == attr.Value).Select(m => m.AttributeMappingId).ToList();
                attributesPredicate = attributesPredicate.And(t => t.Product.AttributeMappings.Any(m => mappingIds.Contains(m.AttributeMappingId)));
            }

            //free text search
            var term = searchParams.SearchTerm;
            var searchTermPredicate = PredicateBuilder.False<Test>();
            searchTermPredicate = searchTermPredicate.Or(t => t.Product.Category.Name.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Product.Name.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Source.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Product.Company.Name.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Place.Province.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Place.Locality.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Place.Department.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Place.Header.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Place.Region.Contains(term));
            searchTermPredicate = searchTermPredicate.Or(t => t.Product.AttributeMappings.Select(a => a.Attribute.Name).Any(attName => attName.Contains(term)));
            searchTermPredicate = searchTermPredicate.Or(t => t.Product.AttributeMappings.Select(a => a.Attribute.Tags).Any(attTag => attTag.Contains(term)));

            //final query predicate
            var query = tests.Where(test => test.Product.CategoryId == searchParams.CategoryId
                                         && (!searchParams.Companies.Any() || searchParams.Companies.Contains(test.Product.CompanyId))
                                         && (!searchParams.Sources.Any() || searchParams.Sources.Contains(test.Source))
                                         && (!searchParams.Provinces.Any() || searchParams.Provinces.Contains(test.Place.Province))
                                         && (!searchParams.Localities.Any() || searchParams.Localities.Contains(test.Place.Locality))
                                         && (!searchParams.Campaigns.Any() || (searchParams.Campaigns.Contains(test.CampaignId)) && test.Campaign.CategoryId == searchParams.CategoryId))
                             .Where(attributesPredicate)
                             .Where(searchTermPredicate);

            return query;
        }
        
    }
}
