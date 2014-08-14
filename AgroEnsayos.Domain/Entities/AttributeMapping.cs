namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AttributeMapping
    {
        public AttributeMapping()
        {
            Products = new HashSet<Product>();
        }

        public int AttributeMappingId { get; set; }

        public int AttributeId { get; set; }

        [Required]
        [StringLength(100)]
        public string MappedValue { get; set; }

        [Required]
        [StringLength(100)]
        public string OriginalValue { get; set; }

        public byte Scale { get; set; }

        public virtual Attribute Attribute { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
