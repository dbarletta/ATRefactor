using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class ProductoLugares
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int LugarId { get; set; }
        public string Region { get; set; }
    }
}
