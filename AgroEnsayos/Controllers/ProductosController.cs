using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers
{
    public class ProductosController : Controller
    {
        private IProductRepository _productRepository = null;
        private ICategoryRepository _categoryRepository = null;
        private IAttributeRepository _attributeRepository = null;
        private ICompanyRepository _companyRepository = null;
        private IPlaceRepository _placeRepository = null;

        public ProductosController()
        {
            var ctxFactory = new EFDataContextFactory();
            _productRepository = new ProductRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
            _attributeRepository = new AttributeRepository(ctxFactory);
            _companyRepository = new CompanyRepository(ctxFactory);
            _placeRepository = new PlaceRepository(ctxFactory);
        }

        [Authorize()]
        public ActionResult Admin(int id)
        {
            ViewBag.Productos = _productRepository.Get(p => p.CategoryId == id);
            return PartialView();
        }

        public ActionResult Index(int id)
        {
            return View();
        }

        [Authorize()]
        public ActionResult GetPageProducts(int lastRowId, bool isHistoryBack, List<string> list_filters, int CategoriaIdProductos = 0, string BuscarProductos = "", string newSort = "", string oldSort = "", string select_agroup = "")
        {
            SearchModel model = new SearchModel();

            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            int limit_ini = lastRowId;
            int limit_fin = 30;

            string cond_empresa = "";
            string cond_antiguedad = "";
            string cond_region = "";
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
                    else if (strRubro == "Region")
                    {
                        cond_region = cond_region + "," + strId;
                    }
                    else if (strRubro == "Nuevo")
                    {
                        cond_antiguedad = cond_antiguedad + "," + strId;
                    }
                    else
                    {
                        list_atributo.Add(strId + "--" + strValor);
                    }
                }
            }

            if (cond_empresa != "")
                cond_empresa = cond_empresa + ",";
            if (cond_region != "")
                cond_region = cond_region + ",";
            if (cond_antiguedad != "")
                cond_antiguedad = cond_antiguedad + ",";

            model.Category = "NN";
            List<Category> ar_c = _categoryRepository.GetAll();
            foreach (Category c in ar_c)
            {
                if (c.Id == model.CategoriaIdProductos)
                {
                    model.Category = c.Name;
                }
            }
            ///////////// Obtener Productos x Filtros /////////////
            model.Products = _productRepository.Lookup(model.CategoriaIdProductos, model.BuscarProductos, cond_empresa, cond_antiguedad, cond_region, list_atributo);
            
            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////
            if (newSort != "" && model.Products != null)
            {
                switch (newSort)
                {
                    case "empresaAsc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenBy(s => s.Company.Name).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenBy(s => s.Company.Name).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenBy(s => s.Company.Name).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenBy(s => s.Company.Name).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenBy(s => s.Company.Name).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ToList<Product>();
                                break;
                        }
                        break;
                    case "nombreAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenBy(s => s.Name).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenBy(s => s.Name).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenBy(s => s.Name).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenBy(s => s.Name).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenBy(s => s.Name).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.Name).ToList<Product>();
                                break;
                        }
                        break;
                    case "cicloAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenBy(s => s.Cycle).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenBy(s => s.Cycle).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenBy(s => s.Cycle).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenBy(s => s.Cycle).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.Cycle).ToList<Product>();
                                break;
                        }
                        break;
                    case "madurezAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenBy(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenBy(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenBy(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenBy(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ToList<Product>();
                                break;
                        }
                        break;
                    case "alturaAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenBy(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenBy(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenBy(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenBy(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenBy(s => s.PlantHeight).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ToList<Product>();
                                break;
                        }
                        break;
                    case "materialAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenBy(s => s.Material).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenBy(s => s.Material).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenBy(s => s.Material).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenBy(s => s.Material).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenBy(s => s.Material).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderBy(s => s.Material).ToList<Product>();
                                break;
                        }
                        break;
                    case "empresaDesc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.Company.Name).ToList<Product>();
                                break;
                        }
                        break;
                    case "nombreDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenByDescending(s => s.Name).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenByDescending(s => s.Name).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenByDescending(s => s.Name).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenByDescending(s => s.Name).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenByDescending(s => s.Name).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.Name).ToList<Product>();
                                break;
                        }
                        break;
                    case "cicloDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenByDescending(s => s.Cycle).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenByDescending(s => s.Cycle).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenByDescending(s => s.Cycle).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenByDescending(s => s.Cycle).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.Cycle).ToList<Product>();
                                break;
                        }
                        break;
                    case "madurezDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenByDescending(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenByDescending(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenByDescending(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenByDescending(s => s.DaysToMaturity).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.DaysToMaturity).ToList<Product>();
                                break;
                        }
                        break;
                    case "alturaDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                            case "material":
                                model.Products = model.Products.OrderBy(s => s.Material).ThenByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.PlantHeight).ToList<Product>();
                                break;
                        }
                        break;
                    case "materialDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Products = model.Products.OrderBy(s => s.Company.Name).ThenByDescending(s => s.Material).ToList<Product>();
                                break;
                            case "nombre":
                                model.Products = model.Products.OrderBy(s => s.Name).ThenByDescending(s => s.Material).ToList<Product>();
                                break;
                            case "ciclo":
                                model.Products = model.Products.OrderBy(s => s.Cycle).ThenByDescending(s => s.Material).ToList<Product>();
                                break;
                            case "madurez":
                                model.Products = model.Products.OrderBy(s => s.DaysToMaturity).ThenByDescending(s => s.Material).ToList<Product>();
                                break;
                            case "altura":
                                model.Products = model.Products.OrderBy(s => s.PlantHeight).ThenByDescending(s => s.Material).ToList<Product>();
                                break;
                            default:
                                model.Products = model.Products.OrderByDescending(s => s.Material).ToList<Product>();
                                break;
                        }
                        break;

                }

                oldSort = newSort;

            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Products != null)
            {
                model.Products = model.Products.OrderBy(s => s.Name).ToList<Product>();
                oldSort = "empresaAsc";
            }
            //////////////////////////////////////////////////////////////

            //Paginacion
            if (model.Products.Count() > limit_fin && model.Products.Count() > limit_ini)
            {
                model.Products = model.Products.Skip<Product>(limit_ini).ToList<Product>();
                model.Products = model.Products.Take(limit_fin).ToList<Product>();
            }
            else if( model.Products.Count() <= limit_ini)
            {
                model.Products.Clear();
            }

            return Json(model.Products, JsonRequestBehavior.AllowGet);
        }

        [Authorize()]
        public ActionResult Resultados(SearchModel model, string new_id_filter, string new_val_filter, string new_name_filter, List<string> list_filters, int remove_id_filter = -1, string newSort = "", string oldSort = "", string select_agroup = "", int CategoriaIdProductos = 0, string BuscarProductos = "")
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;
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
            
            //////////// Agregar Filtro //////////////
            if (new_id_filter != null && new_id_filter != "" && remove_id_filter<0)
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
            string cond_antiguedad = "";
            string cond_region = "";
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
                    tam=strFilter.IndexOf("--");
                    strRubro=strFilter.Substring(0,tam);
                    
                    tam_aux=tam+2;
                    tam = strFilter.IndexOf("--", tam_aux);
                    strId = strFilter.Substring(tam_aux, tam - tam_aux);
                    
                    tam_aux=tam+2;
                    strValor = strFilter.Substring(tam_aux);

                    if (strRubro == "Empresa")
                    {
                        cond_empresa = cond_empresa + "," + strId;
                    }
                    else if (strRubro == "Region")
                    {
                        cond_region = cond_region + "," + strId;
                    }
                    else if (strRubro == "Nuevo")
                    {
                        cond_antiguedad = cond_antiguedad + "," + strId ;
                    }
                    else
                    {
                        list_atributo.Add(strId + "--" + strValor);
                    }
                }
            }

            if (cond_empresa != "")
                cond_empresa = cond_empresa + ",";
            if (cond_region != "")
                cond_region = cond_region + ",";
            if (cond_antiguedad != "")
                cond_antiguedad = cond_antiguedad + ",";

            model.Category = "NN";
            List<Category> ar_c = _categoryRepository.GetAll();
            foreach (Category c in ar_c)
            {
                if (c.Id == model.CategoriaIdProductos)
                {
                    model.Category = c.Name;
                }
            }

            ///////////// Obtener Productos x Filtros /////////////
            model.Products = _productRepository.Lookup(model.CategoriaIdProductos, model.BuscarProductos, cond_empresa, cond_antiguedad, cond_region, list_atributo);


            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = _attributeRepository.GetFilters(model.CategoriaIdProductos).Distinct();
            ViewBag.Empresas = _companyRepository.Get(c => !c.IsDisabled && c.Products.Select(p => p.CategoryId).Contains(model.CategoriaIdProductos)).Distinct();

            var places = new List<int>();
            model.Products.ForEach(p => places.AddRange(p.Places.Select(pl => pl.Id)));

            ViewBag.Regiones = _placeRepository.Get(p => !string.IsNullOrEmpty(p.Region) 
                                                        && places.Contains(p.Id));

            

            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;
         
            return View(model);
        }


       
        [Authorize()]
        public ActionResult Testimonios(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int EsEnsayo=0, int id = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            model.Products = _productRepository.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Products != null && model.Products.Count > 0)
            {
                model.Category = model.Products.First().Category.Name;
            }
            else
            { model.Category = "Trigo"; }
            ///////////// Obtener Productos x Filtros /////////////

            ViewBag.EsEnsayo = EsEnsayo;
            return View(model);
        }

        [Authorize()]
        public ActionResult Ficha(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int EsEnsayo=0, int id = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            List<Product> producto = new List<Product>();
            List<Domain.Entities.Attribute> atributos = new List<Domain.Entities.Attribute>();
            List<Company> empresa = new List<Company>();
            
            if (id != 0)
            {
                producto = _productRepository.Get(p => p.Id == id);
            }

            if (producto.Count > 0)
            {
                empresa = _companyRepository.Get(c => c.Id == producto[0].CompanyId);
                if (empresa.Count > 0)
                {
                    ViewBag.Empresa = empresa[0];
                }
                atributos = producto[0].AttributeMappings.Select(m => m.Attribute).ToList();
                ViewBag.Producto = producto[0];
                ViewBag.Atributo = atributos;
            }

            model.Products = _productRepository.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Products != null && model.Products.Count > 0)
            {
                model.Category = model.Products.First().Category.Name;
            }
            else
            { model.Category = "Trigo"; }
            ///////////// Obtener Productos x Filtros /////////////

            ViewBag.EsEnsayo = EsEnsayo;
            return View(model);
        }

        [Authorize()]
        public ActionResult Comparar(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int id1 = 0, int id2 = 0, int id3 = 0, int id4 = 0, int id5 = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            List<Product> producto1 = new List<Product>();
            List<Product> producto2 = new List<Product>();
            List<Product> producto3 = new List<Product>();
            List<Product> producto4 = new List<Product>();
            List<Product> producto5 = new List<Product>();
            
            List<Domain.Entities.Attribute> atributos1 = new List<Domain.Entities.Attribute>();
            List<Domain.Entities.Attribute> atributos2 = new List<Domain.Entities.Attribute>();
            List<Domain.Entities.Attribute> atributos3 = new List<Domain.Entities.Attribute>();
            List<Domain.Entities.Attribute> atributos4 = new List<Domain.Entities.Attribute>();
            List<Domain.Entities.Attribute> atributos5 = new List<Domain.Entities.Attribute>();

            List<Domain.Entities.Attribute> atr_comp = new List<Domain.Entities.Attribute>();

            atr_comp = _categoryRepository.Single(c => c.Id == CategoriaIdProductos, inc => inc.Attributes).Attributes.ToList();
            ViewBag.AtrComp = atr_comp;
            
            if (id1 != 0)
            {
                producto1 = _productRepository.Get(p => p.Id == id1, inc => inc.AttributeMappings);
                if (producto1.Count > 0)
                    atributos1 = producto1[0].AttributeMappings.Select(x => x.Attribute).ToList();
                ViewBag.Producto1 = producto1[0];
                ViewBag.Atributo1 = atributos1;
            }
            if (id2 != 0)
            {
                producto2 = _productRepository.Get(p => p.Id == id2, inc => inc.AttributeMappings);
                if (producto2.Count > 0)
                    atributos2 = producto2[0].AttributeMappings.Select(x => x.Attribute).ToList();
                ViewBag.Producto2 = producto2[0];
                ViewBag.Atributo2 = atributos2;
            } 
            if (id3 != 0)
            {
                producto3 = _productRepository.Get(p => p.Id == id3, inc => inc.AttributeMappings);
                if (producto3.Count > 0)
                    atributos3 = producto3[0].AttributeMappings.Select(x => x.Attribute).ToList();
                ViewBag.Producto3 = producto3[0];
                ViewBag.Atributo3 = atributos3;
            }
            if (id4 != 0)
            {
                producto4 = _productRepository.Get(p => p.Id == id4, inc => inc.AttributeMappings);
                if (producto4.Count > 0)
                    atributos4 = producto4[0].AttributeMappings.Select(x => x.Attribute).ToList();
                ViewBag.Producto4 = producto4[0];
                ViewBag.Atributo4 = atributos4;
            }
            if (id5 != 0)
            {
                producto5 = _productRepository.Get(p => p.Id == id5, inc => inc.AttributeMappings);
                if (producto5.Count > 0)
                    atributos5 = producto5[0].AttributeMappings.Select(x => x.Attribute).ToList();
                ViewBag.Producto5 = producto5[0];
                ViewBag.Atributo5 = atributos5;
            }

            model.Products = _productRepository.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Products != null && model.Products.Count > 0)
            {
                model.Category = model.Products.First().Category.Name;
            }
            else
            { model.Category = "Trigo"; }
            ///////////// Obtener Productos x Filtros /////////////

            ViewBag.id1 = id1;
            ViewBag.id2 = id2;
            ViewBag.id3 = id1;
            ViewBag.id4 = id4;
            ViewBag.id5 = id5;

            return View(model);
        }
    }
}
