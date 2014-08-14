namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Campaign
    {
        public Campaign()
        {
            Tests = new HashSet<Test>();
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}
