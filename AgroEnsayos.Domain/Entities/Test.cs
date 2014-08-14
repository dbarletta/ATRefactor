namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Test
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        public int ProductId { get; set; }

        public int? PlaceId { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal Yield { get; set; }

        [Required]
        [StringLength(50)]
        public string Source { get; set; }

        [StringLength(50)]
        public string Establishment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PlantingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HarvestDate { get; set; }

        public int? Index { get; set; }

        [StringLength(100)]
        public string Observations { get; set; }

        [StringLength(100)]
        public string File { get; set; }

        public virtual Campaign Campaign { get; set; }

        public virtual Place Place { get; set; }

        public virtual Product Product { get; set; }
    }
}
