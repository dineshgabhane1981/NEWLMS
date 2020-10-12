namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUsersVisaDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string VisaType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CurrentNZVisaExpiryDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TravelConditionsValidTo { get; set; }

        [StringLength(100)]
        public string IntrestedVisa { get; set; }

        [StringLength(100)]
        public string UpdateVisaStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VisaExpiryDate { get; set; }

        [StringLength(250)]
        public string AnyPreviousVisaDecline { get; set; }
    }
}
