using LMSBL.Repository;
using LMSBL.DBModels.CRMNew;
using LMSWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.DBModels;

namespace LMSWeb.Controllers
{
    public class CRMUsersController : Controller
    {
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        // GET: Clients
        public ActionResult Enquiry()
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();

            return View(objCRMUserViewModel);
        }
        public ActionResult AddEnquiry()
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            tblCRMUser ObjCRMUser = new tblCRMUser();
            ObjCRMUser.CurrentStage = 1;
            objCRMUserViewModel.ObjCRMUser = ObjCRMUser;

            objCRMUserViewModel.VisaCountriesList = crmUsersRepository.GetVisaCountries();
            objCRMUserViewModel.CountriesCodes= crmUsersRepository.GetCountryCodes();
            objCRMUserViewModel.WhereDidYouFindUsList = crmUsersRepository.WhereDidYouFindUs();
            objCRMUserViewModel.JobSectorsList = crmUsersRepository.GetJobSector();
            objCRMUserViewModel.UserCountryList = crmUsersRepository.GetCountries();
            objCRMUserViewModel.CurrentVisaTypeList = crmUsersRepository.GetVisaType();
            objCRMUserViewModel.VisaStatusList = crmUsersRepository.GetVisaStatus();
           
            return View(objCRMUserViewModel);
        }
        public ActionResult PotentialClients()
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            objCRMUserViewModel.ObjCRMUser.CurrentStage = 2;
            return View();
        }
        public ActionResult Clients()
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            objCRMUserViewModel.ObjCRMUser.CurrentStage = 3;
            return View();
        }

        //[HttpPost, ValidateInput(false)]
        [HttpPost]
        public bool AddCRMUser(CRMUserViewModel objCRMUserViewModel)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objCRMUserViewModel.ObjCRMUser.CreatedBy = sessionUser.UserId;
            objCRMUserViewModel.ObjCRMUser.CreatedOn = DateTime.Now;
            objCRMUserViewModel.ObjCRMUser.ClientId = Convert.ToInt32(sessionUser.CRMClientId);

            var status = crmUsersRepository.SaveUserData(objCRMUserViewModel.ObjCRMUser, objCRMUserViewModel.ObjCRMUsersOtherInfo, 
                objCRMUserViewModel.ObjCRMUsersBillingAddress, objCRMUserViewModel.ObjCRMUsersPassportDetail, 
                objCRMUserViewModel.ObjCRMUsersVisaDetail, objCRMUserViewModel.ObjCRMUsersMedicalDetail, 
                objCRMUserViewModel.ObjCRMUsersPoliceCertificateInfo, objCRMUserViewModel.ObjCRMUsersINZLoginDetail, 
                objCRMUserViewModel.ObjCRMUsersNZQADetail);


            return status;
        }
    }
}