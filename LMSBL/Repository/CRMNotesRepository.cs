using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSBL.Common;
using LMSBL.DBModels.CRMNew;
using System.Web.Mvc;
using System.Data.Entity;


namespace LMSBL.Repository
{
    public class CRMNotesRepository
    {
        Exceptions newException = new Exceptions();
        public List<SelectListItem> GetClient(int ClientId)
        {
            List<SelectListItem> lstCRMclient = new List<SelectListItem>();

            List<tblCRMUser> lstCRMUsers = new List<tblCRMUser>();
            using (var context = new CRMContext())
            {
                lstCRMUsers = context.tblCRMUsers.Where(a => a.ClientId == ClientId && a.CurrentStage == 3).ToList();
            }
            foreach (var user in lstCRMUsers)
            {
                lstCRMclient.Add(new SelectListItem
                {
                    Text = Convert.ToString(user.FirstName + " " + user.LastName),
                    Value = Convert.ToString(user.Id)
                });
            }

            return lstCRMclient;
        }

        public List<SelectListItem> GetCRMClientSubStages(int id)
        {
            List<SelectListItem> lstCRMSubstages = new List<SelectListItem>();
            List<tblCRMClientSubStage> objCRMClientSubStage = new List<tblCRMClientSubStage>();
            using (var context = new CRMContext())
            {
                objCRMClientSubStage = context.tblCRMClientSubStages.Where(a => a.ClientId == id && a.IsActive == true).ToList();
            }

            foreach (var subStage in objCRMClientSubStage)
            {
                lstCRMSubstages.Add(new SelectListItem
                {
                    Text = Convert.ToString(subStage.SubStageName),
                    Value = Convert.ToString(subStage.SubStageId)
                });
            }
            return lstCRMSubstages;
        }



        public bool SaveUserData(tblCRMNote objCRMNote)
        {
            bool status = false;
            using (var context = new CRMContext())
            {
                context.Database.Log = Console.Write;
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var address = context.tblCRMUsers.First(a => a.Id == 2);
                        context.tblCRMNotes.Add(objCRMNote);
                        context.SaveChanges();



                        transaction.Commit();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newException.AddException(ex);
                        throw ex;
                    }

                }
            }

            return status;
        }

        public List<FileNotesDetails> GetCRMUserFileNotesById(int id)
        {
            List<FileNotesDetails> objCRMNote = new List<FileNotesDetails>();
            using (var context = new CRMContext())
            {               
                objCRMNote = (from a in context.tblCRMNotes
                              join b in context.tblCRMClientSubStages on a.SubStage equals b.SubStageId
                              where a.ClientId == id
                              orderby a.SubStage descending, a.CreatedDate
                              select new FileNotesDetails
                              {
                                  ClientId = a.ClientId,
                                  Notes = a.Notes,
                                  SubStage = b.SubStageId,
                                  SubStageName = b.SubStageName,
                                  CommunicationSource = a.CommunicationSource,
                                  CreatedDate = a.CreatedDate

                              }).ToList();
            }
            return objCRMNote;
        }
        public List<string> GetCRMUserFileNotesSubStagesById(int id)
        {
            List<FileNotesDetails> objCRMNote = new List<FileNotesDetails>();
            List<string> lstItems = new List<string>();
            using (var context = new CRMContext())
            {
                //objCRMNote = context.tblCRMNotes.Where(a => a.ClientId == id).ToList();
                objCRMNote = (from a in context.tblCRMNotes
                              join b in context.tblCRMClientSubStages on a.SubStage equals b.SubStageId
                              where a.ClientId == id
                              orderby a.SubStage descending
                              select new FileNotesDetails
                              {
                                  ClientId = a.ClientId,
                                  Notes = a.Notes,
                                  SubStage = b.SubStageId,
                                  SubStageName = b.SubStageName,
                                  CommunicationSource = a.CommunicationSource,
                                  CreatedDate = a.CreatedDate

                              }).ToList();                
                var objCRMNote1 = objCRMNote.GroupBy(item => item.SubStage)
                 .Select(group => new { Customer = group.ToString(), Items = group.ToList() })
                 .ToList();
                
                foreach(var item in objCRMNote1)
                {
                    lstItems.Add(item.Items[0].SubStageName);
                }
            }
            return lstItems;
        }


    }
}
