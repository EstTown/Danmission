using DanmissionManager.DBTypes.NewFolder1;

namespace DanmissionManager
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ServerContext : DbContext
    {
        public ServerContext()
            : base("name=ServerContext")
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SoldProduct> Soldproducts { get; set; }
        public virtual DbSet<Standardprice> Standardprices { get; set; }
        public virtual DbSet<Storesection> Storesection { get; set; }
        public virtual DbSet<SubCategory> Subcategories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<SoldProduct>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<SoldProduct>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<Standardprice>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Standardprice>()
                .Property(e => e.CorrespondingCategoryString)
                .IsUnicode(false);

            modelBuilder.Entity<SubCategory>()
                .Property(e => e.corresponding_string)
                .IsUnicode(false);

            modelBuilder.Entity<SubCategory>()
                .Property(e => e.subcategory)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.date)
                .IsUnicode(false);
        }
    }
}
