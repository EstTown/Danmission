namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.subcategories")]
    public partial class SubCategory
    {
        [Key]
        public int idsubcategories { get; set; }

        [Required]
        [StringLength(45)]
        public string corresponding_string { get; set; }

        [Required]
        [StringLength(45)]
        public string subcategory { get; set; }
    }
}
