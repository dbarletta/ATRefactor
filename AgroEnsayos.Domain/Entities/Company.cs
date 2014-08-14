namespace AgroEnsayos.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company
    {
        public Company()
        {
            Products = new HashSet<Product>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDisabled { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(100)]
        public string ZipCode { get; set; }

        [StringLength(250)]
        public string Locality { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(250)]
        public string LogoUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
