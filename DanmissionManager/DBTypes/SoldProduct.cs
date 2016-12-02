namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.soldproducts")]
    public partial class SoldProduct
    {

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

        public int category { get; set; }

        [Column(TypeName = "mediumblob")]
        public byte[] image { get; set; }

        public int transactionid { get; set; }

        public int previousid { get; set; }
    }
}
