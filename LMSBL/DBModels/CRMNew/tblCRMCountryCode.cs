namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMCountryCode
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string CountryCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string CountryName { get; set; }
    }
}
