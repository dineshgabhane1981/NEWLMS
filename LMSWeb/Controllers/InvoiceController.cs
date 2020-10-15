using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using LMSBL.Common;
using LMSBL.DBModels.CRMNew;
using LMSBL.DBModels;
using LMSBL.Repository;
using LMSWeb.ViewModel;
using System.Threading;

namespace LMSWeb.Controllers
{
    public class InvoiceController : Controller
    {
        CRMNotesRepository crmnr = new CRMNotesRepository();
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        // GET: Invoice
        public ActionResult Index()
        {
            CRMInvoiceViewModel CRMInvoiceModelView = new CRMInvoiceViewModel();
            tblCRMInvoice objNewInvoice = new tblCRMInvoice();
            CRMInvoiceModelView.ObjCRMInvoivce = objNewInvoice;
            TblUser sessionUser = (TblUser)Session["UserSession"];
            CRMInvoiceModelView.lstCRMclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            return View(CRMInvoiceModelView);
        }
    }
}