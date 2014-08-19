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

namespace AgroEnsayos.Controllers
{
    public class EnsayosController : Controller
    {
        private ITestRepository _testRepository = null;
        private ICategoryRepository _categoryRepository = null;
        private IAttributeRepository _attributeRepository = null;
        private ICompanyRepository _companyRepository = null;
        private ICampaignRepository _campaignRepository = null;

        public EnsayosController()
        {
            var ctxFactory = new EFDataContextFactory();
            _testRepository = new TestRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
            _attributeRepository = new AttributeRepository(ctxFactory);
            _companyRepository = new CompanyRepository(ctxFactory);
            _campaignRepository = new CampaignRepository(ctxFactory);
        }

        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.Ensayos = _testRepository.Get(t => t.Product.CategoryId == 4).ToList();
            return View();
        }

        [Authorize()]
        public ActionResult GetPageEnsayos(int lastRowId, bool isHistoryBack, List<string> list_filters = null, string newSort = "", string oldSort = "", string select_agroup = "", int CategoriaIdEnsayos = 0, string BuscarEnsayos = "", int ProductoId = 0, string FiltroFuente = "", string FiltroCampana = "", string FiltroCampanaId = "", string FiltroLocalidad = "")
        {
            SearchModel model = new SearchModel();

            model.BuscarEnsayos = BuscarEnsayos;
            model.CategoriaIdEnsayos = CategoriaIdEnsayos;

            int limit_ini = lastRowId;
            int limit_fin = 30;

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

            ///////////// Obtener Ensayos x Filtros /////////////
            if (ProductoId != 0)
            {
                model.Tests = _testRepository.Get(t => t.Product.CategoryId == model.CategoriaIdEnsayos && t.ProductId == ProductoId);
            }
            else
            {
                model.Tests = _testRepository.Lookup(model.CategoriaIdEnsayos, model.BuscarEnsayos, cond_empresa, cond_fuente, cond_provincia, cond_localidad, cond_campana, list_atributo);
            }

            model.Category = "NN";
            List<Category> ar_c = _categoryRepository.GetAll();
            foreach (Category c in ar_c)
            {
                if (c.Id == model.CategoriaIdEnsayos)
                {
                    model.Category = c.Name;
                }
            }

            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////

            if (newSort != "" && model.Tests != null)
            {
                switch (newSort)
                {
                    case "campanaAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenBy(s => s.Campaign).ToList<Test>();
                                break;
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Campaign.Name).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Campaign.Name).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Campaign.Name).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenBy(s => s.Campaign).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ToList<Test>();
                                break;
                        }
                        break;
                    case "productoAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenBy(s => s.Product.Name).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenBy(s => s.Product.Name).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Product.Name).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Product.Name).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenBy(s => s.Product.Name).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ToList<Test>();
                                break;
                        }
                        break;
                    case "fuenteAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Source).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenBy(s => s.Source).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Source).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Source).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenBy(s => s.Source).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Source).ToList<Test>();
                                break;
                        }
                        break;
                    case "provinciaAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Place.Province).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenBy(s => s.Place.Province).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenBy(s => s.Place.Province).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Place.Province).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenBy(s => s.Place.Province).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ToList<Test>();
                                break;
                        }
                        break;
                    case "localidadAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenBy(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenBy(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenBy(s => s.Place.Locality).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ToList<Test>();
                                break;
                        }
                        break;
                    case "rindeAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenBy(s => s.Yield).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenBy(s => s.Yield).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenBy(s => s.Yield).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenBy(s => s.Yield).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenBy(s => s.Yield).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ToList<Test>();
                                break;
                        }
                        break;

                    case "campanaDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenByDescending(s => s.Campaign).ToList<Test>();
                                break;
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Campaign).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Campaign).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Campaign).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Campaign).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Campaign).ToList<Test>();
                                break;
                        }
                        break;
                    case "productoDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Product.Name).ToList<Test>();
                                break;
                        }
                        break;
                    case "fuenteDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Source).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Source).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Source).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Source).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Source).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Source).ToList<Test>();
                                break;
                        }
                        break;
                    case "provinciaDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Place.Province).ToList<Test>();
                                break;
                        }
                        break;
                    case "localidadDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                            case "rinde":
                                model.Tests = model.Tests.OrderBy(s => s.Yield).ThenByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Place.Locality).ToList<Test>();
                                break;
                        }
                        break;
                    case "rindeDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Tests = model.Tests.OrderBy(s => s.Product.Name).ThenByDescending(s => s.Yield).ToList<Test>();
                                break;
                            case "campana":
                                model.Tests = model.Tests.OrderBy(s => s.Campaign).ThenByDescending(s => s.Yield).ToList<Test>();
                                break;
                            case "fuente":
                                model.Tests = model.Tests.OrderBy(s => s.Source).ThenByDescending(s => s.Yield).ToList<Test>();
                                break;
                            case "provincia":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Province).ThenByDescending(s => s.Yield).ToList<Test>();
                                break;
                            case "localidad":
                                model.Tests = model.Tests.OrderBy(s => s.Place.Locality).ThenByDescending(s => s.Yield).ToList<Test>();
                                break;
                            default:
                                model.Tests = model.Tests.OrderByDescending(s => s.Yield).ToList<Test>();
                                break;
                        }
                        break;

                }

                oldSort = newSort;

            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Tests != null)
            {
                model.Tests = model.Tests.OrderByDescending(s => s.Yield).ToList<Test>();
                oldSort = "rindeDesc";
            }
            //////////////////////////////////////////////////////////////

            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = _attributeRepository.GetFilters(model.CategoriaIdEnsayos).Distinct();
            ViewBag.Empresas = _companyRepository.Get(c => !c.IsDisabled && c.Products.Select(p => p.CategoryId).Contains(model.CategoriaIdEnsayos)).Distinct();
            ViewBag.Provincias = model.Tests.Select(t => t.Place.Province).Distinct();
            ViewBag.Localidades = model.Tests.Select(t => t.Place.Locality).Distinct();
            ViewBag.Fuentes = model.Tests.Select(t => t.Source).Distinct();
            ViewBag.Campanas = _campaignRepository.Get(c => c.CategoryId == model.CategoriaIdEnsayos);

            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;

            //Paginacion
            if (model.Tests.Count() > limit_fin && model.Tests.Count() > limit_ini)
            {
                model.Tests = model.Tests.Skip<Test>(limit_ini).ToList<Test>();
                model.Tests = model.Tests.Take(limit_fin).ToList<Test>();
            }
            else if (model.Tests.Count() <= limit_ini)
            {
                model.Tests.Clear();
            }

            return Json(model.Tests, JsonRequestBehavior.AllowGet);

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


            if (ProductoId != 0)
            {
                model.Tests = _testRepository.Get(t => t.Product.CategoryId == model.CategoriaIdEnsayos && t.ProductId == ProductoId,
                                                    inc => inc.Product);
            }
            else 
            {
                model.Tests = _testRepository.Lookup(model.CategoriaIdEnsayos, model.BuscarEnsayos, cond_empresa, cond_fuente, cond_provincia, cond_localidad, cond_campana, list_atributo);
            }
            

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
            ViewBag.Filtros = _attributeRepository.GetFilters(model.CategoriaIdEnsayos).Distinct();
            ViewBag.Empresas = _companyRepository.Get(c => !c.IsDisabled && c.Products.Select(p => p.CategoryId).Contains(model.CategoriaIdEnsayos)).Distinct();
            ViewBag.Provincias = model.Tests.Select(t => t.Place.Province).Distinct();
            ViewBag.Localidades = model.Tests.Select(t => t.Place.Locality).Distinct();
            ViewBag.Fuentes = model.Tests.Select(t => t.Source).Distinct();
            ViewBag.Campanas = _campaignRepository.Get(c => c.CategoryId == model.CategoriaIdEnsayos);
            
            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;

            return View(model);
        }

        //TODO: Completar;
        [Authorize()]
        [HttpGet()]
        public string GetChartData(int categoriaId, int campanaId, int lugarId, string fuente)
        {
            return string.Empty; //EnsayoService.Get(categoriaId, null, campanaId, lugarId, fuente, true).ToJson();
        }
    }
}
