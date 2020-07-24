using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using LMSWeb.App_Start;
using LMSWeb.ViewModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;



namespace LMSWeb.Controllers
{
    public class EmailTemplateController : Controller
    {
        EmailTemplateRepository etr = new EmailTemplateRepository();
        Exceptions newException = new Exceptions();
        // GET: EmailTemplate
        public ActionResult Index()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstTemplate = etr.GetEmailTemplateAssigns(sessionUser.TenantId);
            return View(lstTemplate);

        }
        public ActionResult EditTemplate(int id)
        {
            try
            {
                List<tblTemplateAssign> TemplateDetails = new List<tblTemplateAssign>();
                TemplateDetails = etr.GetEmailTemplateAssignsById(id);

                return View("UpdateEmailTemplate", TemplateDetails[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                TempData["Message"] = "-1";
                return RedirectToAction("Index");

            }

        }
    }
}