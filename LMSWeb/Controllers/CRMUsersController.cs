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
using System.Web.Script.Serialization;

namespace LMSWeb.Controllers
{
    public class CRMUsersController : Controller
    {
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        // GET: Clients
        public ActionResult Enquiry()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId),1);
            ViewBag.StageForButton = 1;
            return View(listingViewModel);
        }
        public ActionResult AddEnquiry(string id)
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();           

            //objCRMUserViewModel.VisaCountriesList = crmUsersRepository.GetVisaCountries();
            //objCRMUserViewModel.CountriesCodes= crmUsersRepository.GetCountryCodes();
            //objCRMUserViewModel.WhereDidYouFindUsList = crmUsersRepository.WhereDidYouFindUs();
            //objCRMUserViewModel.JobSectorsList = crmUsersRepository.GetJobSector();
            //objCRMUserViewModel.UserCountryList = crmUsersRepository.GetCountries();
            //objCRMUserViewModel.CurrentVisaTypeList = crmUsersRepository.GetVisaType();
            //objCRMUserViewModel.VisaStatusList = crmUsersRepository.GetVisaStatus();            

            if(!string.IsNullOrEmpty(id))
            {
                //Edit mode
                int userId = Convert.ToInt32(id);
                objCRMUserViewModel = LoadModel(userId);                
            }            
            else
            {
                
                //Add mode
                objCRMUserViewModel = FillAlldropdownLists(objCRMUserViewModel);
                tblCRMUser ObjCRMUser = new tblCRMUser();
                ObjCRMUser.CurrentStage = 1;
                objCRMUserViewModel.ObjCRMUser = ObjCRMUser;
            }
           
            return View(objCRMUserViewModel);
        }
        public ActionResult PotentialClients()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId), 2);
            ViewBag.StageForButton = 2;
            return View("Enquiry", listingViewModel);
        }
        public ActionResult AddPotentialClient(string id)
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();

            //objCRMUserViewModel.VisaCountriesList = crmUsersRepository.GetVisaCountries();
            //objCRMUserViewModel.CountriesCodes = crmUsersRepository.GetCountryCodes();
            //objCRMUserViewModel.WhereDidYouFindUsList = crmUsersRepository.WhereDidYouFindUs();
            //objCRMUserViewModel.JobSectorsList = crmUsersRepository.GetJobSector();
            //objCRMUserViewModel.UserCountryList = crmUsersRepository.GetCountries();
            //objCRMUserViewModel.CurrentVisaTypeList = crmUsersRepository.GetVisaType();
            //objCRMUserViewModel.VisaStatusList = crmUsersRepository.GetVisaStatus();
            
            if (!string.IsNullOrEmpty(id))
            {
                //Edit mode
                int userId = Convert.ToInt32(id);
                objCRMUserViewModel = LoadModel(userId);
                
            }
            else
            {
                //Add mode
                objCRMUserViewModel = FillAlldropdownLists(objCRMUserViewModel);
                tblCRMUser ObjCRMUser = new tblCRMUser();
                ObjCRMUser.CurrentStage = 2;
                objCRMUserViewModel.ObjCRMUser = ObjCRMUser;
            }

            return View("AddEnquiry",objCRMUserViewModel);
        }

        public ActionResult Clients()
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            objCRMUserViewModel.ObjCRMUser.CurrentStage = 3;
            return View();
        }
                
        [HttpPost]
        public bool AddCRMUser(CRMUserViewModel objCRMUserViewModel)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objCRMUserViewModel.ObjCRMUser.CreatedBy = sessionUser.UserId;
            objCRMUserViewModel.ObjCRMUser.CreatedOn = DateTime.Now;
            objCRMUserViewModel.ObjCRMNote.CreatedDate = DateTime.Now;
            objCRMUserViewModel.ObjCRMNote.CreatedBy = sessionUser.UserId;
            objCRMUserViewModel.ObjCRMUser.ClientId = Convert.ToInt32(sessionUser.CRMClientId);

            var status = crmUsersRepository.SaveUserData(objCRMUserViewModel.ObjCRMUser, objCRMUserViewModel.ObjCRMUsersBillingAddress, objCRMUserViewModel.ObjCRMUsersPassportDetail, 
                objCRMUserViewModel.ObjCRMUsersVisaDetail, objCRMUserViewModel.ObjCRMUsersMedicalDetail, 
                objCRMUserViewModel.ObjCRMUsersPoliceCertificateInfo, objCRMUserViewModel.ObjCRMUsersINZLoginDetail, 
                objCRMUserViewModel.ObjCRMUsersNZQADetail, objCRMUserViewModel.ObjCRMNote);


            return status;
        }
        
        [HttpPost]
        public ActionResult MoveUser(int id, int stage,int currentstage)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];

            var result = crmUsersRepository.UpdateStage(id, stage);
            List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId), currentstage);
            return PartialView("_EnquiryList", listingViewModel);
        }

        public CRMUserViewModel LoadModel(int userId)
        {
            CRMUserViewModel objModel = new CRMUserViewModel();
            objModel = FillAlldropdownLists(objModel);
            objModel.ObjCRMUser = crmUsersRepository.GetCRMUserById(userId);
            //objCRMUserViewModel.ObjCRMUsersOtherInfo = crmUsersRepository.GetCRMUserOtherInfoById(userId);
            objModel.ObjCRMUsersBillingAddress = crmUsersRepository.GetCRMUserBillingAddressById(userId);
            objModel.ObjCRMUsersPassportDetail = crmUsersRepository.GetCRMUserPassportDetailById(userId);
            objModel.ObjCRMUsersVisaDetail = crmUsersRepository.GetCRMUserVisaDetailById(userId);
            objModel.ObjCRMUsersMedicalDetail = crmUsersRepository.GetCRMUserMedicalDetailById(userId);
            objModel.ObjCRMUsersPoliceCertificateInfo = crmUsersRepository.GetCRMUserPoliceCertificateInfoById(userId);
            objModel.ObjCRMUsersINZLoginDetail = crmUsersRepository.GetCRMUserINZLoginDetailById(userId);
            objModel.ObjCRMUsersNZQADetail = crmUsersRepository.GetCRMUserNZQADetailById(userId);
            objModel.ObjCRMNoteLST = crmUsersRepository.GetCRMUserFileNotesById(userId);

            return objModel;

        }

        public CRMUserViewModel FillAlldropdownLists(CRMUserViewModel objModel)
        {
            objModel.VisaCountriesList = crmUsersRepository.GetVisaCountries();
            objModel.CountriesCodes = crmUsersRepository.GetCountryCodes();
            objModel.WhereDidYouFindUsList = crmUsersRepository.WhereDidYouFindUs();
            objModel.JobSectorsList = crmUsersRepository.GetJobSector();
            objModel.UserCountryList = crmUsersRepository.GetCountries();
            objModel.CurrentVisaTypeList = crmUsersRepository.GetVisaType();
            objModel.VisaStatusList = crmUsersRepository.GetVisaStatus();
            return objModel;
        }
    }
}