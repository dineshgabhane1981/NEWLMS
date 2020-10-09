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
            foreach(var user in lstCRMUsers)
            {
                lstCRMclient.Add(new SelectListItem
                {
                    Text = Convert.ToString(user.FirstName +" " + user.LastName),
                    Value = Convert.ToString(user.Id)
                });
            }
           
            return lstCRMclient;
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

        public List<tblCRMNote> GetCRMUserFileNotesById(int id)
        {
            List<tblCRMNote> objCRMNote = new List<tblCRMNote>();
            using (var context = new CRMContext())
            {
                objCRMNote = context.tblCRMNotes.Where(a => a.ClientId == id).ToList();
            }
            return objCRMNote;
        }
    }
}
