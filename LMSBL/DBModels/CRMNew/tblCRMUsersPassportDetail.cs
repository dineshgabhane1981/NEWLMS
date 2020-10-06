namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUsersPassportDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string PassportNumber { get; set; }

        [StringLength(100)]
        public string CountryOfIssue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PassportIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PassportExpiryDate { get; set; }

        [StringLength(100)]
        public string SecondPassportNumber { get; set; }

        [StringLength(100)]
        public string SecondCountryOfIssue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SecondPassportIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SecondPassportExpiryDate { get; set; }
    }
}
