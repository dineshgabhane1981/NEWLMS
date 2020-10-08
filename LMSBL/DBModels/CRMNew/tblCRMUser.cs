namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCRMUser
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string VisaCountry { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string OtherName { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string AlternateEmail { get; set; }

        [StringLength(50)]
        public string SecondaryEmail { get; set; }

        [StringLength(50)]
        public string OtherEmail { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(20)]
        public string MobileNoCountry { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string SecondaryMobile { get; set; }

        [StringLength(50)]
        public string OverseasMobile { get; set; }

        [StringLength(50)]
        public string LandLine { get; set; }

        [StringLength(250)]
        public string OtherContactInfo { get; set; }

        [StringLength(50)]
        public string MaritalStatus { get; set; }

        public int? DependentChildren { get; set; }

        public int? CurrentStage { get; set; }

        public int? CurrentSubStage { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? ClientId { get; set; }

        [StringLength(100)]
        public string Qualification { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(100)]
        public string WhereDidYouFindUs { get; set; }
    }
}
