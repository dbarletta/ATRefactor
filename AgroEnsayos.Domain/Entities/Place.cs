namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Place
    {
        public Place()
        {
            Tests = new HashSet<Test>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [StringLength(30)]
        public string Region { get; set; }

        [StringLength(50)]
        public string Province { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Header { get; set; }

        [StringLength(50)]
        public string Locality { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Column(TypeName = "xml")]
        public string Perimeter { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
