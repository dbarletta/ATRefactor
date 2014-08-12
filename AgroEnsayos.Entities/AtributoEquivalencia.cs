using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class AtributoEquivalencia
    {
        public int Id { get; set; }
        public int AtributoId { get; set; }
        public string Atributo { get; set; }
        public string Valor { get; set; }
        public string Equivalencia { get; set; }
        public byte Escala { get; set; }
    }
}
