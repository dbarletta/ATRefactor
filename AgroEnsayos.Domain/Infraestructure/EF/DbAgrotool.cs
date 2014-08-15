using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.Repositories;

namespace AgroEnsayos.Domain.Infraestructure.EF
{    

    public partial class DbAgrotool : DbContext
    {
        public DbAgrotool()
            : base("name=DbAgrotool")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<AttributeMapping> AttributeMappings { get; set; }
        public virtual DbSet<Domain.Entities.Attribute> Attributes { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Locality> Localities { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttributeMapping>()
                .HasMany(e => e.Products)
                .WithMany(e => e.AttributeMappings)
                .Map(m => m.ToTable("ProductAttribute").MapLeftKey("AttributeMappingId").MapRightKey("ProductId"));

            modelBuilder.Entity<Domain.Entities.Attribute>()
                .HasMany(e => e.AttributeMappings)
                .WithRequired(e => e.Attribute)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Domain.Entities.Attribute>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Attributes)
                .Map(m => m.ToTable("AttributeCategory").MapLeftKey("AttributeId").MapRightKey("CategoryId"));
            
            modelBuilder.Entity<Campaign>()
                .HasMany(e => e.Tests)
                .WithRequired(e => e.Campaign)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Campaigns)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Place>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Places)
                .Map(m => m.ToTable("ProductPlace").MapLeftKey("PlaceId").MapRightKey("ProductId"));
            
            modelBuilder.Entity<Product>()
                .HasMany(e => e.Tests)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
                .HasMany(u => u.CategoriesOfIntrest)
                .WithMany(c => c.Users)
                .Map(m => m.MapLeftKey("UserId").MapRightKey("CategoryId").ToTable("UserCategory"));

        }
    }
}
