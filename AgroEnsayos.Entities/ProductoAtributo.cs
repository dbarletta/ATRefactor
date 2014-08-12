using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class ProductoAtributo
    {
        public int ProductoId { get; set; }
        public int AtributoId { get; set; }
        public string Valor { get; set; }

    }
}
