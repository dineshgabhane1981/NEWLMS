namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRMVisaType")]
    public partial class tblCRMVisaType
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VisaId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string VisaName { get; set; }
    }
}
