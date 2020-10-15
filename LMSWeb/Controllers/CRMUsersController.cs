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
using LMSWeb.App_Start;

namespace LMSWeb.Controllers
{
    public class CRMUsersController : Controller
    {
        CRMNotesRepository crmNotesRepository = new CRMNotesRepository();
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        CRMDocumentsRepository crmDocRepo = new CRMDocumentsRepository();
        public ActionResult Enquiry()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId), 1);
            ViewBag.StageForButton = 1;
            return View(listingViewModel);
        }
        public ActionResult AddEnquiry(string myid)
        {

            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            if (!string.IsNullOrEmpty(myid))
            {
                //Edit mode
                CommonFunctions common = new CommonFunctions();
                myid = common.DecryptString(myid);                
                int userId = Convert.ToInt32(myid);
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
        public ActionResult AddPotentialClient(string myid)
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            if (!string.IsNullOrEmpty(myid))
            {
                //Edit mode
                CommonFunctions common = new CommonFunctions();
                myid = common.DecryptString(myid);
                int userId = Convert.ToInt32(myid);
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

            return View("AddEnquiry", objCRMUserViewModel);
        }

        public ActionResult Clients()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            CRMClientViewModel objCRMClientViewModel = new CRMClientViewModel();
            objCRMClientViewModel.lstClientSubStages = crmUsersRepository.GetCRMClientSubStages(Convert.ToInt32(sessionUser.CRMClientId));
            objCRMClientViewModel.objClientTicketLST = crmUsersRepository.GetCRMTicketsAll(Convert.ToInt32(sessionUser.CRMClientId), 3);
            //List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            //listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId), 3);
            ViewBag.StageForButton = 3;
            return View(objCRMClientViewModel);
        }

        public ActionResult AddClient(string myid, string clone)
        {
            CRMUserViewModel objCRMUserViewModel = new CRMUserViewModel();
            if (!string.IsNullOrEmpty(myid))
            {
                //Edit mode
                CommonFunctions common = new CommonFunctions();
                myid = common.DecryptString(myid);
                int userId = Convert.ToInt32(myid);
                objCRMUserViewModel = LoadModel(userId);
                objCRMUserViewModel.Clone = clone;

            }
            else
            {
                //Add mode
                objCRMUserViewModel = FillAlldropdownLists(objCRMUserViewModel);
                tblCRMUser ObjCRMUser = new tblCRMUser();
                ObjCRMUser.CurrentStage = 3;
                objCRMUserViewModel.ObjCRMUser = ObjCRMUser;
            }

            return View("AddEnquiry", objCRMUserViewModel);

        }

        [HttpPost]
        public bool AddCRMUser(CRMUserViewModel objCRMUserViewModel)
        {
            var status = false;
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objCRMUserViewModel.ObjCRMUser.CreatedBy = sessionUser.UserId;
            objCRMUserViewModel.ObjCRMUser.CreatedOn = DateTime.Now;
            objCRMUserViewModel.ObjCRMNote.CreatedDate = DateTime.Now;
            objCRMUserViewModel.ObjCRMNote.CreatedBy = sessionUser.UserId;
            objCRMUserViewModel.ObjCRMUser.ClientId = Convert.ToInt32(sessionUser.CRMClientId);
            if (!string.IsNullOrEmpty(objCRMUserViewModel.Clone))
            {
                status = crmUsersRepository.CloneUserData(objCRMUserViewModel.ObjCRMUser, objCRMUserViewModel.ObjCRMUsersVisaDetail, objCRMUserViewModel.ObjCRMUsersINZLoginDetail);
            }
            else
            {
                status = crmUsersRepository.SaveUserData(objCRMUserViewModel.ObjCRMUser, objCRMUserViewModel.ObjCRMUsersBillingAddress, objCRMUserViewModel.ObjCRMUsersPassportDetail,
                    objCRMUserViewModel.ObjCRMUsersVisaDetail, objCRMUserViewModel.ObjCRMUsersMedicalDetail,
                    objCRMUserViewModel.ObjCRMUsersPoliceCertificateInfo, objCRMUserViewModel.ObjCRMUsersINZLoginDetail,
                    objCRMUserViewModel.ObjCRMUsersNZQADetail, objCRMUserViewModel.ObjCRMNote);

            }
            return status;
        }

        [HttpPost]
        public ActionResult MoveUser(int id, int stage, int currentstage)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];

            var result = crmUsersRepository.UpdateStage(id, stage);
            List<EnquiryListing> listingViewModel = new List<EnquiryListing>();
            listingViewModel = crmUsersRepository.GetCRMUsersAll(Convert.ToInt32(sessionUser.CRMClientId), currentstage);
            ViewBag.StageForButton = currentstage;
            return PartialView("_EnquiryList", listingViewModel);
        }

        [HttpPost]
        public string MoveUserToSubStage(int uId, int sId)
        {
            List<string> lstResult = new List<string>();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var currentUserStage = crmUsersRepository.GetCRMUserById(uId);
            lstResult.Add(Convert.ToString(currentUserStage.CurrentSubStage));

            var result = crmUsersRepository.UpdateSubStage(uId, sId);

            var currentNoOfUsers = crmUsersRepository.GetCRMUsersBySubStageId(Convert.ToInt32(sessionUser.CRMClientId), Convert.ToInt32(currentUserStage.CurrentSubStage));
            lstResult.Add(Convert.ToString(currentNoOfUsers.Count));

            var noOfUsers = crmUsersRepository.GetCRMUsersBySubStageId(Convert.ToInt32(sessionUser.CRMClientId), sId);
            lstResult.Add(Convert.ToString(noOfUsers.Count));

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            var jsonData = json_serializer.Serialize(lstResult);

            return jsonData;
        }


        public CRMUserViewModel LoadModel(int userId)
        {
            CRMUserViewModel objModel = new CRMUserViewModel();
            objModel = FillAlldropdownLists(objModel);
            objModel.ObjCRMUser = crmUsersRepository.GetCRMUserById(userId);
            objModel.ObjCRMUsersBillingAddress = crmUsersRepository.GetCRMUserBillingAddressById(userId);
            objModel.ObjCRMUsersPassportDetail = crmUsersRepository.GetCRMUserPassportDetailById(userId);
            objModel.ObjCRMUsersVisaDetail = crmUsersRepository.GetCRMUserVisaDetailById(userId);
            objModel.ObjCRMUsersMedicalDetail = crmUsersRepository.GetCRMUserMedicalDetailById(userId);
            objModel.ObjCRMUsersPoliceCertificateInfo = crmUsersRepository.GetCRMUserPoliceCertificateInfoById(userId);
            objModel.ObjCRMUsersINZLoginDetail = crmUsersRepository.GetCRMUserINZLoginDetailById(userId);
            objModel.ObjCRMUsersNZQADetail = crmUsersRepository.GetCRMUserNZQADetailById(userId);
            objModel.ObjCRMNoteLST = crmUsersRepository.GetCRMUserFileNotesById(userId);
            objModel.lstNotes = crmNotesRepository.GetCRMUserFileNotesById(userId);
            objModel.lstNotesSubStages = crmNotesRepository.GetCRMUserFileNotesSubStagesById(userId);
            objModel.ObjCRMDocumentLST = crmDocRepo.GetCRMDocumentList(userId);

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

            TblUser sessionUser = (TblUser)Session["UserSession"];
            objModel.SubStagesList = crmNotesRepository.GetCRMClientSubStages(Convert.ToInt32(sessionUser.CRMClientId));
            objModel.StagesList = crmUsersRepository.GetCRMStagesList(Convert.ToInt32(sessionUser.CRMClientId));
            return objModel;
        }

        [HttpPost, ValidateInput(false)]
        public bool adddocuments(string jsonData)
        {
            tblCRMDocument objDocument = new tblCRMDocument();
            string base64string = string.Empty;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objDocData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objDocData)
            {
                objDocument.ClientId = Convert.ToInt32(item["ClientId"]);
                objDocument.DocumentName = Convert.ToString(item["DocName"]);
                objDocument.DocumentfileName = Convert.ToString(item["FileName"]);                
                base64string = Convert.ToString(item["base64string"]);
            }
            bool status = false;
            TblUser sessionUser = (TblUser)Session["UserSession"];

            objDocument.UpdatedBy = sessionUser.UserId;
            objDocument.UpdatedDate = DateTime.Now;
            //objCRMDocumentsViewModel.objCRMDocument.DocumentLink = objCRMDocumentsViewModel.documentFileName;
            status = crmDocRepo.AddDocument(objDocument, base64string, objDocument.DocumentfileName);

            return status;
        }

    }
}