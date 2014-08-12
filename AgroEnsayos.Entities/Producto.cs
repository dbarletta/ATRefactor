using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
        public int EmpresaId { get; set; }
        public string Empresa { get; set; }
        public string Nombre { get; set; }
        public string Material { get; set; }

        public bool? EsHibrido { get; set ; }
        public string Ciclo { get; set; } 
        public bool? EsConvencional { get; set; }

        public int? DiasFloracion { get; set; }
        public int? DiasMadurez { get; set; }

        public int? AlturaPlanta { get; set; }
        public bool? EsNuevo { get; set; }
        public int? Alta { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string DescripcionPG { get; set; }

        public bool Deshabilitado { get; set; }

        public List<ProductoLugares> Lugares { get; set; }
        public List<Atributo> Atributos { get; set; }

        public Producto()
        {
            this.Ciclo = "";
            this.AlturaPlanta = 0;
        
        }


    }

    

}
