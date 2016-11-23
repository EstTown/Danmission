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

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Standardprice> Standardprices { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.subname)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<Standardprice>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.date)
                .IsUnicode(false);
        }
    }
}
