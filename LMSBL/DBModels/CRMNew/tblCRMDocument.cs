namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMDocument
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(250)]
        public string DocumentName { get; set; }

        [StringLength(250)]
        public string DocumentLink { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime UpdatedDate { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UpdatedBy { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsReviewed { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }
    }
}
