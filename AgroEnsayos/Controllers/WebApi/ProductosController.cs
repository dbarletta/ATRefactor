using AgroEnsayos.Entities;
using AgroEnsayos.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AgroEnsayos.Controllers.WebApi
{
    public class ProductosController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Producto
        // GET api/productos
        public IEnumerable<Producto> Get()
        {
            return ProductoService.Get(null, true);
        }

        // GET api/productos/5
        public Producto Get(int id)
        {
            return ProductoService.Get(null, false, id).First();
        }

        // POST api/productos
        public Producto Post([FromBody]Producto prod)
        {
            try
            {
                if (prod.Id == 0)
                    ProductoService.Add(prod);
                else
                    ProductoService.Edit(prod);
                return prod;
            }
            catch(Exception ex)
            {
                logger.Error("Error en el Post/Save de producto", ex);
                throw ex;
            }
        }

        // DELETE api/productos/5
        public void Delete(int id)
        {
            ProductoService.DisableProduct(id);
        }


        // GET api/productos/?categoryId=5
        public IEnumerable<Producto> GetByCategory(int categoryId)
        {
            return ProductoService.Get(categoryId);
        }
        #endregion

        #region ProductoLugares
        public List<ProductoLugarCheck> GetLugares(int productoId)
        {
            List<ProductoLugares> productosLugares = LugarService.GetProductoLugares(productoId);
            List<Lugar> lugares = LugarService.GetAllRegiones();

            List<ProductoLugarCheck> listado = lugares.Select(l => new ProductoLugarCheck()
            {
                LugarId = l.Id,
                ProductoId = productoId,
                Region = l.Region,
                IsChecked = productosLugares.Any(pl => pl.LugarId == l.Id)
            }).ToList();

            return listado;
        }

        public void PostLugares(List<ProductoLugares> lista, int productId)
        {
            ProductoService.SaveProductoLugares(lista, productId);
        } 
        #endregion

        #region ProductoAtributos
        
        //public Producto Get(Producto prod)
        //{
        //    prod.Atributos = AtributoService.Atributo_GetWithOriginalValues(prod.Id);
        //    return prod;
        //}

        //// POST api/products
        //public void Post([FromBody]List<Atributo> attributes, int productId)
        //{
        //    List<ProductoAtributo> lista = attributes.Select(at => new ProductoAtributo()
        //    {
        //        ProductoId = productId,
        //        AtributoId = at.Id,
        //        Valor = at.equivalencia_valor,

        //    }).ToList();
        //    ProductoService.SaveProductoAtributos(lista);
        //} 
        #endregion
    }
}
