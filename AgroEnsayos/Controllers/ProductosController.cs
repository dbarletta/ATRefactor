using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Services;
using AgroEnsayos.Entities;
using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers
{
    public class ProductosController : Controller
    {
        [Authorize()]
        public ActionResult Admin(int id)
        {
            ViewBag.Productos = ProductoService.Get(id);
            return PartialView();
        }

        [Authorize()]
        public ActionResult GetPageProducts(int lastRowId, bool isHistoryBack, List<string> list_filters, int CategoriaIdProductos = 0, string BuscarProductos = "", string newSort = "", string oldSort = "", string select_agroup = "")
        {
            SearchModel model = new SearchModel();

            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            int limit_ini = lastRowId;
            int limit_fin = 30;
            //var sectionArticles = BLL.SectionArticle.GetNextSectionTopArticles(lastRowId, isHistoryBack);

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

            model.Categoria = "NN";
            List<Categoria> ar_c = CategoriaService.Get();
            foreach (Categoria c in ar_c)
            {
                if (c.Id == model.CategoriaIdProductos)
                {
                    model.Categoria = c.Nombre;
                }
            }
            ///////////// Obtener Productos x Filtros /////////////
            model.Productos = ProductoService.Lookup(model.CategoriaIdProductos, model.BuscarProductos, cond_empresa, cond_antiguedad, cond_region, list_atributo);//,limit_ini,limit_fin); //Matias, Comente esta parte de codigo para que compile y asi hacer un deploy.
            
            if (select_agroup != "" && newSort == "")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////
            if (newSort != "" && model.Productos != null)
            {
                switch (newSort)
                {
                    case "empresaAsc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ToList<Producto>();
                                break;
                        }
                        break;
                    case "nombreAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ToList<Producto>();
                                break;
                        }
                        break;
                    case "cicloAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ToList<Producto>();
                                break;
                        }
                        break;
                    case "madurezAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                        }
                        break;
                    case "alturaAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                        }
                        break;
                    case "materialAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Material).ToList<Producto>();
                                break;
                        }
                        break;
                    case "empresaDesc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                        }
                        break;
                    case "nombreDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                        }
                        break;
                    case "cicloDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                        }
                        break;
                    case "madurezDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                        }
                        break;
                    case "alturaDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                        }
                        break;
                    case "materialDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Material).ToList<Producto>();
                                break;
                        }
                        break;

                }

                oldSort = newSort;

            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Productos != null)
            {
                model.Productos = model.Productos.OrderBy(s => s.Nombre).ToList<Producto>();
                oldSort = "empresaAsc";
            }
            //////////////////////////////////////////////////////////////

            //Paginacion
            if (model.Productos.Count() > limit_fin && model.Productos.Count() > limit_ini)
            {
                model.Productos = model.Productos.Skip<Producto>(limit_ini).ToList<Producto>();
                model.Productos = model.Productos.Take(limit_fin).ToList<Producto>();
            }
            else if( model.Productos.Count() <= limit_ini)
            {
                model.Productos.Clear();
            }

            return Json(model.Productos, JsonRequestBehavior.AllowGet);
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

            model.Categoria = "NN";
            List<Categoria> ar_c = CategoriaService.Get();
            foreach(Categoria c in ar_c)
            {
                if (c.Id == model.CategoriaIdProductos)
                {
                    model.Categoria = c.Nombre;
                }
            }

            ///////////// Obtener Productos x Filtros /////////////
            model.Productos = ProductoService.Lookup(model.CategoriaIdProductos, model.BuscarProductos, cond_empresa, cond_antiguedad, cond_region, list_atributo);

            /*
            if (select_agroup != "" && newSort=="")
            {
                newSort = select_agroup + "Asc";
            }

            ////////////////////// Ordenar y Agrupar //////////////////////////////
            if (newSort != "" && model.Productos != null)
            {
                switch (newSort)
                {
                    case "empresaAsc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Empresa).ToList<Producto>();
                                break;
                            default: 
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ToList<Producto>();
                                break;
                        }
                        break;
                    case "nombreAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Nombre).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ToList<Producto>();
                                break;
                        }
                        break;
                    case "cicloAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.Ciclo).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ToList<Producto>();
                                break;
                        }
                        break;
                    case "madurezAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ToList<Producto>();
                                break;
                        }
                        break;
                    case "alturaAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                        }
                        break;
                    case "materialAsc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenBy(s => s.Material).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderBy(s => s.Material).ToList<Producto>();
                                break;
                        }
                        break;
                    case "empresaDesc":
                        switch (select_agroup)
                        {
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Empresa).ToList<Producto>();
                                break;
                        }
                        break;
                    case "nombreDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Nombre).ToList<Producto>();
                                break;
                        }
                        break;
                    case "cicloDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Ciclo).ToList<Producto>();
                                break;
                        }
                        break;
                    case "madurezDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.DiasMadurez).ToList<Producto>();
                                break;
                        }
                        break;
                    case "alturaDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            case "material":
                                model.Productos = model.Productos.OrderBy(s => s.Material).ThenByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.AlturaPlanta).ToList<Producto>();
                                break;
                        }
                        break;
                    case "materialDesc":
                        switch (select_agroup)
                        {
                            case "empresa":
                                model.Productos = model.Productos.OrderBy(s => s.Empresa).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "nombre":
                                model.Productos = model.Productos.OrderBy(s => s.Nombre).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "ciclo":
                                model.Productos = model.Productos.OrderBy(s => s.Ciclo).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "madurez":
                                model.Productos = model.Productos.OrderBy(s => s.DiasMadurez).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            case "altura":
                                model.Productos = model.Productos.OrderBy(s => s.AlturaPlanta).ThenByDescending(s => s.Material).ToList<Producto>();
                                break;
                            default:
                                model.Productos = model.Productos.OrderByDescending(s => s.Material).ToList<Producto>();
                                break;
                        }
                        break;
                        
                }

                oldSort = newSort;
                
            }

            if (newSort == "" && oldSort == "" && select_agroup == "" && model.Productos != null)
            {
                model.Productos = model.Productos.OrderBy(s => s.Nombre).ToList<Producto>();
                oldSort = "empresaAsc";
            }
            //////////////////////////////////////////////////////////////
            */
            ///////////// Guardar Parametros /////////////////////
            ViewBag.Filtros = AtributoService.Filter_Get(model.CategoriaIdProductos,1);
            ViewBag.Empresas = EmpresaService.Get(0, model.CategoriaIdProductos);
            ViewBag.Regiones = LugarService.GetRegiones(model.CategoriaIdProductos);
            ViewBag.OldSort = oldSort;
            ViewBag.SelectAgroup = select_agroup;

            if (list_filters != null)
            {
                list_filters.Sort();
            }
            ViewBag.list_filters = list_filters;
         
            return View(model);
        }

        public ActionResult Index(int id)
        {
            return View();
        }

       
        [Authorize()]
        public ActionResult Testimonios(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int EsEnsayo=0, int id = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            model.Productos = ProductoService.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Productos != null && model.Productos.Count > 0)
            {
                model.Categoria = model.Productos.First().Categoria;
            }
            else
            { model.Categoria = "Trigo"; }
            ///////////// Obtener Productos x Filtros /////////////

            ViewBag.EsEnsayo = EsEnsayo;
            return View(model);
        }

        [Authorize()]
        public ActionResult Ficha(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int EsEnsayo=0, int id = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            List<Producto> producto = new List<Producto>();
            List<Atributo> atributos = new List<Atributo>();
            List<Empresa> empresa = new List<Empresa>();
            
            if (id != 0)
            {
                producto = ProductoService.Get(0,true,id);
            }

            if (producto.Count > 0)
            {
                empresa = EmpresaService.Get(producto[0].EmpresaId);
                if (empresa.Count > 0)
                {
                    ViewBag.Empresa = empresa[0];
                }
                atributos = AtributoService.ProductoAtributo_Get(producto[0].Id);
                ViewBag.Producto = producto[0];
                ViewBag.Atributo = atributos;
            }

            model.Productos = ProductoService.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Productos != null && model.Productos.Count > 0)
            {
                model.Categoria = model.Productos.First().Categoria;
            }
            else
            { model.Categoria = "Trigo"; }
            ///////////// Obtener Productos x Filtros /////////////

            ViewBag.EsEnsayo = EsEnsayo;
            return View(model);
        }

        [Authorize()]
        public ActionResult Comparar(SearchModel model, int CategoriaIdProductos = 0, string BuscarProductos = "", int id1 = 0, int id2 = 0, int id3 = 0, int id4 = 0, int id5 = 0)
        {
            model.BuscarProductos = BuscarProductos;
            model.CategoriaIdProductos = CategoriaIdProductos;

            List<Producto> producto1 = new List<Producto>();
            List<Producto> producto2 = new List<Producto>();
            List<Producto> producto3 = new List<Producto>();
            List<Producto> producto4 = new List<Producto>();
            List<Producto> producto5 = new List<Producto>();
            
            List<Atributo> atributos1 = new List<Atributo>();
            List<Atributo> atributos2 = new List<Atributo>();
            List<Atributo> atributos3 = new List<Atributo>();
            List<Atributo> atributos4 = new List<Atributo>();
            List<Atributo> atributos5 = new List<Atributo>();

            List<Atributo> atr_comp = new List<Atributo>();

            atr_comp = AtributoService.Get(CategoriaIdProductos);
            ViewBag.AtrComp = atr_comp;
            
            if (id1 != 0)
            {
                producto1 = ProductoService.Get(0, true, id1);
                if (producto1.Count > 0)
                    atributos1 = AtributoService.ProductoAtributo_Get(producto1[0].Id);
                ViewBag.Producto1 = producto1[0];
                ViewBag.Atributo1 = atributos1;
            }
            if (id2 != 0)
            {
                producto2 = ProductoService.Get(0, true, id2);
                if (producto2.Count > 0)
                    atributos2 = AtributoService.ProductoAtributo_Get(producto2[0].Id);
                ViewBag.Producto2 = producto2[0];
                ViewBag.Atributo2 = atributos2;
            } 
            if (id3 != 0)
            {
                producto3 = ProductoService.Get(0, true, id3);
                if (producto3.Count > 0)
                    atributos3 = AtributoService.ProductoAtributo_Get(producto3[0].Id);
                ViewBag.Producto3 = producto3[0];
                ViewBag.Atributo3 = atributos3;
            }
            if (id4 != 0)
            {
                producto4 = ProductoService.Get(0, true, id4);
                if (producto4.Count > 0)
                    atributos4 = AtributoService.ProductoAtributo_Get(producto4[0].Id);
                ViewBag.Producto4 = producto4[0];
                ViewBag.Atributo4 = atributos4;
            }
            if (id5 != 0)
            {
                producto5 = ProductoService.Get(0, true, id5);
                if (producto5.Count > 0)
                    atributos5 = AtributoService.ProductoAtributo_Get(producto5[0].Id);
                ViewBag.Producto5 = producto5[0];
                ViewBag.Atributo5 = atributos5;
            }

            model.Productos = ProductoService.Lookup(model.CategoriaIdProductos, model.BuscarProductos);
            if (model.Productos != null && model.Productos.Count > 0)
            {
                model.Categoria = model.Productos.First().Categoria;
            }
            else
            { model.Categoria = "Trigo"; }
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
