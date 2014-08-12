using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Services;
using AgroEnsayos.Entities;
using AgroEnsayos.Models;
using AgroEnsayos.Helpers;

namespace AgroEnsayos.Controllers
{
    public class EnsayosController : Controller
    {
        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.Ensayos = EnsayoService.Get(4).ToList();
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

            ///////////// Obtener Productos x Filtros /////////////
            if (ProductoId != 0)
            {
                model.Ensayos = EnsayoService.Get(model.CategoriaIdEnsayos, ProductoId);
            }
            else
            {
                model.Ensayos = EnsayoService.Lookup(model.CategoriaIdEnsayos, model.BuscarEnsayos, cond_empresa, cond_fuente, cond_provincia, cond_localidad, cond_campana, list_atributo);
            }

            model.Categoria = "NN";
            List<Categoria> ar_c = CategoriaService.Get();
            foreach (Categoria c in ar_c)
            {
                if (c.Id == model.CategoriaIdEnsayos)
                {
                    model.Categoria = c.Nombre;
                }
            }

            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////

            if (newSort != "" && model.Ensayos != null)
            {
                switch (newSort)
                {
                    case "campanaAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "productoAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "fuenteAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "provinciaAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "localidadAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "rindeAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                        }
                        break;

                    case "campanaDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "productoDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "fuenteDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "provinciaDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "localidadDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "rindeDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                        }
                        break;

                }

                oldSort = newSort;

            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Ensayos != null)
            {
                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Rinde).ToList<Ensayo>();
                oldSort = "rindeDesc";
            }
            //////////////////////////////////////////////////////////////

            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = AtributoService.Filter_Get(model.CategoriaIdEnsayos, 1);
            ViewBag.Empresas = EmpresaService.Get(0, model.CategoriaIdEnsayos);
            ViewBag.Provincias = LugarService.GetProvincias(model.CategoriaIdEnsayos);
            ViewBag.Localidades = LugarService.GetLocalidades(model.CategoriaIdEnsayos);
            ViewBag.Fuentes = EnsayoService.GetFuentes(model.CategoriaIdEnsayos);
            ViewBag.Campanas = CampanaService.GetByCategoria(model.CategoriaIdEnsayos);

            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;

            //Paginacion
            if (model.Ensayos.Count() > limit_fin && model.Ensayos.Count() > limit_ini)
            {
                model.Ensayos = model.Ensayos.Skip<Ensayo>(limit_ini).ToList<Ensayo>();
                model.Ensayos = model.Ensayos.Take(limit_fin).ToList<Ensayo>();
            }
            else if (model.Ensayos.Count() <= limit_ini)
            {
                model.Ensayos.Clear();
            }

            return Json(model.Ensayos, JsonRequestBehavior.AllowGet);

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
                model.Ensayos = EnsayoService.Get(model.CategoriaIdEnsayos, ProductoId);
            }
            else 
            {
                model.Ensayos = EnsayoService.Lookup(model.CategoriaIdEnsayos, model.BuscarEnsayos, cond_empresa, cond_fuente, cond_provincia, cond_localidad, cond_campana, list_atributo);
            }
            
            if (model.Ensayos != null && model.Ensayos.Count > 0)
            {
                model.Categoria = model.Ensayos.First().Categoria;
            }
            else
            { model.Categoria = "Trigo"; }

            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////
            
            if (newSort != "" && model.Ensayos != null)
            {
                /*
                switch (newSort)
                {
                    case "campanaAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Campana).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "productoAsc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Producto).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "fuenteAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "provinciaAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "localidadAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "rindeAsc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ToList<Ensayo>();
                                break;
                        }
                        break;

                    case "campanaDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Campana).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "productoDesc":
                        switch (select_agroup)
                        {
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Producto).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "fuenteDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Fuente).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "provinciaDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Provincia).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "localidadDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            case "rinde":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Rinde).ThenByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Localidad).ToList<Ensayo>();
                                break;
                        }
                        break;
                    case "rindeDesc":
                        switch (select_agroup)
                        {
                            case "producto":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Producto).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "campana":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Campana).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "fuente":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Fuente).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "provincia":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Provincia).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            case "localidad":
                                model.Ensayos = model.Ensayos.OrderBy(s => s.Localidad).ThenByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                            default:
                                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Rinde).ToList<Ensayo>();
                                break;
                        }
                        break;

                }
            */
                oldSort = newSort;
                
            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Ensayos != null)
            {
                model.Ensayos = model.Ensayos.OrderByDescending(s => s.Rinde).ToList<Ensayo>();
                oldSort = "rindeDesc";
            }
            //////////////////////////////////////////////////////////////

            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = AtributoService.Filter_Get(model.CategoriaIdEnsayos,1);
            ViewBag.Empresas = EmpresaService.Get(0, model.CategoriaIdEnsayos);
            ViewBag.Provincias = LugarService.GetProvincias(model.CategoriaIdEnsayos);
            ViewBag.Localidades = LugarService.GetLocalidades(model.CategoriaIdEnsayos);
            ViewBag.Fuentes = EnsayoService.GetFuentes(model.CategoriaIdEnsayos);
            ViewBag.Campanas = CampanaService.GetByCategoria(model.CategoriaIdEnsayos);
            
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
        [HttpGet()]
        public string GetChartData(int categoriaId, int campanaId, int lugarId, string fuente)
        {
            return EnsayoService.Get(categoriaId, null, campanaId, lugarId, fuente, true).ToJson();
        }
    }
}
