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
        CRMDocumentsRepository crmDocRepo = new CRMDocumentsRepository();
        // GET: Documents
        public ActionResult Index()
        {
            CRMDocumentsViewModel objCRMDocument = new CRMDocumentsViewModel();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            objCRMDocument.lstCRMclient = lstclient;
            return View(objCRMDocument);
        }

        [HttpPost, ValidateInput(false)]
        public bool adddocuments(CRMDocumentsViewModel objCRMDocumentsViewModel)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objCRMDocumentsViewModel.objCRMDocument.UpdatedBy = sessionUser.UserId;
            objCRMDocumentsViewModel.objCRMDocument.UpdatedDate = DateTime.Now;
            objCRMDocumentsViewModel.objCRMDocument.DocumentLink = objCRMDocumentsViewModel.documentFileName;
            var status = crmDocRepo.AddDocument(objCRMDocumentsViewModel.objCRMDocument, objCRMDocumentsViewModel.documentData, objCRMDocumentsViewModel.documentFileName);
           
            return status;
        }

        public ActionResult LoadDocuments(int Id)
        {
            CRMDocumentsViewModel objCRMDocumentsViewModel = new CRMDocumentsViewModel();
            var docList = crmDocRepo.GetCRMDocumentList(Id);
            objCRMDocumentsViewModel.objCRMDocumentLST = docList;            
            return PartialView("_documentList", objCRMDocumentsViewModel);
        }

        public ActionResult DownloadDocuments(int Id)
        {            
            var path = crmDocRepo.DownloadFileFromS3(Id);            
            return null;
        }

        public bool SaveDocumentLink(int Id, string link)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var status = crmDocRepo.SaveDocumentLink(Id,link, sessionUser.UserId);
            return status;
        }
        public string GetDocumentLink(int Id)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var link = crmDocRepo.GetDocumentLink(Id);
            return link;
        }

        public string DownloadAllDocuments(int Id)
        {
            var path = crmDocRepo.DownloadAllFileFromS3(Id);
            return path;
        }
    }
}