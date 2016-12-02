namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.transactions")]
    public partial class Transaction
    {
        public int id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? date { get; set; }

        public double sum { get; set; }
    }
}
