using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace AgroEnsayos.Entities
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string Domicilio { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string Logo { get; set; }

    }
}
