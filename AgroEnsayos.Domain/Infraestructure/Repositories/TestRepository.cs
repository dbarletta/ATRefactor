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
        List<Test> Lookup(TestSearchParamsDto searchParams, int skip, int take, string orderby, string thenby);
        int LookupCount(TestSearchParamsDto searchParams);

        List<string> GetSources();
    }

    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(IDataContextFactory factory)
            : base(factory)
        {

        }

        public List<Test> Lookup(TestSearchParamsDto searchParams, int skip, int take, string orderBy, string thenBy)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                //Predicate and conditions
                var query = ApplyConditions(ctx.Tests, searchParams.CategoryId, searchParams.Companies, searchParams.Sources, searchParams.Provinces, searchParams.Localities, searchParams.Campaigns, searchParams.AttributesPredicate, searchParams.SearchTerm);

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

        public int LookupCount(TestSearchParamsDto searchParams)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                //Predicate and conditions
                var query = ApplyConditions(ctx.Tests, searchParams.CategoryId, searchParams.Companies, searchParams.Sources, searchParams.Provinces, searchParams.Localities, searchParams.Campaigns, searchParams.AttributesPredicate, searchParams.SearchTerm);

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




        private IQueryable<Test> OrderByThenBy(IQueryable<Test> tests, string sortBy, string thenBy)
        {
            IOrderedQueryable<Test> ordered = null;

            switch (sortBy)
            {
                case "fuente": ordered = tests.OrderBy(t => t.Source); break;
                case "campana": ordered = tests.OrderBy(t => t.Campaign.Name); break;
                case "producto": ordered = tests.OrderBy(t => t.Product.Name); break;
                case "provincia": ordered = tests.OrderBy(t => t.Place.Province); break;
                case "localidad": ordered = tests.OrderBy(t => t.Place.Locality); break;
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

                case "fuente": tests = ordered.ThenByDescending(t => t.Source); break;
                case "campanaDesc": tests = ordered.ThenByDescending(t => t.Campaign.Name); break;
                case "productoDesc": tests = ordered.ThenByDescending(t => t.Product.Name); break;
                case "provinciaDesc": tests = ordered.ThenByDescending(t => t.Place.Province); break;
                case "localidadDesc": tests = ordered.ThenByDescending(t => t.Place.Locality); break;
                case "rindeDesc": tests = ordered.ThenByDescending(t => t.Yield); break;

                default: tests = ordered.ThenByDescending(t => t.Yield); break;
            }

            return tests;

            #region CommentedOut
            //switch (sortBy)
            //{
            //    case "campanaAsc":
            //        switch (thenBy)
            //        {
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenBy(s => s.Campaign);
            //                break;
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Campaign.Name);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Campaign.Name);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Campaign.Name);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenBy(s => s.Campaign);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Campaign);
            //                break;
            //        }
            //        break;
            //    case "productoAsc":
            //        switch (thenBy)
            //        {
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenBy(s => s.Product.Name);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenBy(s => s.Product.Name);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Product.Name);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Product.Name);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenBy(s => s.Product.Name);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Product.Name);
            //                break;
            //        }
            //        break;
            //    case "fuenteAsc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Source);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenBy(s => s.Source);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Source);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Source);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenBy(s => s.Source);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Source);
            //                break;
            //        }
            //        break;
            //    case "provinciaAsc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Place.Province);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenBy(s => s.Place.Province);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenBy(s => s.Place.Province);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Place.Province);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenBy(s => s.Place.Province);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Place.Province);
            //                break;
            //        }
            //        break;
            //    case "localidadAsc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Place.Locality);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenBy(s => s.Place.Locality);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenBy(s => s.Place.Locality);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Place.Locality);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenBy(s => s.Place.Locality);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Place.Locality);
            //                break;
            //        }
            //        break;
            //    case "rindeAsc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Yield);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenBy(s => s.Yield);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenBy(s => s.Yield);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Yield);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Yield);
            //                break;
            //            default:
            //                tests = tests.OrderBy(s => s.Yield);
            //                break;
            //        }
            //        break;

            //    case "campanaDesc":
            //        switch (thenBy)
            //        {
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenByDescending(s => s.Campaign);
            //                break;
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Campaign);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Campaign);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Campaign);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Campaign);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Campaign);
            //                break;
            //        }
            //        break;
            //    case "productoDesc":
            //        switch (thenBy)
            //        {
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenByDescending(s => s.Product.Name);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Product.Name);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Product.Name);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Product.Name);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Product.Name);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Product.Name);
            //                break;
            //        }
            //        break;
            //    case "fuenteDesc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Source);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Source);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Source);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Source);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Source);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Source);
            //                break;
            //        }
            //        break;
            //    case "provinciaDesc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Place.Province);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Place.Province);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenByDescending(s => s.Place.Province);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Place.Province);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Place.Province);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Place.Province);
            //                break;
            //        }
            //        break;
            //    case "localidadDesc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Place.Locality);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Place.Locality);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenByDescending(s => s.Place.Locality);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Place.Locality);
            //                break;
            //            case "rinde":
            //                tests = tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Place.Locality);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Place.Locality);
            //                break;
            //        }
            //        break;
            //    case "rindeDesc":
            //        switch (thenBy)
            //        {
            //            case "producto":
            //                tests = tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Yield);
            //                break;
            //            case "campana":
            //                tests = tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Yield);
            //                break;
            //            case "fuente":
            //                tests = tests.OrderBy(s => s.Source).ThenByDescending(s => s.Yield);
            //                break;
            //            case "provincia":
            //                tests = tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Yield);
            //                break;
            //            case "localidad":
            //                tests = tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Yield);
            //                break;
            //            default:
            //                tests = tests.OrderByDescending(s => s.Yield);
            //                break;
            //        }
            //        break;

            //}

            //return tests; 
            #endregion
        }

        private IQueryable<Test> ApplyConditions(IQueryable<Test> tests, int categoryId, IEnumerable<int> companies, IEnumerable<string> sources, IEnumerable<string> provinces, IEnumerable<string> localities, IEnumerable<int> campaigns, Expression<Func<Test, bool>> attributeFiltersCondition, string searchTerm)
        {
            var query = tests.Where(test => test.Product.CategoryId == categoryId 
                                         && (!companies.Any() || companies.Contains(test.Product.CompanyId))
                                         && (!sources.Any() || sources.Contains(test.Source))
                                         && (!provinces.Any() || provinces.Contains(test.Place.Province))
                                         && (!localities.Any() || localities.Contains(test.Place.Locality))
                                         && (!campaigns.Any() || (campaigns.Contains(test.CampaignId)) && test.Campaign.CategoryId == categoryId))
                             .Where(attributeFiltersCondition);

            return query;
        }

    }
}
