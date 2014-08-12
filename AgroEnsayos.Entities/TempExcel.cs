using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class TempExcel
    {

        public string Fuente { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Mal { get; set; }
        public string Establecimiento { get; set; }
        public string Campana { get; set; }
        public DateTime FechaSiembra { get; set; }
        public DateTime FechaCosecha { get; set; }
        public string Producto { get; set; }
        public string ColumnaVacia { get; set; }
        public float Rinde { get; set; }
        public string Indice { get; set; }
        public string Quebrado { get; set; }
        public string Vuelco { get; set; }
        public string AlturaPlanta { get; set; }
        public string Humedad { get; set; }
        public string EsPromedio { get; set; }
        public string PlantasXHectarea { get; set; }
        public string DiasFloracion { get; set; }
        public string EmergenciaFloracion { get; set; }
        public string Observaciones { get; set; }
        public string Archivo { get; set; }
        public int? Row { get; set; }
    }
}
