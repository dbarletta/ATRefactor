namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public Product()
        {
            Tests = new HashSet<Test>();
            AttributeMappings = new HashSet<AttributeMapping>();
            Places = new HashSet<Place>();
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        public bool? IsHybrid { get; set; }

        [StringLength(50)]
        public string Cycle { get; set; }

        public bool? IsConventional { get; set; }

        public int? DaysToFlowering { get; set; }

        public int? DaysToMaturity { get; set; }

        public int? PlantHeight { get; set; }

        public bool? IsNew { get; set; }

        public int? Height { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EntryDate { get; set; }

        public bool IsDisabled { get; private set; }

        public virtual Category Category { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public virtual ICollection<AttributeMapping> AttributeMappings { get; set; }

        public virtual ICollection<Place> Places { get; set; }

        /// <summary>
        /// Disables the current product and updates the mappings
        /// </summary>
        public void Disable()
        {
            this.IsDisabled = true;
            //foreach(var map in AttributeMappings)
            //{
            //    map.OriginalValue = null;
            //}
        }
    }
}
