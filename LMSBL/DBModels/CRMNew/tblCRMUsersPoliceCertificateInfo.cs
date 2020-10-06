namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRMUsersPoliceCertificateInfo")]
    public partial class tblCRMUsersPoliceCertificateInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PoliceCertificateDate1 { get; set; }

        [StringLength(100)]
        public string PoliceCertificateCountry1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PoliceCertificateDate2 { get; set; }

        [StringLength(100)]
        public string PoliceCertificateCountry2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PoliceCertificateDate3 { get; set; }

        [StringLength(100)]
        public string PoliceCertificateCountry3 { get; set; }
    }
}
