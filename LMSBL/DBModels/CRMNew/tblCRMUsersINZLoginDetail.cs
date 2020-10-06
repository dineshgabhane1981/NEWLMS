namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUsersINZLoginDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string ClientNumber { get; set; }

        [StringLength(100)]
        public string ClientUserName { get; set; }

        [StringLength(100)]
        public string ClientPassword { get; set; }
    }
}
