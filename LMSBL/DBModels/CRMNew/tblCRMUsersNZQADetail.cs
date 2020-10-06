namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUsersNZQADetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NZQAOnlineSubmissionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NZQADocumentsSubmissionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NZQADocumentsReturnedDate { get; set; }
    }
}
