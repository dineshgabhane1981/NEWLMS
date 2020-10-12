using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.DBModels.CRMNew;

namespace LMSWeb.ViewModel
{
    public class EnquiryFormViewModel
    {
        public tblCRMUser ObjCRMUser { get; set; }
        public SelectListItem[] MaritalStatus()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "Single" }, new SelectListItem() { Text = "Married" }, new SelectListItem() { Text = "Widowed" }, new SelectListItem() { Text = "Defacto" }, new SelectListItem() { Text = "Separated" }, new SelectListItem() { Text = "Divorced" } };
        }
        public List<SelectListItem> UserCountryList { get; set; }
        public List<SelectListItem> WhereDidYouFindUsList { get; set; }
       public  string uploadFile { get; set; }
        public string FileName { get; set; }
        public string ClientId { get; set; }

    }
}