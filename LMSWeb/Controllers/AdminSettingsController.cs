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

namespace LMSWeb.Controllers
{
    
    public class AdminSettingsController : Controller
    {
        UserRepository ur = new UserRepository();
        RolesRepository rr = new RolesRepository();
        TenantRepository tr = new TenantRepository();
        Exceptions newException = new Exceptions();
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
            //objEditData.Tenants = tr.GetAllTenants();

            objEditData.IsMyProfile = true;
            objuserviewmodel.objtbluser = objEditData;
            return View("Index", objuserviewmodel);


        }
        [HttpPost]
        public ActionResult UpdateUser(TblUserViewModel objUserviewmodel)
        {
            LMSBL.DBModels.TblUser model = new LMSBL.DBModels.TblUser();
            model = (LMSBL.DBModels.TblUser)Session["UserSession"];
            int rows = 0;
            rows = ur.EditUser(objUserviewmodel.objtbluser);
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
        }
    }