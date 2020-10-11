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
        CRMUsersRepository crmUsersRepository = new CRMUsersRepository();
        public ActionResult Index()
        {            
            CRMNotesViewModel CRMNotesView = new CRMNotesViewModel();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstclient = crmnr.GetClient(Convert.ToInt32(sessionUser.CRMClientId));
            var lstSubStages = crmnr.GetCRMClientSubStages(Convert.ToInt32(sessionUser.CRMClientId));
            CRMNotesView.lstSubStages = lstSubStages;
            CRMNotesView.lstCRMclient = lstclient;           
            return View(CRMNotesView);
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
            CRMNotesViewModel objNotesViewModel = new CRMNotesViewModel();
            objNotesViewModel.lstNotes = crmnr.GetCRMUserFileNotesById(Id);
            objNotesViewModel.lstNotesSubStages = crmnr.GetCRMUserFileNotesSubStagesById(Id);
            var currentUser = crmUsersRepository.GetCRMUserById(Id);
                
            //TempData["CurrentStage"] = 3;
            ViewBag.CurrentStage = currentUser.CurrentSubStage;
            return PartialView("_NotesList", objNotesViewModel);
        }
    }
}