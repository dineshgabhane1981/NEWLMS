using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.DBModels.CRMNew;

namespace LMSWeb.ViewModel
{
    public class CRMUserViewModel
    {
        public tblCRMUser ObjCRMUser { get; set; }

        public tblCRMUsersBillingAddress ObjCRMUsersBillingAddress { get; set; }
        public tblCRMUsersPassportDetail ObjCRMUsersPassportDetail { get; set; }
        public tblCRMUsersVisaDetail ObjCRMUsersVisaDetail { get; set; }
        public tblCRMUsersMedicalDetail ObjCRMUsersMedicalDetail { get; set; }
        public tblCRMUsersPoliceCertificateInfo ObjCRMUsersPoliceCertificateInfo { get; set; }
        public tblCRMUsersINZLoginDetail ObjCRMUsersINZLoginDetail { get; set; }
        public tblCRMUsersNZQADetail ObjCRMUsersNZQADetail { get; set; }
        public tblCRMNote ObjCRMNote { get; set; }
        public List<tblCRMNote> ObjCRMNoteLST { get; set; }
        public List<FileNotesDetails> lstNotes { get; set; }
        public List<string> lstNotesSubStages { get; set; }

        public List<SelectListItem> VisaCountriesList { get; set; }
        public List<SelectListItem> CountriesCodes { get; set; }

        public List<SelectListItem> SalesPersonsList { get; set; }
        public List<SelectListItem> ClientSourceList { get; set; }
        public List<SelectListItem> JobSectorsList { get; set; }
        public List<SelectListItem> UserCountryList { get; set; }
        public List<SelectListItem> WhereDidYouFindUsList { get; set; }
        public List<SelectListItem> CurrentVisaTypeList { get; set; }
        public List<SelectListItem> VisaStatusList { get; set; }
        public List<SelectListItem> SubStagesList { get; set; }
        public List<SelectListItem> StagesList { get; set; }
        public SelectListItem[] Gender()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Male" }, new SelectListItem() { Text = "Female" } };
        }
        public SelectListItem[] MaritalStatus()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "Single" }, new SelectListItem() { Text = "Married" }, new SelectListItem() { Text = "Widowed" }, new SelectListItem() { Text = "Defacto" }, new SelectListItem() { Text = "Separated" }, new SelectListItem() { Text = "Divorced" } };
        }
        public SelectListItem[] MedicalGrading()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "A" }, new SelectListItem() { Text = "AB" }, new SelectListItem() { Text = "B" } };
        }
        public SelectListItem[] MedicalNotesDDL()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "ASH" }, new SelectListItem() { Text = "ASH with Conditions" }, new SelectListItem() { Text = "Not ASH" } };
        }
    }


}