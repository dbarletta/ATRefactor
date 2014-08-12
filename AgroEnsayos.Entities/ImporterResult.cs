using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class ImporterResult
    {
        public TipoError TipoError { get; set; }
        public Severity Severity { get; set; }
        public string Description { get; set; }

        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }

        public int Row { get; set; } 

    }

    public enum TipoError : int
    {
        CampoNulo = 1,
        SinDatoBD = 2,
        FormatoIncorrecto = 3,
        SinArchivo = 4,
        
    }

    public enum Severity : int
    {
        Information = 1,
        Warning = 2,
        Error = 3,
    }

}
