using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Models;
using AgroEnsayos.Helpers;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Entities;
using AutoMapper;
using System.Linq.Expressions;
using AgroEnsayos.Domain.Entities.Dto;

namespace AgroEnsayos.Controllers
{
    public class EnsayosController : Controller
    {
        private ITestRepository _testRepository = null;
        private ICategoryRepository _categoryRepository = null;
        private IAttributeRepository _attributeRepository = null;
        private ICompanyRepository _companyRepository = null;
        private ICampaignRepository _campaignRepository = null;
        private IPlaceRepository _placeRepository = null;
        private IProductRepository _productRepository = null;

        public EnsayosController()
        {
            var ctxFactory = new EFDataContextFactory();
            _testRepository = new TestRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
            _attributeRepository = new AttributeRepository(ctxFactory);
            _companyRepository = new CompanyRepository(ctxFactory);
            _campaignRepository = new CampaignRepository(ctxFactory);
            _placeRepository = new PlaceRepository(ctxFactory);
            _productRepository = new ProductRepository(ctxFactory);
        }

        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.Ensayos = _testRepository.Get(t => t.Product.CategoryId == 4).ToList();
            return View();
        }

        [Authorize()]
        public JsonResult GetPageEnsayos(int lastRowId, bool isHistoryBack, List<string> list_filters = null, string newSort = "", string oldSort = "", string select_agroup = "", int CategoriaIdEnsayos = 0, string BuscarEnsayos = "", int ProductoId = 0, string FiltroFuente = "", string FiltroCampana = "", string FiltroCampanaId = "", string FiltroLocalidad = "")
        {
            var skip = lastRowId;
            var take = 30;

            var searchParams = new TestsSearchParams(_attributeRepository, list_filters, CategoriaIdEnsayos, BuscarEnsayos, FiltroFuente, FiltroCampana, FiltroCampanaId, FiltroLocalidad);
            var searchParamsDto = Mapper.Map<TestSearchParamsDto>(searchParams);

            var tests = _testRepository.Lookup(searchParamsDto, skip, take, select_agroup, newSort, _attributeRepository);

            var dto = Mapper.Map<List<TestDto>>(tests);
            return Json(dto);
        }

        [Authorize()]
        public ActionResult ResultadosEnsayos(SearchModel model, string new_id_filter = "", string new_val_filter = "", string new_name_filter = "", List<string> list_filters = null, int remove_id_filter = -1, string newSort = "", string oldSort = "", string select_agroup = "", int CategoriaIdEnsayos = 0, string BuscarEnsayos = "", int ProductoId = 0, string FiltroFuente = "", string FiltroCampana = "", string FiltroCampanaId = "", string FiltroLocalidad = "")
        {
            model.BuscarEnsayos = BuscarEnsayos;
            model.CategoriaIdEnsayos = CategoriaIdEnsayos;
            //////// Borrar Filtros ////////////////////
            if (list_filters != null && remove_id_filter >= 0 && remove_id_filter != 9999)
            {
                if (list_filters.Count >= remove_id_filter)
                {
                    list_filters.RemoveAt(remove_id_filter);
                }
            }
            if (list_filters != null && remove_id_filter == 9999)
            {
                list_filters.Clear();
            }

            if (FiltroFuente != "" && FiltroCampana != "")
            {
                if (list_filters != null)
                {
                    list_filters.Clear();
                }
                model.BuscarEnsayos = "";
                list_filters = new List<string>();
                list_filters.Add("Fuente" + "--" + FiltroFuente + "--" + FiltroFuente);
                list_filters.Add("Campana" + "--" + FiltroCampanaId + "--" + FiltroCampana);
                list_filters.Add("Localidad" + "--" + FiltroLocalidad + "--" + FiltroLocalidad);
            }

            //////////// Agregar Filtro //////////////
            if (new_id_filter != null && new_id_filter != "" && remove_id_filter < 0)
            {
                //si no existe el filtro, se crea la lista
                if (list_filters == null)
                {
                    list_filters = new List<string>();
                }
                // No duplica filtros
                if (list_filters.IndexOf(new_name_filter + "--" + new_id_filter + "--" + new_val_filter) < 0)
                {
                    //Agrega filtro nuevo
                    list_filters.Add(new_name_filter + "--" + new_id_filter + "--" + new_val_filter);
                }
            }

            string cond_empresa = "";
            string cond_fuente = "";
            string cond_provincia = "";
            string cond_localidad = "";
            string cond_campana = "";
            List<string> list_atributo = new List<string>();

            string strRubro = "";
            string strId = "";
            string strValor = "";

            int tam = 0;
            int tam_aux = 0;

            //Parsear filtro y sacar datos para armar la condicion
            if (list_filters != null && list_filters.Count > 0)
            {
                foreach (String strFilter in list_filters)
                {
                    tam = strFilter.IndexOf("--");
                    strRubro = strFilter.Substring(0, tam);

                    tam_aux = tam + 2;
                    tam = strFilter.IndexOf("--", tam_aux);
                    strId = strFilter.Substring(tam_aux, tam - tam_aux);

                    tam_aux = tam + 2;
                    strValor = strFilter.Substring(tam_aux);

                    if (strRubro == "Empresa")
                    {
                        cond_empresa = cond_empresa + "," + strId;
                    }
                    else if (strRubro == "Provincia")
                    {
                        cond_provincia = cond_provincia + "," + strId;
                    }
                    else if (strRubro == "Localidad")
                    {
                        cond_localidad = cond_localidad + "," + strId;
                    }
                    else if (strRubro == "Campana")
                    {
                        cond_campana = cond_campana + "," + strId;
                    }
                    else if (strRubro == "Fuente")
                    {
                        cond_fuente = cond_fuente + "," + strId;
                    }
                    else
                    {
                        list_atributo.Add(strId + "--" + strValor);
                    }
                }
            }

            if (cond_empresa != "")
                cond_empresa = cond_empresa + ",";
            if (cond_provincia != "")
                cond_provincia = cond_provincia + ",";
            if (cond_fuente != "")
                cond_fuente = cond_fuente + ",";
            if (cond_campana != "")
                cond_campana = cond_campana + ",";
            if (cond_localidad != "")
                cond_localidad = cond_localidad + ",";

            ///////////// Obtener Productos x Filtros /////////////


            //if (ProductoId != 0)
            //{
            //    model.Tests = _testRepository.Get(t => t.Product.CategoryId == model.CategoriaIdEnsayos && t.ProductId == ProductoId,
            //                                        i => i.Product);
            //}
            //else
            //{
            //    model.Tests = _testRepository.Lookup(model.CategoriaIdEnsayos, model.BuscarEnsayos, cond_empresa, cond_fuente, cond_provincia, cond_localidad, cond_campana, list_atributo);
            //}


            if (model.Tests != null && model.Tests.Count > 0)
            {
                model.Category = _categoryRepository.Single(c => c.Id == model.CategoriaIdEnsayos).Name;
            }
            else
            { model.Category = "Trigo"; }

            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////

            if (newSort != "" && model.Tests != null)
            {
                oldSort = newSort;
            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Tests != null)
            {
                model.Tests = model.Tests.OrderByDescending(s => s.Yield).ToList<Test>();
                oldSort = "rindeDesc";
            }
            //////////////////////////////////////////////////////////////

            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = _attributeRepository.GetFilters(model.CategoriaIdEnsayos);
            ViewBag.Empresas = _companyRepository.Get(c => !c.IsDisabled && c.Products.Select(p => p.CategoryId).Distinct().Contains(model.CategoriaIdEnsayos));
            ViewBag.Provincias = _placeRepository.GetProvincesWithTests(model.CategoriaIdEnsayos);
            ViewBag.Localidades = _placeRepository.GetLocalitiesWithTests(model.CategoriaIdEnsayos);
            ViewBag.Fuentes = _testRepository.GetSources();
            ViewBag.Campanas = _campaignRepository.Get(c => c.CategoryId == model.CategoriaIdEnsayos);

            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;


            var searchParams = new TestsSearchParams(_attributeRepository, list_filters, CategoriaIdEnsayos, BuscarEnsayos, FiltroFuente, FiltroCampana, FiltroCampanaId, FiltroLocalidad);
            var searchParamsDto = Mapper.Map<TestSearchParamsDto>(searchParams);

            model.TestsCount = _testRepository.LookupCount(searchParamsDto, _attributeRepository);

            return View(model);
        }

        [Authorize()]
        [HttpGet()]
        public string GetChartData(int categoriaId, int campanaId, int lugarId, string fuente)
        {
            var tests = _testRepository.GetChartData(categoriaId, campanaId, lugarId, fuente);
            var dtos = Mapper.Map<List<TestDto>>(tests);

            return dtos.ToJson();
        }

    }

    public class TestsSearchParams : SearchParams
    {
        public TestsSearchParams(IAttributeRepository attributeRepo, List<string> list_filters = null, int CategoriaIdEnsayos = 0, string BuscarEnsayos = "", string FiltroFuente = "", string FiltroCampana = "", string FiltroCampanaId = "", string FiltroLocalidad = "")
        {
            string cond_empresa = "";
            string cond_fuente = "";
            string cond_provincia = "";
            string cond_localidad = "";
            string cond_campana = "";
            List<string> list_atributo = new List<string>();

            string strRubro = "";
            string strId = "";
            string strValor = "";

            int tam = 0;
            int tam_aux = 0;

            //Parsear filtro y sacar datos para armar la condicion
            if (list_filters != null && list_filters.Count > 0)
            {
                foreach (String strFilter in list_filters)
                {
                    tam = strFilter.IndexOf("--");
                    strRubro = strFilter.Substring(0, tam);

                    tam_aux = tam + 2;
                    tam = strFilter.IndexOf("--", tam_aux);
                    strId = strFilter.Substring(tam_aux, tam - tam_aux);

                    tam_aux = tam + 2;
                    strValor = strFilter.Substring(tam_aux);

                    if (strRubro == "Empresa")
                    {
                        cond_empresa = cond_empresa + "," + strId;
                    }
                    else if (strRubro == "Provincia")
                    {
                        cond_provincia = cond_provincia + "," + strId;
                    }
                    else if (strRubro == "Localidad")
                    {
                        cond_localidad = cond_localidad + "," + strId;
                    }
                    else if (strRubro == "Campana")
                    {
                        cond_campana = cond_campana + "," + strId;
                    }
                    else if (strRubro == "Fuente")
                    {
                        cond_fuente = cond_fuente + "," + strId;
                    }
                    else
                    {
                        list_atributo.Add(strId + "--" + strValor);
                    }
                }
            }

            if (cond_empresa != "")
                cond_empresa = cond_empresa + ",";
            if (cond_provincia != "")
                cond_provincia = cond_provincia + ",";
            if (cond_fuente != "")
                cond_fuente = cond_fuente + ",";
            if (cond_campana != "")
                cond_campana = cond_campana + ",";
            if (cond_localidad != "")
                cond_localidad = cond_localidad + ",";

            this.CategoryId = CategoriaIdEnsayos;
            this.Companies = SplitIntegers(cond_empresa);
            this.Sources = SplitStrings(cond_fuente);
            this.Provinces = SplitStrings(cond_provincia);
            this.Localities = SplitStrings(cond_localidad);
            this.Campaigns = SplitIntegers(cond_campana);
            this.SearchTerm = BuscarEnsayos;

            this.AttributeFilters = SplitAttributes(list_atributo);

            
        }


        public int CategoryId { get; set; }

        public IEnumerable<int> Companies { get; private set; }
        public IEnumerable<string> Sources { get; private set; }
        public IEnumerable<string> Provinces { get; private set; }
        public IEnumerable<string> Localities { get; private set; }
        public IEnumerable<int> Campaigns { get; private set; }
        public string SearchTerm { get; private set; }

        public Dictionary<int, string> AttributeFilters { get; private set; }

    }

    public abstract class SearchParams
    {
        protected string[] SplitStrings(string param)
        {
            return param.Trim(',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected IEnumerable<int> SplitIntegers(string param)
        {
            return param.Trim(',')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse);
        }

        protected Dictionary<int, string> SplitAttributes(List<string> parameters)
        {
            Dictionary<int, string> attrs = new Dictionary<int, string>();

            if (parameters != null)
            {
                string[] keyValue = new string[2];
                foreach (var a in parameters)
                {
                    keyValue = a.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
                    attrs.Add(Convert.ToInt32(keyValue[0]), keyValue[1]);
                }
            }

            return attrs;
        }
    }
}
