namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.categories")]
    public partial class Category
    {
        public int id { get; set; }

        [Required]
        [StringLength(45)]
        public string name { get; set; }

        [NotMapped]
        public int Sum { get; set; }
    }
}
