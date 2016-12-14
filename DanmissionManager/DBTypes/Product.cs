namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Drawing;
    using System.Windows.Media.Imaging;

    [Table("data.products")]
    public partial class Product
    {
        public Product()
        {
            
        }
        public Product(string name, int id, bool isUnique, string desc)
        {
            this.date = DateTime.Now;
            this.name = name;
            this.category = id;
            this.isUnique = isUnique;
            this.desc = desc;
        }
        public int id { get; set; }

        [Required]
        [StringLength(45)]
        public string name { get; set; }

        public double price { get; set; }

        [StringLength(255)]
        public string desc { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? date { get; set; }

        public bool isUnique { get; set; }

        public int? quantity { get; set; }

        public int category { get; set; }

        [Column(TypeName = "mediumblob")]
        public byte[] image { get; set; }

        public DateTime? expiredate { get; set; }

        [NotMapped]
        public BitmapImage productImage { get; set; }


        public Product ShallowCopy()
        {
            return (Product)this.MemberwiseClone();
        }
    }
}
