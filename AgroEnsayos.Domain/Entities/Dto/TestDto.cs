using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroEnsayos.Domain.Entities.Dto
{
    public class TestDto
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        public int ProductId { get; set; }

        public int? PlaceId { get; set; }

        public decimal Yield { get; set; }

        public string Source { get; set; }

        public string Establishment { get; set; }

        public DateTime? PlantingDate { get; set; }

        public DateTime? HarvestDate { get; set; }

        public int? Index { get; set; }

        public string Observations { get; set; }

        public string File { get; set; }

        public virtual string CampaignName { get; set; }

        public virtual string PlaceProvince { get; set; }

        public virtual string PlaceLocality { get; set; }


        public virtual string ProductName { get; set; }

    }
}