using AgroEnsayos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgroEnsayos.Models
{
    public class SearchModel
    {
        [Display(Name = "Buscar")]
        public string BuscarProductos { get; set; }

        [Display(Name = "Buscar")]
        public string BuscarEnsayos { get; set; }

        [Display(Name = "CategoriaId")]
        public int CategoriaIdProductos { get; set; }

        [Display(Name = "CategoriaId")]
        public int CategoriaIdEnsayos { get; set; }

        public List<Producto> Productos { get; set; }

        public List<Ensayo> Ensayos { get; set; }

        public string Categoria { get; set; }

        public Dictionary<string, List<string>> dicFilter{get ; set;}
    
    }
}