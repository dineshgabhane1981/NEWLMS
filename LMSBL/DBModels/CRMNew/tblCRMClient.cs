namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMClient
    {
        [Key]
        public int ClientID { get; set; }

        [Required]
        [StringLength(250)]
        public string ClientName { get; set; }

        [StringLength(500)]
        public string ClientLogo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool? isLMS { get; set; }

        public int? TenantId { get; set; }

        public string Address { get; set; }
    }
}
