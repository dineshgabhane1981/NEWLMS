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
        public bool SaveUserData(tblCRMUser ObjCRMUser, tblCRMUsersOtherInfo ObjCRMUsersOtherInfo, 
            tblCRMUsersBillingAddress ObjCRMUsersBillingAddress, tblCRMUsersPassportDetail ObjCRMUsersPassportDetail, 
            tblCRMUsersVisaDetail ObjCRMUsersVisaDetail, tblCRMUsersMedicalDetail ObjCRMUsersMedicalDetail, 
            tblCRMUsersPoliceCertificateInfo ObjCRMUsersPoliceCertificateInfo, 
            tblCRMUsersINZLoginDetail ObjCRMUsersINZLoginDetail, tblCRMUsersNZQADetail ObjCRMUsersNZQADetail)
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
                        context.tblCRMUsers.Add(ObjCRMUser);
                        context.SaveChanges();

                        ObjCRMUsersOtherInfo.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersOtherInfoes.Add(ObjCRMUsersOtherInfo);
                        context.SaveChanges();

                        ObjCRMUsersBillingAddress.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersBillingAddresses.Add(ObjCRMUsersBillingAddress);
                        context.SaveChanges();

                        ObjCRMUsersPassportDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersPassportDetails.Add(ObjCRMUsersPassportDetail);
                        context.SaveChanges();

                        ObjCRMUsersVisaDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersVisaDetails.Add(ObjCRMUsersVisaDetail);
                        context.SaveChanges();

                        ObjCRMUsersMedicalDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersMedicalDetails.Add(ObjCRMUsersMedicalDetail);
                        context.SaveChanges();

                        ObjCRMUsersPoliceCertificateInfo.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersPoliceCertificateInfoes.Add(ObjCRMUsersPoliceCertificateInfo);
                        context.SaveChanges();

                        ObjCRMUsersINZLoginDetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersINZLoginDetails.Add(ObjCRMUsersINZLoginDetail);
                        context.SaveChanges();

                        ObjCRMUsersNZQADetail.CRMUserId = ObjCRMUser.Id;
                        context.tblCRMUsersNZQADetails.Add(ObjCRMUsersNZQADetail);
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
    
    
    
        public List<tblCRMUser> GetCRMUsersAll(int ClientId)
        {
            List<tblCRMUser> lstCRMUsers = new List<tblCRMUser>();
            using (var context = new CRMContext())
            {
                lstCRMUsers = context.tblCRMUsers.Where(a => a.ClientId == ClientId).ToList();
            }
            return lstCRMUsers;
        }
    }
}
