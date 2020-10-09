using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels.CRMNew;
using LMSBL.DBModels;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Script.Serialization;

namespace LMSBL.Repository
{
    public class CRMUsersRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        public List<SelectListItem> GetVisaCountries()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetVisaCountries");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                countriesItem.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["CountryName"]),
                                    Value = Convert.ToString(dr["CountryId"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return countriesItem;
        }
        public List<SelectListItem> GetCountryCodes()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetCountryCode");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                countriesItem.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["CountryName"]),
                                    Value = Convert.ToString(dr["CountryCode"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return countriesItem;
        }
        public List<SelectListItem> WhereDidYouFindUs()
        {
            List<SelectListItem> clientSource = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetClientSource");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                clientSource.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["SourceName"]),
                                    Value = Convert.ToString(dr["SourceId"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return clientSource;

        }
        public List<SelectListItem> GetJobSector()
        {
            List<SelectListItem> clientSource = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetJobSector");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                clientSource.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["JobName"]),
                                    Value = Convert.ToString(dr["JobId"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return clientSource;

        }
        public List<SelectListItem> GetCountries()
        {
            List<SelectListItem> clientSource = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetAllCountries");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                clientSource.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["CountryName"]),
                                    Value = Convert.ToString(dr["CountryName"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return clientSource;

        }
        public List<SelectListItem> GetVisaType()
        {
            List<SelectListItem> visaType = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetVisaType");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                visaType.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["VisaName"]),
                                    Value = Convert.ToString(dr["VisaId"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return visaType;

        }
        public List<SelectListItem> GetVisaStatus()
        {
            List<SelectListItem> visaType = new List<SelectListItem>();
            try
            {
                db.parameters.Clear();
                DataSet ds = db.FillData("sp_GetVisaStatus");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                visaType.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(dr["VisaStatusName"]),
                                    Value = Convert.ToString(dr["VisaStatusId"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return visaType;

        }

        [HttpPost, ValidateInput(false)]
        public bool SaveUserData(tblCRMUser ObjCRMUser,
            tblCRMUsersBillingAddress ObjCRMUsersBillingAddress, tblCRMUsersPassportDetail ObjCRMUsersPassportDetail,
            tblCRMUsersVisaDetail ObjCRMUsersVisaDetail, tblCRMUsersMedicalDetail ObjCRMUsersMedicalDetail,
            tblCRMUsersPoliceCertificateInfo ObjCRMUsersPoliceCertificateInfo,
            tblCRMUsersINZLoginDetail ObjCRMUsersINZLoginDetail, tblCRMUsersNZQADetail ObjCRMUsersNZQADetail,
            tblCRMNote ObjCRMNote)
        {
            bool status = false;
            using (var context = new CRMContext())
            {

                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.tblCRMUsers.AddOrUpdate(ObjCRMUser);
                        context.SaveChanges();

                        ObjCRMUsersBillingAddress.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersBillingAddresses.AddOrUpdate(ObjCRMUsersBillingAddress);
                        context.SaveChanges();

                        ObjCRMUsersPassportDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersPassportDetails.AddOrUpdate(ObjCRMUsersPassportDetail);
                        context.SaveChanges();

                        ObjCRMUsersVisaDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersVisaDetails.AddOrUpdate(ObjCRMUsersVisaDetail);
                        context.SaveChanges();

                        ObjCRMUsersMedicalDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersMedicalDetails.AddOrUpdate(ObjCRMUsersMedicalDetail);
                        context.SaveChanges();

                        ObjCRMUsersPoliceCertificateInfo.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersPoliceCertificateInfoes.AddOrUpdate(ObjCRMUsersPoliceCertificateInfo);
                        context.SaveChanges();

                        ObjCRMUsersINZLoginDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersINZLoginDetails.AddOrUpdate(ObjCRMUsersINZLoginDetail);
                        context.SaveChanges();

                        ObjCRMUsersNZQADetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersNZQADetails.AddOrUpdate(ObjCRMUsersNZQADetail);
                        context.SaveChanges();

                        if (!string.IsNullOrEmpty(ObjCRMNote.Notes))
                        {
                            ObjCRMNote.ClientId = ObjCRMUser.Id;
                            context.tblCRMNotes.Add(ObjCRMNote);
                            context.SaveChanges();
                        }

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
        public List<EnquiryListing> GetCRMUsersAll(int ClientId, int stage)
        {
            List<tblCRMUser> lstCRMUsers = new List<tblCRMUser>();            
            using (var context = new CRMContext())
            {               
                var lstResult = (from a in context.tblCRMUsers
                                 join b in context.tblCRMUsersVisaDetails on a.Id equals b.CRMUserId
                                 join c in context.tblCRMVisaTypes on b.IntrestedVisa equals c.VisaId into temp
                                 from d in temp.DefaultIfEmpty()
                                 where a.ClientId == ClientId && a.CurrentStage == stage
                                 select new EnquiryListing
                                 {
                                     Id = a.Id,
                                     Name = a.FirstName + " " + a.LastName,
                                     Email = a.Email,
                                     Contact = a.MobileNoCountry + " " + a.MobileNo,
                                     CreatedDate = a.CreatedOn,
                                     IntrestedVisa = d.VisaName

                                 }).ToList();
               
                return lstResult;
            }           
            
        }
        public tblCRMUser GetCRMUserById(int id)
        {
            tblCRMUser objCRMUsers = new tblCRMUser();
            using (var context = new CRMContext())
            {
                objCRMUsers = context.tblCRMUsers.First(a => a.Id == id);
            }
            return objCRMUsers;
        }
        //public tblCRMUsersOtherInfo GetCRMUserOtherInfoById(int id)
        //{
        //    tblCRMUsersOtherInfo objCRMUsersOtherInfo = new tblCRMUsersOtherInfo();
        //    using (var context = new CRMContext())
        //    {
        //        objCRMUsersOtherInfo = context.tblCRMUsersOtherInfoes.First(a => a.CRMUserId == id);
        //    }
        //    return objCRMUsersOtherInfo;
        //}
        public tblCRMUsersBillingAddress GetCRMUserBillingAddressById(int id)
        {
            tblCRMUsersBillingAddress objCRMUsersBillingAddress = new tblCRMUsersBillingAddress();
            using (var context = new CRMContext())
            {
                objCRMUsersBillingAddress = context.tblCRMUsersBillingAddresses.First(a => a.CRMUserId == id);
            }
            return objCRMUsersBillingAddress;
        }
        public tblCRMUsersPassportDetail GetCRMUserPassportDetailById(int id)
        {
            tblCRMUsersPassportDetail objCRMUsersPassportDetail = new tblCRMUsersPassportDetail();
            using (var context = new CRMContext())
            {
                objCRMUsersPassportDetail = context.tblCRMUsersPassportDetails.First(a => a.CRMUserId == id);
            }
            return objCRMUsersPassportDetail;
        }
        public tblCRMUsersVisaDetail GetCRMUserVisaDetailById(int id)
        {
            tblCRMUsersVisaDetail objCRMUsersVisaDetail = new tblCRMUsersVisaDetail();
            using (var context = new CRMContext())
            {
                objCRMUsersVisaDetail = context.tblCRMUsersVisaDetails.First(a => a.CRMUserId == id);
            }
            return objCRMUsersVisaDetail;
        }
        public tblCRMUsersMedicalDetail GetCRMUserMedicalDetailById(int id)
        {
            tblCRMUsersMedicalDetail objCRMUsersMedicalDetail = new tblCRMUsersMedicalDetail();
            using (var context = new CRMContext())
            {
                objCRMUsersMedicalDetail = context.tblCRMUsersMedicalDetails.First(a => a.CRMUserId == id);
            }
            return objCRMUsersMedicalDetail;
        }
        public tblCRMUsersPoliceCertificateInfo GetCRMUserPoliceCertificateInfoById(int id)
        {
            tblCRMUsersPoliceCertificateInfo objCRMUsersPoliceCertificateInfo = new tblCRMUsersPoliceCertificateInfo();
            using (var context = new CRMContext())
            {
                objCRMUsersPoliceCertificateInfo = context.tblCRMUsersPoliceCertificateInfoes.First(a => a.CRMUserId == id);
            }
            return objCRMUsersPoliceCertificateInfo;
        }
        public tblCRMUsersINZLoginDetail GetCRMUserINZLoginDetailById(int id)
        {
            tblCRMUsersINZLoginDetail objCRMUsersINZLoginDetail = new tblCRMUsersINZLoginDetail();
            using (var context = new CRMContext())
            {
                objCRMUsersINZLoginDetail = context.tblCRMUsersINZLoginDetails.First(a => a.CRMUserId == id);
            }
            return objCRMUsersINZLoginDetail;
        }
        public tblCRMUsersNZQADetail GetCRMUserNZQADetailById(int id)
        {
            tblCRMUsersNZQADetail objCRMUsersNZQADetail = new tblCRMUsersNZQADetail();
            using (var context = new CRMContext())
            {
                objCRMUsersNZQADetail = context.tblCRMUsersNZQADetails.First(a => a.CRMUserId == id);
            }
            return objCRMUsersNZQADetail;
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

        public List<tblCRMClientStage> GetCRMClientStages(int id)
        {
            List<tblCRMClientStage> objCRMClientStage = new List<tblCRMClientStage>();
            using (var context = new CRMContext())
            {
                objCRMClientStage = context.tblCRMClientStages.Where(a => a.ClientId == id).ToList();
            }
            return objCRMClientStage;
        }

        public List<tblCRMClientSubStage> GetCRMClientSubStages(int id)
        {
            List<tblCRMClientSubStage> objCRMClientSubStage = new List<tblCRMClientSubStage>();
            using (var context = new CRMContext())
            {
                objCRMClientSubStage = context.tblCRMClientSubStages.Where(a => a.ClientId == id).ToList();
            }
            return objCRMClientSubStage;
        }
        public List<tblCRMUser> GetCRMUsersBySubStageId(int clientId, int subStageId)
        {
            List<tblCRMUser> objCRMUsersOfSubStage = new List<tblCRMUser>();
            using (var context = new CRMContext())
            {
                objCRMUsersOfSubStage = context.tblCRMUsers.Where(a => a.ClientId == clientId && a.CurrentSubStage == subStageId && a.CurrentStage == 3).ToList();
            }
            return objCRMUsersOfSubStage;
        }
        public List<tblCRMUser> GetCRMClientsAll(int ClientId, int stage)
        {
            List<tblCRMUser> lstCRMUsers = new List<tblCRMUser>();
            using (var context = new CRMContext())
            {
                lstCRMUsers = context.tblCRMUsers.Where(a => a.ClientId == ClientId && a.CurrentStage == stage).ToList();
            }
            return lstCRMUsers;
        }

        public List<ClientTicket> GetCRMTicketsAll(int ClientId, int stage)
        {
            List<ClientTicket> lstCRMUsers = new List<ClientTicket>();
            using (var context = new CRMContext())
            {
                //lstCRMUsers = context.tblCRMUsers.Where(a => a.ClientId == ClientId && a.CurrentStage == stage).ToList();
                var lstResult = (from a in context.tblCRMUsers
                                 join b in context.tblCRMUsersVisaDetails on a.Id equals b.CRMUserId
                                 join c in context.tblCRMVisaTypes on b.IntrestedVisa equals c.VisaId into temp
                                 from d in temp.DefaultIfEmpty()
                                 where a.ClientId == ClientId && a.CurrentStage == stage
                                 select new ClientTicket
                                 {
                                     UserId = a.Id,
                                     UserName = a.FirstName + " " + a.LastName,
                                     CurrentSubStage = a.CurrentSubStage,
                                     ContactNo = a.MobileNoCountry + " " + a.MobileNo,                                     
                                     VisaIntrested = d.VisaName

                                 }).ToList();

                return lstResult;
            }
            //return lstCRMUsers;
        }
        public bool UpdateStage(int id, int stage)
        {
            bool result = false;
            using (var context = new CRMContext())
            {
                var objCRMUser = context.tblCRMUsers.First(a => a.Id == id);
                objCRMUser.CurrentStage = stage;
                if(stage==3)
                {
                    objCRMUser.CurrentSubStage = 1;
                }
                context.tblCRMUsers.AddOrUpdate(objCRMUser);
                context.SaveChanges();
                result = true;
            }
            return result;

        }
        public bool UpdateSubStage(int userId, int subStage)
        {
            bool result = false;
            using (var context = new CRMContext())
            {
                var objCRMUser = context.tblCRMUsers.First(a => a.Id == userId);
                objCRMUser.CurrentSubStage = subStage;
                context.tblCRMUsers.AddOrUpdate(objCRMUser);
                context.SaveChanges();
                result = true;
            }
            return result;

        }
    }
}
