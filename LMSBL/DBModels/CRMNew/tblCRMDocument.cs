namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMDocument
    {
        public int ClientId { get; set; }

        [Key]
        public int DocumentId { get; set; }

        [StringLength(250)]
        public string DocumentName { get; set; }

        [StringLength(250)]
        public string DocumentLink { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsDocumentLocation { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        [StringLength(250)]
        public string DocumentfileName { get; set; }
    }
}
