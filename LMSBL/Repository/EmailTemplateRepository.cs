using System;
using System.Collections.Generic;
using System.Linq;
using LMSBL.DBModels;
using System.Data;
using System.Data.SqlClient;
using LMSBL.Common;
using System.IO;
using System.Configuration;
using System.Web.UI;

namespace LMSBL.Repository
{
    public class EmailTemplateRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        public List<tblTemplateAssign> GetEmailTemplateAssigns(int TenantId)
        {
            try 
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_GetEmailTemplate");
                List<tblTemplateAssign> Emailtemplate = ds.Tables[0].AsEnumerable().Select(dr => new tblTemplateAssign
                {
                    TenantId = Convert.ToInt32(dr["TenantId"]),
                    TemplateId = Convert.ToInt32(dr["TemplateId"]),
                    EmailSubject= Convert.ToString(dr["EmailSubject"]),
                    EmailBody = Convert.ToString(dr["EmailBody"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"])
                

                  }).ToList();
                    return Emailtemplate;

            }
            catch(Exception ex)
            {

                newException.AddException(ex);
                throw ex;
            }
        
        
        }

        public List<tblTemplateAssign> GetEmailTemplateAssignsById(int TemplateId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@templateId", SqlDbType.Int, TemplateId);
                DataSet ds = db.FillData("sp_GetEmailTemplateById");
                List<tblTemplateAssign> Emailtemplate = ds.Tables[0].AsEnumerable().Select(dr => new tblTemplateAssign
                {
                    TenantId = Convert.ToInt32(dr["TenantId"]),
                    TemplateId = Convert.ToInt32(dr["TemplateId"]),
                    EmailSubject = Convert.ToString(dr["EmailSubject"]),
                    EmailBody = Convert.ToString(dr["EmailBody"]),
                    DateCreated = Convert.ToDateTime(dr["DateCreated"])


                }).ToList();
                return Emailtemplate;

            }
            catch (Exception ex)
            {

                newException.AddException(ex);
                throw ex;
            }


        }

        public int UpdateEmailTemplate(tblTemplateAssign obj)
        {

            int status = 0;
            int templateId = 0;
            try
            {
                db.AddParameter("@tenantId", SqlDbType.Int, obj.TenantId);
                db.AddParameter("@templateId", SqlDbType.Int, obj.TemplateId);
                db.AddParameter("@emailSubject", SqlDbType.Text, obj.EmailSubject);
                db.AddParameter("@emailbody", SqlDbType.Text, obj.EmailBody);
                //db.AddParameter("@datecreated", SqlDbType.DateTime, obj.DateCreated);
                db.AddParameter("@templateIdoutput", SqlDbType.Int, ParameterDirection.Output);
                 status = db.ExecuteQuery("sp_UpdateEmailTemplate");
                templateId = Convert.ToInt32(db.parameters[4].Value);
                

                if (templateId > 0)
                {
                    status = templateId;
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                //throw ex;
                status = -2;
            }
            return status;
        }


    }
}
