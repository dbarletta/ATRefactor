using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgroEnsayos.Models
{
    public class LoginModel : AgroEnsayos.Entities.User
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }

        [Display(Name = "CategoriaId")]
        public int CategoriaIdProductos { get; set; }

        [Display(Name = "CategoriaId")]
        public int CategoriaIdEnsayos { get; set; }
    }
}