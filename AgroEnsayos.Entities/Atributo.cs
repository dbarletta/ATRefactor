using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class Atributo
    {
        public int Id { get; set; }
        public string Rubro { get; set; }
        public string Nombre { get; set; }
        public byte TipoDato { get; set; }
        public string Tags { get; set; }
        public bool UsarComoFiltro { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
        
        // Cuando se obtienen los filtros por categoria, obtengo los valores posibles de cada atributo
        public string equivalencia_valor { get; set; }

        public Atributo()
        {
            this.equivalencia_valor = "";
        }
    }

    public enum TipoDato : int
    {
        Texto = 1,
        Numerico = 2,
        Binario = 3,
        Porcetaje = 4
    }

    //Tipos de datos:
    // 1 = texto libre
    // 2 = numerico
    // 3 = booleano
    // 4 = porcentual  
}
