using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class Lugar
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string Cabecera { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Perimetro { get; set; }
        
    }
}
