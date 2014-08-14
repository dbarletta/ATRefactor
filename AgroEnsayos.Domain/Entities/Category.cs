namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        public Category()
        {
            Campaigns = new HashSet<Campaign>();
            Products = new HashSet<Product>();
            Attributes = new HashSet<Attribute>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Attribute> Attributes { get; set; }
    }
}
