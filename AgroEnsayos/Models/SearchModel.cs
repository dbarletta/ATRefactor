using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AgroEnsayos.Domain.Entities;

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

        public List<Product> Products { get; set; }

        public List<Test> Tests { get; set; }

        public string Category { get; set; }

        public Dictionary<string, List<string>> dicFilter{get ; set;}
    
    }
}