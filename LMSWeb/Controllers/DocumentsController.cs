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
    public class DocumentsController : Controller
    {
        CRMNotesRepository crmnr = new CRMNotesRepository();
        // GET: Documents
        public ActionResult Index()
        {
            CRMDocumentsViewModel objCRMDocument = new CRMDocumentsViewModel();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            objCRMDocument.lstCRMclient = lstclient;
            return View(objCRMDocument);
        }
    }
}