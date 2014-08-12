using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public int? PadreId { get; set; }
        public string Padre { get; set; }
        public string Nombre { get; set; }
    }
}
