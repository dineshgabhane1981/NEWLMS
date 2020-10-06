namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMConfigDocument
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(250)]
        public string DocumentName { get; set; }
    }
}
