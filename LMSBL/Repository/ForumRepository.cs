using System;
using System.Collections.Generic;
using System.Linq;
using LMSBL.DBModels;
using System.Data;
using System.Data.SqlClient;
using LMSBL.Common;
using System.IO;
using System.Configuration;

namespace LMSBL.Repository
{

    public class ForumRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();


        public int AddForum(tblForum obj)
        {

            int status = 0;
            int forumId = 0;
            try
            {

                db.AddParameter("@Title", SqlDbType.Text, obj.Title);
                db.AddParameter("@Description", SqlDbType.Text, obj.Description);
                db.AddParameter("@ForumType", SqlDbType.Int, obj.ForumType);
                db.AddParameter("@TenantId", SqlDbType.Int, obj.TenantId);
                db.AddParameter("@CreatedBy", SqlDbType.Int, obj.CreatedBy);
                db.AddParameter("@IsBrodcast", SqlDbType.Bit, obj.IsBrodcast);

                db.AddParameter("@ForumId", SqlDbType.Int, ParameterDirection.Output);
                status = db.ExecuteQuery("sp_ForumAdd");
                if (Convert.ToInt32(db.parameters[6].Value) > 0)
                {
                    forumId = Convert.ToInt32(db.parameters[6].Value);
                }

                if (forumId > 0)
                {
                    status = forumId;
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

        public int EditForum(tblForum obj)
        {

            int status = 0;
            int forumId = 0;
            try
            {
                db.AddParameter("@ForumId", SqlDbType.Int, obj.ForumId);
                db.AddParameter("@Title", SqlDbType.Text, obj.Title);
                db.AddParameter("@Description", SqlDbType.Text, obj.Description);
                db.AddParameter("@ForumType", SqlDbType.Int, obj.ForumType);
                db.AddParameter("@IsBrodcast", SqlDbType.Bit, obj.IsBrodcast);
                db.AddParameter("@ForumIdOutput", SqlDbType.Int, ParameterDirection.Output);
                status = db.ExecuteQuery("sp_ForumUpdate");
                if (Convert.ToInt32(db.parameters[5].Value) > 0)
                {
                    forumId = Convert.ToInt32(db.parameters[5].Value);
                }

                if (forumId > 0)
                {
                    status = forumId;
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

        public List<tblForum> GetAllForums(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_ForumGet");
                List<tblForum> forumDetails = ds.Tables[0].AsEnumerable().Select(dr => new tblForum
                {
                    ForumId = Convert.ToInt32(dr["ForumId"]),
                    Title = Convert.ToString(dr["Title"]),
                    Description = Convert.ToString(dr["Description"]),
                    ForumType = Convert.ToInt32(dr["ForumType"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    IsBrodcast = Convert.ToBoolean(dr["IsBrodcast"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    TenantId = Convert.ToInt32(dr["TenantId"])

                }).ToList();
                return forumDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblForum> GetForumById(int forumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@forumId", SqlDbType.Int, forumId);
                DataSet ds = db.FillData("sp_ForumGetById");
                List<tblForum> forumDetails = ds.Tables[0].AsEnumerable().Select(dr => new tblForum
                {
                    ForumId = Convert.ToInt32(dr["ForumId"]),
                    Title = Convert.ToString(dr["Title"]),
                    Description = Convert.ToString(dr["Description"]),
                    ForumType = Convert.ToInt32(dr["ForumType"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    IsBrodcast = Convert.ToBoolean(dr["IsBrodcast"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    TenantId = Convert.ToInt32(dr["TenantId"])

                }).ToList();
                return forumDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

    }
}