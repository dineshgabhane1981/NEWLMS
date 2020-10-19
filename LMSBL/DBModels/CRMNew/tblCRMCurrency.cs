namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRMCurrency")]
    public partial class tblCRMCurrency
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CurrencyId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CurrencyName { get; set; }
    }
}
