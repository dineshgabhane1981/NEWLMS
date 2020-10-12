using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.DBModels.CRMNew;
using LMSBL.Repository;
using LMSWeb.ViewModel;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using LMSWeb.App_Start;

namespace LMSWeb.Controllers
{
    public class EnquiryFormController : Controller
    {
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        CRMDocumentsRepository crmDocRepository = new CRMDocumentsRepository();
        // GET: EnquiryForm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult websiteform(string id)
        {            
            EnquiryFormViewModel enquiryFormViewModel = new EnquiryFormViewModel();                       
            enquiryFormViewModel.UserCountryList = crmUsersRepository.GetCountries();
            enquiryFormViewModel.WhereDidYouFindUsList = crmUsersRepository.WhereDidYouFindUs();
            return View(enquiryFormViewModel);
        }

        [HttpPost, ValidateInput(false)]
        public bool addenquiry(EnquiryFormViewModel objEnquiryFormViewModel)
        {
            CommonFunctions common = new CommonFunctions();
            string clientId = common.DecryptString(objEnquiryFormViewModel.ClientId);
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objEnquiryFormViewModel.ObjCRMUser.CreatedBy = sessionUser.UserId;
            objEnquiryFormViewModel.ObjCRMUser.CreatedOn = DateTime.Now;
            objEnquiryFormViewModel.ObjCRMUser.CurrentStage = 1;
            objEnquiryFormViewModel.ObjCRMUser.ClientId = Convert.ToInt32(clientId);
            var status = crmUsersRepository.AddEnquiryFromWebsite(objEnquiryFormViewModel.ObjCRMUser, objEnquiryFormViewModel.uploadFile, objEnquiryFormViewModel.FileName);

            return true;
        }
    }
}