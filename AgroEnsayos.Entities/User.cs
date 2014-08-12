using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int EmpresaId { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }

        public string Empresa { get; set; }

        public List<Categoria> Categorias { get; set; }

    }
}
