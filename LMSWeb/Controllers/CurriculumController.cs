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
        CurriculumRepository cc = new CurriculumRepository();
        Exceptions newException = new Exceptions();
        // GET: Curriculum
        public ActionResult Index()
        {
            List<tblCurriculum> lstCurriculum = new List<tblCurriculum>();
            return View(lstCurriculum);
        }
        public ActionResult AddCurriculum()
        {
            tblCurriculum objCurriculum = new tblCurriculum();
            return View(objCurriculum);
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

        public ActionResult testing()
        {

            tblCurriculum objCurriculum = new tblCurriculum();
            return View(objCurriculum);
        }

      
        public ActionResult AddCurriculumToDB(string selectedType)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(selectedType);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}