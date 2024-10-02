namespace EntitySchool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        public int StudentID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public decimal? AverageScore { get; set; }

        public int? FacultyID { get; set; }

        public virtual Faculty Faculty { get; set; }
    }
}
