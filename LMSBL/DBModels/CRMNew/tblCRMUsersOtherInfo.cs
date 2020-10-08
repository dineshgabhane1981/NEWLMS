namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRMUsersOtherInfo")]
    public partial class tblCRMUsersOtherInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string NationalId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleDate { get; set; }

        [StringLength(100)]
        public string SalesPerson { get; set; }

        [StringLength(250)]
        public string SourceDescription { get; set; }

        [StringLength(100)]
        public string JobSector { get; set; }

        [StringLength(100)]
        public string RegionYouResideIn { get; set; }

        [StringLength(100)]
        public string Qualification { get; set; }

        [StringLength(100)]
        public string WhereDidYouFindUs { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        public string Notes { get; set; }
    }
}
