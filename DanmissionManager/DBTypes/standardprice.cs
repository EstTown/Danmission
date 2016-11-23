namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.standardprices")]
    public partial class Standardprice
    {
        public int id { get; set; }

        [Required]
        [StringLength(45)]
        public string name { get; set; }

        [Column("standardprice")]
        public double standardprice1 { get; set; }
    }
}
