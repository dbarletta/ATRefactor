using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Services;
using AgroEnsayos.Entities;
using System.IO;

namespace AgroEnsayos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Productos

        public ViewResult Productos()
        {
            return View();
        }

        public void Edit(Producto producto)
        {
            ProductoService.Edit(producto);
        }

        public void Add(Producto producto)
        {
            ProductoService.Add(producto);
        }

        public void Delete(int productId)
        {
            ProductoService.DisableProduct(productId);
        }

        public JsonResult GetCategorias()
        {
            var categorias = CategoriaService.Get().Where(x => x.PadreId.HasValue && x.Padre.Equals("Semillas")).ToList();
            return Json(categorias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresas()
        {
            var empresas = EmpresaService.Get(0, 0);
            return Json(empresas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductos(int categoriaId = 4)
        {
            var productos = ProductoService.Get(categoriaId);
            return Json(productos);
        }

        public JsonResult GetProductoById(int productoId)
        {
            var prod = ProductoService.Get(null, false, productoId).First();
            return Json(prod);
        }
        #endregion

        #region ProductoAtributos

        public ViewResult ProductoAtributos()
        {
            return View();
        }

        public JsonResult getProductoAtributos(Producto producto)
        {

            producto.Atributos = AtributoService.Atributo_GetWithOriginalValues(producto.Id);
            return Json(producto);
        }

        public void SaveProductoAtributos(List<Atributo> Atributos, int ProductoId)
        {
            List<ProductoAtributo> lista = Atributos.Select(at => new ProductoAtributo()
            {
                ProductoId = ProductoId,
                AtributoId = at.Id,
                Valor = at.equivalencia_valor,

            }).ToList();
            ProductoService.SaveProductoAtributos(lista);
        }
        #endregion

        #region ProductoLugares

        public ViewResult ProductoLugares()
        {
            return View();
        }
        
        public void SaveProductoLugares(List<ProductoLugares> lista, int productId)
        {
            ProductoService.SaveProductoLugares(lista, productId);
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
            ViewBag.Categorias = CategoriaService.Get().Where(x => x.PadreId.HasValue && x.Padre.Equals("Semillas")).ToList();
            return View();
        }
        #endregion

        #region Ensayos

        public ViewResult Ensayos()
        {
            return View();
        }

        public ActionResult verifyExcelUploaded()
        {
            object obj= ImporterService.IsUploaded();
            return Json(obj);
        }

        [HttpPost]
        public ActionResult UploadEnsayosFile(HttpPostedFileBase file)
        {
            var sourceDirectoryPath = Configuration.ImportersPath;
            DirectoryInfo di = new DirectoryInfo(sourceDirectoryPath);
            FileInfo[] files = di.GetFiles("*.xls*", SearchOption.TopDirectoryOnly);
            if (files.Count() > 0)
            {
                string[] allFiles = Directory.GetFiles(sourceDirectoryPath);
                foreach (string filePath in allFiles)
                {
                    System.IO.File.Delete(filePath);
                }
            }
            ImporterService.TruncateTemp();
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Configuration.ImportersPath, fileName);
                file.SaveAs(path);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Ensayos");
        }

        public ActionResult verifyExcelHeader(string fileName)
        {
            List<ImporterResult> lista = ImporterService.ImportEnsayosStep1();
            return Json(lista);
        }

        public ActionResult ImportExcelToTemporalTable()
        {
            List<ImporterResult> errors = ImporterService.ImportEnsayosStep2();
            return Json(errors);
        }

        public ActionResult RunVerifications(int categoriaId)
        {
            List<ImporterResult> listaErrores = ImporterService.ImportEnsayosStep3(categoriaId);
            return Json(listaErrores);
        }

        public JsonResult UpsertEnsayos(int categoriaId)
        {
            var succeed = ImporterService.ImportEnsayosStep4(categoriaId);
            return Json(succeed);
        }
        #endregion

      

    }
}
