namespace WpfAppEmp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Column(TypeName = "text")]
        public string emp_name { get; set; }

        public int? emp_age { get; set; }

        public double? emp_salary { get; set; }

        public DateTimeOffset? join_date { get; set; }

        [StringLength(50)]
        public string phone { get; set; }
    }
}
