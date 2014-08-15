using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Entities;

namespace AgroEnsayos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private IProductRepository _productRepository = null;
        private ICategoryRepository _categoryRepository = null;
        private ICompanyRepository _companyRepository = null;

        public AdminController()
        {
            var ctxFactory = new EFDataContextFactory();
            _productRepository = new ProductRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
            _companyRepository = new CompanyRepository(ctxFactory);
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Productos

        public ViewResult Productos()
        {
            return View();
        }

        public void Edit(Product producto)
        {
            _productRepository.Update(producto);
        }

        public void Add(Product producto)
        {
            _productRepository.Insert(producto);
        }

        public void Delete(int productoId)
        {
            var product = _productRepository.Single(prd => prd.Id == productoId);

            product.Disable();

            _productRepository.Update(product);
        }

        public JsonResult GetCategorias()
        {
            var categorias = _categoryRepository.Get(x => x.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase)).ToList();
            return Json(categorias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresas()
        {
            var empresas = _companyRepository.Get(comp => !comp.IsDisabled);
            return Json(empresas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductos(int categoriaId = 4)
        {
            var productos = _productRepository.Get(prd => prd.CategoryId == categoriaId && !prd.IsDisabled);
            return Json(productos);
        }

        public JsonResult GetProductoById(int productoId)
        {
            var prod = _productRepository.Single(p => p.Id == productoId);
            return Json(prod);
        }
        #endregion

        #region ProductoAtributos

        public ViewResult ProductoAtributos()
        {
            return View();
        }

        //TODO: Revisar en UI los mappings.
        public JsonResult getProductoAtributos(Product producto)
        {
            var prd = _productRepository.Single(p => p.Id == producto.Id, inc => inc.AttributeMappings);
            producto.AttributeMappings = prd.AttributeMappings;
            return Json(producto);
        }

        //public void SaveProductoAtributos(List<Attribute> Atributos, int ProductoId)
        //{
        //List<ProductoAtributo> lista = Atributos.Select(at => 
        //    new ProductoAtributo()
        //    {
        //        ProductoId = ProductoId,
        //        AtributoId = at.Id,
        //        Valor = at.equivalencia_valor,

        //    }).ToList();
        //ProductoService.SaveProductoAtributos(lista);
        //}


        //TODO: Cambiar este metodo en la UI por SaveProductoAtributos
        public void SaveWithMappings(Product product)
        {
            _productRepository.SaveWithMappings(product);
        }

        #endregion

        #region ProductoLugares

        public ViewResult ProductoLugares()
        {
            return View();
        }

        //public void SaveProductoLugares(List<ProductoLugares> lista, int productId)
        //{
        //    ProductoService.SaveProductoLugares(lista, productId);
        //}

        //TODO: Cambiar este metodo en la UI por SaveProductoAtributos
        public void SaveWithPlaces(Product product)
        {
            _productRepository.SaveGraph(product);
        }

        #endregion

        #region Atributos

        public ViewResult Atributos()
        {
            return View();
        }
        #endregion

        #region Equivalencias

        public ViewResult Equivalencias()
        {
            ViewBag.Categorias = _categoryRepository.Get(x => x.ParentId.HasValue && x.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase)).ToList();
            return View();
        }
        #endregion

        //TODO: Revisar como conviene migrar el importador de ensayos.
        #region Ensayos
        //public ViewResult Ensayos()
        //{
        //    return View();
        //}

        //public ActionResult verifyExcelUploaded()
        //{
        //    object obj = ImporterService.IsUploaded();
        //    return Json(obj);
        //}

        //[HttpPost]
        //public ActionResult UploadEnsayosFile(HttpPostedFileBase file)
        //{
        //    var sourceDirectoryPath = Configuration.ImportersPath;
        //    DirectoryInfo di = new DirectoryInfo(sourceDirectoryPath);
        //    FileInfo[] files = di.GetFiles("*.xls*", SearchOption.TopDirectoryOnly);
        //    if (files.Count() > 0)
        //    {
        //        string[] allFiles = Directory.GetFiles(sourceDirectoryPath);
        //        foreach (string filePath in allFiles)
        //        {
        //            System.IO.File.Delete(filePath);
        //        }
        //    }
        //    ImporterService.TruncateTemp();
        //    // Verify that the user selected a file
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // extract only the fielname
        //        var fileName = Path.GetFileName(file.FileName);
        //        // store the file inside ~/App_Data/uploads folder
        //        var path = Path.Combine(Configuration.ImportersPath, fileName);
        //        file.SaveAs(path);
        //    }
        //    // redirect back to the index action to show the form once again
        //    return RedirectToAction("Ensayos");
        //}

        //public ActionResult verifyExcelHeader(string fileName)
        //{
        //    List<ImporterResult> lista = ImporterService.ImportEnsayosStep1();
        //    return Json(lista);
        //}

        //public ActionResult ImportExcelToTemporalTable()
        //{
        //    List<ImporterResult> errors = ImporterService.ImportEnsayosStep2();
        //    return Json(errors);
        //}

        //public ActionResult RunVerifications(int categoriaId)
        //{
        //    List<ImporterResult> listaErrores = ImporterService.ImportEnsayosStep3(categoriaId);
        //    return Json(listaErrores);
        //}

        //public JsonResult UpsertEnsayos(int categoriaId)
        //{
        //    var succeed = ImporterService.ImportEnsayosStep4(categoriaId);
        //    return Json(succeed);
        //}
        #endregion
    }
}
