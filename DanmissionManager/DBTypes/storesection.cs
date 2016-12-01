namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.storesection")]
    public partial class Storesection
    {
        public int id { get; set; }

        public double? space { get; set; }

        public int? category { get; set; }
    }
}
