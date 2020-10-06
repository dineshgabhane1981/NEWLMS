namespace LMSBL.DBModels.CRMNew
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CRMContext : DbContext
    {
        public CRMContext()
            : base("name=CRMContext")
        {
        }

        public virtual DbSet<tblCRMClient> tblCRMClients { get; set; }
        public virtual DbSet<tblCRMUser> tblCRMUsers { get; set; }
        public virtual DbSet<tblCRMConfigDocument> tblCRMConfigDocuments { get; set; }
        public virtual DbSet<tblCRMConfigStage> tblCRMConfigStages { get; set; }
        public virtual DbSet<tblCRMConfigSubStage> tblCRMConfigSubStages { get; set; }
        public virtual DbSet<tblCRMCountry> tblCRMCountries { get; set; }
        public virtual DbSet<tblCRMCountryCode> tblCRMCountryCodes { get; set; }
        public virtual DbSet<tblCRMDocument> tblCRMDocuments { get; set; }
        public virtual DbSet<tblCRMNote> tblCRMNotes { get; set; }
        public virtual DbSet<tblCRMUsersBillingAddress> tblCRMUsersBillingAddresses { get; set; }
        public virtual DbSet<tblCRMUsersINZLoginDetail> tblCRMUsersINZLoginDetails { get; set; }
        public virtual DbSet<tblCRMUsersMedicalDetail> tblCRMUsersMedicalDetails { get; set; }
        public virtual DbSet<tblCRMUsersNZQADetail> tblCRMUsersNZQADetails { get; set; }
        public virtual DbSet<tblCRMUsersOtherInfo> tblCRMUsersOtherInfoes { get; set; }
        public virtual DbSet<tblCRMUsersPassportDetail> tblCRMUsersPassportDetails { get; set; }
        public virtual DbSet<tblCRMUsersPoliceCertificateInfo> tblCRMUsersPoliceCertificateInfoes { get; set; }
        public virtual DbSet<tblCRMUsersVisaDetail> tblCRMUsersVisaDetails { get; set; }
        public virtual DbSet<tblCRMVisaStatu> tblCRMVisaStatus { get; set; }
        public virtual DbSet<tblCRMVisaType> tblCRMVisaTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
