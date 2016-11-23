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

        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<standardprice> standardprices { get; set; }
        public virtual DbSet<transaction> transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.subname)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<standardprice>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<transaction>()
                .Property(e => e.date)
                .IsUnicode(false);
        }
    }
}
