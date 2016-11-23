namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.transaction")]
    public partial class transaction
    {
        public int id { get; set; }

        [Required]
        [StringLength(45)]
        public string date { get; set; }
    }
}
