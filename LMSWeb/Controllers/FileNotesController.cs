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

namespace LMSWeb.Controllers
{
    public class FileNotesController : Controller
    {
        // GET: FileNotes
        CRMNotesRepository crmnr = new CRMNotesRepository();
        public ActionResult Index()
        {
            //  tblCRMUser objclient = new tblCRMUser();
            CRMNotesViewModel CRMNotesView = new CRMNotesViewModel();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            CRMNotesView.lstCRMclient = lstclient;
            //objclient.ClientList = lstclient;
            return View(CRMNotesView);

           // return View();
        }

        public bool AddNotes(CRMNotesViewModel cRMNotesViewModel)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];            
            cRMNotesViewModel.objCRMnotes.CreatedDate = DateTime.Now;
            cRMNotesViewModel.objCRMnotes.CreatedBy = sessionUser.UserId;
            var status = crmnr.SaveUserData(cRMNotesViewModel.objCRMnotes);
            LoadNotes(cRMNotesViewModel.objCRMnotes.ClientId);

            return status;
        }

        
        public ActionResult LoadNotes(int Id)
        {
            CRMNotesViewModel objnotesvm = new CRMNotesViewModel();
            objnotesvm.lstNotes = crmnr.GetCRMUserFileNotesById(Id);
            return PartialView("_NotesList", objnotesvm);

        }
    }
}