using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AgroEnsayos.Entities;

namespace AgroEnsayos.Models
{
    public class EquivalenciaModel
    {
        public List<AtributoEquivalencia> Equivalencias { get; set; }
    }
}