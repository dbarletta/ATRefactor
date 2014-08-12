using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class Ensayo
    {
        public int Id { get; set; }
        public int CampanaId { get; set; }
        public string Campana { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int LugarId { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }

        public int CategoriaId { get; set; }
        public string Categoria { get; set; }

        public decimal Rinde { get; set; }
        public string Fuente { get; set; }
        public string Establecimiento { get; set; }

        public DateTime? FechaSiembra { get; set; }
        public DateTime? FechaCosecha { get; set; }

        public int? Indice { get; set; }
        public string Observaciones { get; set; }
        public string Archivo { get; set; }

        public int Ranking { get; set; }
        public int Total { get; set; }
    }
}
