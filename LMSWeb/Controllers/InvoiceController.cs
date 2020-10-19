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
using System.Web.Script.Serialization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using IronPdf;

namespace LMSWeb.Controllers
{
    public class InvoiceController : Controller
    {
        CRMNotesRepository crmnr = new CRMNotesRepository();
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        CRMInvoiceRepository invoiceRepository = new CRMInvoiceRepository();
        // GET: Invoice
        public ActionResult Index()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            CRMInvoiceViewModel CRMInvoiceModelView = new CRMInvoiceViewModel();
            tblCRMInvoice objNewInvoice = new tblCRMInvoice();
            //objNewInvoice.InvoiceNumber = invoiceRepository.GetInvoiceNumber(Convert.ToInt32(sessionUser.CRMClientId));
            CRMInvoiceModelView.ObjCRMInvoivce = objNewInvoice;

            CRMInvoiceModelView.lstCRMCurriencies = invoiceRepository.GetCRMCurriencies();
            CRMInvoiceModelView.lstCRMclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            return View(CRMInvoiceModelView);
        }

        [HttpPost]
        public bool AddInvoice(CRMInvoiceViewModel CRMInvoiceModelView)
        {
            bool result = false;
            TblUser sessionUser = (TblUser)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objInvoiceData = (object[])json_serializer.DeserializeObject(CRMInvoiceModelView.JsonData);

            CRMInvoiceModelView.ObjCRMInvoivce.CreatedBy = sessionUser.UserId;
            CRMInvoiceModelView.ObjCRMInvoivce.CreatedOn = DateTime.Now;
            CRMInvoiceModelView.ObjCRMInvoivce.UpdatedBy = sessionUser.UserId;
            CRMInvoiceModelView.ObjCRMInvoivce.UpdatedOn = DateTime.Now;

            List<tblCRMInvoiceItem> lstInvoiceItems = new List<tblCRMInvoiceItem>();
            foreach (Dictionary<string, object> item in objInvoiceData)
            {
                tblCRMInvoiceItem InvoiveItem = new tblCRMInvoiceItem();
                InvoiveItem.ItemDescription = Convert.ToString(item["ItemDesc"]);
                InvoiveItem.Price = Convert.ToDecimal(item["ItemPrice"]);
                InvoiveItem.Amount = Convert.ToDecimal(item["ItemAmount"]);
                lstInvoiceItems.Add(InvoiveItem);
            }

            result = invoiceRepository.SaveInvoice(CRMInvoiceModelView.ObjCRMInvoivce, lstInvoiceItems);

            return result;
        }

        public string GetInvoiceNumber(int clientId)
        {
            string invoiceNo = string.Empty;
            invoiceNo = invoiceRepository.GetInvoiceNumber(Convert.ToInt32(clientId));
            return invoiceNo;
        }

        public ActionResult GetInvoices(int clientId)
        {
            CRMInvoiceViewModel CRMInvoiceModelView = new CRMInvoiceViewModel();
            CRMInvoiceModelView.ObjCRMInvoivceLST = invoiceRepository.GetInvoices(clientId);
            return PartialView("_InvoiceList", CRMInvoiceModelView);
        }

        public ActionResult GetInvoiceForEdit(int InvoiceId)
        {
            CRMInvoiceViewModel CRMInvoiceModelView = new CRMInvoiceViewModel();
            CRMInvoiceModelView.ObjCRMInvoivce = invoiceRepository.GetInvoiceForEdit(InvoiceId);
            CRMInvoiceModelView.ObjCRMInvoiceItemLST = invoiceRepository.GetInvoiceItemForEdit(InvoiceId);

            return Json(CRMInvoiceModelView, JsonRequestBehavior.AllowGet);
        }

        public bool MarkPaidInvoice(int invoiceId)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var status = invoiceRepository.MarkPaidInvoice(invoiceId, sessionUser.UserId);
            return status;
        }

       
        public ActionResult DownloadInvoice(int InvoiceId)
        {
            //var Renderer = new IronPdf.HtmlToPdf();
            //var PDF = Renderer.RenderUrlAsPdf("https://en.wikipedia.org/wiki/Portable_Document_Format");
            //PDF.SaveAs(Server.MapPath("/assets/") + "wikipedia.pdf");
            CRMInvoiceViewModel CRMInvoiceModelView = new CRMInvoiceViewModel();
            CRMInvoiceModelView.ObjCRMInvoivce = invoiceRepository.GetInvoiceForEdit(InvoiceId);
            CRMInvoiceModelView.ObjCRMInvoiceItemLST = invoiceRepository.GetInvoiceItemForEdit(InvoiceId);
            CRMInvoiceModelView.ObjCRMUser = crmUsersRepository.GetCRMUserById(CRMInvoiceModelView.ObjCRMInvoivce.ClientId);
            CRMInvoiceModelView.ObjCRMClient = crmUsersRepository.GetClientDetails(Convert.ToInt32(CRMInvoiceModelView.ObjCRMUser.ClientId));




            return View(CRMInvoiceModelView);
        }


        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}