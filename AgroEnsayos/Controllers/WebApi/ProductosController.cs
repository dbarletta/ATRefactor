using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers.WebApi
{
    public class ProductosController : ApiController
    {
        private static Logger logger = null;
        private IProductRepository _productRepository = null;
        private IPlaceRepository _placeRepository = null;

        public ProductosController()
        {
            logger = LogManager.GetCurrentClassLogger();
            var ctxFactory = new EFDataContextFactory();
            _productRepository = new ProductRepository(ctxFactory);
            _placeRepository = new PlaceRepository(ctxFactory);
        }

        #region Producto
        // GET api/productos
        public IEnumerable<Product> Get()
        {
            return _productRepository.GetAll();
        }

        // GET api/productos/5
        public Product Get(int id)
        {
            return _productRepository.Single(p => p.Id == id && !p.IsDisabled);
        }

        // POST api/productos
        public Product Post([FromBody]Product prod)
        {
            try
            {
                if (prod.Id == 0)
                    _productRepository.Insert(prod);
                else
                    _productRepository.Update(prod);
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
            var product = _productRepository.Single(p => p.Id == id);
            product.Disable();

            _productRepository.Update(product);
        }


        // GET api/productos/?categoryId=5
        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _productRepository.Get(p => p.CategoryId == categoryId);
        }
        #endregion

        #region ProductoLugares
        public List<ProductoLugarCheck> GetLugares(int productoId)
        {
            var product = _productRepository.Single(p => p.Id == productoId, inc => inc.Places);

            List<Place> regions = _placeRepository.Get(l => !string.IsNullOrEmpty(l.Region)
                                                           && string.IsNullOrEmpty(l.Department)
                                                           && string.IsNullOrEmpty(l.Header));

            List<ProductoLugarCheck> listado = regions.Select(l => new ProductoLugarCheck()
            {
                LugarId = l.Id,
                ProductoId = productoId,
                Region = l.Region,
                IsChecked = product.Places.Any(pl => pl.Id == l.Id)
            }).ToList();

            return listado;
        }

        //TODO: Refactor UI con nuevo param
        public void PostLugares(Product product)
        {
            _productRepository.SaveGraph(product);
        } 
        #endregion

        #region ProductoAtributos
        

        #endregion
    }
}
