using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LMSWeb.ViewModel;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using System.Data;
using LMSBL;

namespace LMSWeb.Controllers
{
    public class CurriculumController : Controller
    {
        UserRepository userRepository = new UserRepository();
        CurriculumRepository cc = new CurriculumRepository();
        Exceptions newException = new Exceptions();
        // GET: Curriculum
        public ActionResult Index()
        {
            List<tblCurriculum> lstCurriculum = new List<tblCurriculum>();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            lstCurriculum = cc.GetCurriculumAll(sessionUser.TenantId);
            return View(lstCurriculum);
        }
        public ActionResult AddCurriculum()
        {
            tblCurriculum objCurriculum = new tblCurriculum();
            return View(objCurriculum);
        }

        public ActionResult EditCurriculum(int id)
        {
            var objCurriculum = cc.GetCurriculumById(id);
            return View("AddCurriculum", objCurriculum[0]);
        }

        public ActionResult GetActivities(string selectedType)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            List<Param> activityList = new List<Param>();
            if (selectedType == "1")
                activityList = cc.GetCurriculumCourses(sessionUser.TenantId);
            if (selectedType == "2")
                activityList = cc.GetCurriculumSurveys(sessionUser.TenantId);
            if (selectedType == "3")
                activityList = cc.GetCurriculumForums(sessionUser.TenantId);

            return Json(activityList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsers(string cId)
        {
            List<SelectListItem> userItems = new List<SelectListItem>();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            
            var Users = userRepository.GetAllUsers(sessionUser.TenantId);
            foreach (var user in Users)
            {
                userItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(user.FirstName + " " + user.LastName),
                    Value = Convert.ToString(user.UserId)
                });
            }


            DataSet ds = cc.GetCurriculumUsers(Convert.ToInt32(cId));

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (var item in userItems)
                        {
                            DataRow[] foundUsers = ds.Tables[0].Select("UserId = " + item.Value + "");
                            if (foundUsers.Length != 0)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }




            return Json(userItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult testing()
        {

            tblCurriculum objCurriculum = new tblCurriculum();
            return View(objCurriculum);
        }

      
        public ActionResult AddCurriculumToDB(string jsonData, string title,string CId)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            tblCurriculum objCurriculum = new tblCurriculum();
            objCurriculum.CreatedBy = sessionUser.UserId;
            objCurriculum.TenantId = sessionUser.TenantId;
            objCurriculum.CurriculumTitle = title;
            if(Convert.ToInt32(CId)>0)
            {
                objCurriculum.CurriculumId = Convert.ToInt32(CId);
            }
            
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = null;
            if (!string.IsNullOrEmpty(jsonData))
            {
                objData = (object[])json_serializer.DeserializeObject(jsonData);
            }
            var result = cc.AddCurriculumToDB(objData, objCurriculum);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}