using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using LMSWeb.App_Start;
using System.Net;
using System.Collections;
using LMSWeb.ViewModel;
using LMSBL.DBModels.CRMNew;

namespace LMSWeb.Controllers
{
    
    public class AdminSettingsController : Controller
    {
        UserRepository ur = new UserRepository();
        RolesRepository rr = new RolesRepository();
        TenantRepository tr = new TenantRepository();
        Exceptions newException = new Exceptions();
        CRMRepository cr = new CRMRepository();
        CRMUsersRepository cur = new CRMUsersRepository();
        // GET: AdminSettings
        public ActionResult Index()
        {
                
            LMSBL.DBModels.TblUser model = new LMSBL.DBModels.TblUser();
            model = (LMSBL.DBModels.TblUser)Session["UserSession"];
            TblUserViewModel objuserviewmodel = new TblUserViewModel();
            List<TblUser> userDetails = new List<TblUser>();
            userDetails = ur.GetUserById(model.UserId);
            TblUser objEditData = new TblUser();
            objEditData = userDetails[0];
            objEditData.UserRoles = rr.GetAllRoles();            
            objEditData.IsMyProfile = true;
            List<tblCRMClient> clientdetails = new List<tblCRMClient>();
            clientdetails = cr.GetClientById(Convert.ToInt32(model.CRMClientId));
            tblCRMClient objcrmclient = new tblCRMClient();
            objcrmclient = clientdetails[0];
            objuserviewmodel.objtblCRMClient = objcrmclient;
            objuserviewmodel.objtbluser = objEditData;
            List<tblCRMClientStage> stagesdetails = new List<tblCRMClientStage>();
            List<tblCRMClientSubStage> substagesdetails = new List<tblCRMClientSubStage>();
            stagesdetails = cur.GetCRMClientStages(Convert.ToInt32(model.CRMClientId));
            substagesdetails = cur.GetCRMClientSubStagesAll(Convert.ToInt32(model.CRMClientId));
            objuserviewmodel.lstCRMClientStage = stagesdetails;
            objuserviewmodel.lstCRMClientSubStage = substagesdetails;
            return View("Index", objuserviewmodel);

            


        }
        [HttpPost]
        public ActionResult UpdateUser(TblUserViewModel objUserviewmodel, HttpPostedFileBase file)
        {
            LMSBL.DBModels.TblUser model = new LMSBL.DBModels.TblUser();
            model = (LMSBL.DBModels.TblUser)Session["UserSession"];
            int rows = 0;
            bool ResultUpdate;

            if (file != null)
            {
                //var profileURL = System.Configuration.ConfigurationManager.AppSettings["ProfileImages"];
                //var profilePhysicalURL = System.Configuration.ConfigurationManager.AppSettings["ProfileImagesPhysicalURL"];

                //if (!System.IO.Directory.Exists(profilePhysicalURL + "\\" + objUser.TenantId))
                //{
                //    System.IO.Directory.CreateDirectory(profilePhysicalURL + "\\" + objUser.TenantId);
                //}

                //string filePhysicalPath = System.IO.Path.Combine(profilePhysicalURL + "\\" + objUser.TenantId + "\\" + objUser.UserId + ".jpg");
                //string path = System.IO.Path.Combine(profileURL + "\\" + objUser.TenantId + "\\" + objUser.UserId + ".jpg");
                //file.SaveAs(filePhysicalPath);
                //objUser.profileImage = path;
            }

            rows = ur.EditUser(objUserviewmodel.objtbluser);
            if(!string.IsNullOrEmpty(objUserviewmodel.objtblCRMClient.ClientLogo))
            {
                ResultUpdate = cr.UpdateCRMClient(Convert.ToInt32(model.CRMClientId), objUserviewmodel.objtblCRMClient.ClientLogo);
            }
            if (objUserviewmodel.objtbluser.IsMyProfile)
            {
                if (!string.IsNullOrEmpty(objUserviewmodel.objtbluser.OldPassword) && !string.IsNullOrEmpty(objUserviewmodel.objtbluser.Password))
                {
                    CommonFunctions common = new CommonFunctions();
                    objUserviewmodel.objtbluser.OldPassword = common.GetEncodePassword(objUserviewmodel.objtbluser.OldPassword);
                    objUserviewmodel.objtbluser.Password = common.GetEncodePassword(objUserviewmodel.objtbluser.Password);
                    var result = ur.ChangePassword(objUserviewmodel.objtbluser, objUserviewmodel.objtbluser.Password);

                }

            }
            if (objUserviewmodel.objtbluser.IsMyProfile)
            {
                var userDetails = ur.GetUserById(model.UserId);
                Session["UserSession"] = userDetails[0];
                TempData["Message"] = "User Information Saved Successfully";
                // return RedirectToAction("MyProfile", "Account");
            }
            //In case of Edit User
            if (rows == 0)
            {
                TempData["IssueMessage"] = "Not Saved Successfully";
                // return View("AddNewUser", objUserviewmodel.objtbluser);
            }
            else
            {
                TempData["UserMessage"] = "Saved Successfully";
                // return View("AddNewUser", objUserviewmodel.objtbluser);
            }

               return View("Index", objUserviewmodel.objtbluser);
            }

        public bool UpdateStages(TblUserViewModel objuserviewmodel,string id)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var status = cur.UpdateStageName(Convert.ToInt32(sessionUser.CRMClientId), objuserviewmodel.objtblCRMClientStage.StageName, objuserviewmodel.objtblCRMClientStage.StageId);
            
            return status;
        }
        }
    }