namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMConfigStage
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StageId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string StageName { get; set; }
    }
}
