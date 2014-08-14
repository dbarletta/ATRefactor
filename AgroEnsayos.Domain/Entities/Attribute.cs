namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Attribute
    {
        public Attribute()
        {
            AttributeMappings = new HashSet<AttributeMapping>();
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Family { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public byte DataType { get; set; }

        [StringLength(200)]
        public string Tags { get; set; }

        public bool IsFilter { get; set; }

        public bool IsDisabled { get; set; }

        public virtual ICollection<AttributeMapping> AttributeMappings { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
