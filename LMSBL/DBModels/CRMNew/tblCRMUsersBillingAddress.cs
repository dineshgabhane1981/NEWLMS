namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRMUsersBillingAddress")]
    public partial class tblCRMUsersBillingAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRMUserId { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string House { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(100)]
        public string Suburb { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string PostalCode { get; set; }
    }
}
