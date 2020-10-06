namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUsersMedicalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string NZER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MedicalCertificateIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MedicalCertificateExpiryDate { get; set; }

        [StringLength(100)]
        public string MedicalGrading { get; set; }

        [Column(TypeName = "date")]
        public DateTime? XrayIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? XrayExpiryDate { get; set; }

        [StringLength(100)]
        public string XrayGrading { get; set; }

        [StringLength(20)]
        public string MedicalNotesDDL { get; set; }

        public string MedicalNotes { get; set; }
    }
}
