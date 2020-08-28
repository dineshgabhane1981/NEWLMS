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
                string result = string.Empty;
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

        public List<tblForum> GetForumByForumType(int tenantId)
        {
            try
            {
                string result = string.Empty;
                db.parameters.Clear();
                //db.AddParameter("@forumType", SqlDbType.Int, forumType);
                db.AddParameter("@tenantId",SqlDbType.Int, tenantId);
                //db.AddParameter("@isBrodcast",SqlDbType.Int,isBrodcast);
                DataSet ds = db.FillData("sp_GetForumByForumType");
                List<tblForum> forumsDetailsByType = ds.Tables[0].AsEnumerable().Select(dr => new tblForum
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
                return forumsDetailsByType;
                
            }
            catch(Exception ex)
            {
                newException.AddException(ex);
                throw ex;

            }
        }

        public List<tblForum> GetAllForumsForLearner(int userId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@Userid", SqlDbType.Int, userId);
                DataSet ds = db.FillData("sp_ViewAllAssignedForum");
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

        public List<tblForumReply> GetForumReplyForLearner(int forumId, int userId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ForumId", SqlDbType.Int, forumId);
                db.AddParameter("@Userid", SqlDbType.Int, userId);
                DataSet ds = db.FillData("sp_ForumGetAllReply");
                List<tblForumReply> replyList = ds.Tables[0].AsEnumerable().Select(dr => new tblForumReply
                {
                    ForumId = Convert.ToInt32(dr["ForumId"]),
                    UserId = Convert.ToInt32(dr["UserId"]),
                    ForumReply = Convert.ToString(dr["ForumReply"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    ProfileImage = Convert.ToString(dr["profileImage"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"])
                    

                }).ToList();
                return replyList;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblForumReply> GetForumReplyReportLearner(int forumId, int userId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ForumId", SqlDbType.Int, forumId);
                db.AddParameter("@Userid", SqlDbType.Int, userId);
                DataSet ds = db.FillData("sp_ForumGetLearnerReply");
                List<tblForumReply> replyList = ds.Tables[0].AsEnumerable().Select(dr => new tblForumReply
                {
                    ForumId = Convert.ToInt32(dr["ForumId"]),
                    UserId = Convert.ToInt32(dr["UserId"]),
                    ForumReply = Convert.ToString(dr["ForumReply"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    ProfileImage = Convert.ToString(dr["profileImage"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"])


                }).ToList();
                return replyList;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public int AddForumReply(tblForumReply obj)
        {
            int status = 0;             
            try
            {

                db.AddParameter("@ForumId", SqlDbType.Int, obj.ForumId);
                db.AddParameter("@UserId", SqlDbType.Int, obj.UserId);
                db.AddParameter("@ForumReply", SqlDbType.NText, obj.ForumReply);
                
                db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                status = db.ExecuteQuery("sp_ForumReplyAdd");
                if (Convert.ToInt32(db.parameters[3].Value) > 0)
                {
                    status = Convert.ToInt32(db.parameters[3].Value);
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

        public DataSet GetAssignedForumUsers(int forumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@forumId", SqlDbType.Int, forumId);
                DataSet ds = db.FillData("sp_GetForumAssignedUsers");
                return ds;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblForumAssign> GetForumAssingedDetailedUserslist(int forumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@forumId", SqlDbType.Int, forumId);
                DataSet ds = db.FillData("sp_GetForumAssignedUserslist");

                List<tblForumAssign> lstforumUsers = ds.Tables[0].AsEnumerable().Select(dr => new tblForumAssign
                {
                    UserId = Convert.ToInt32(dr["UserId"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    DueDate = Convert.ToDateTime(dr["DueDate"]).ToString("MM/dd/yyyy")
                }).ToList();

                return lstforumUsers;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public int AssignForumToDB(object[] objData, int fId)
        {
            int forumId = 0;
            if (objData != null)
            {
                db.parameters.Clear();
                db.AddParameter("@forumId", SqlDbType.Int, fId);
                // db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                var isDelete = db.ExecuteQuery("sp_DeleteForumAssignedUsers");
                isDelete = 1;

                if (isDelete == 1)
                {
                    foreach (Dictionary<string, object> item in objData)
                    {
                        db.parameters.Clear();
                        db.AddParameter("@forumId", SqlDbType.Int, Convert.ToInt32(fId));
                        db.AddParameter("@UserId", SqlDbType.Int, Convert.ToInt32(item["UserId"]));
                        db.AddParameter("@DueDate", SqlDbType.DateTime, Convert.ToDateTime(item["DueDate"]));
                        // db.AddParameter("@result", SqlDbType.Int, ParameterDirection.Output);
                        var result = db.ExecuteQuery("sp_ForumAssign");

                        forumId = 1;
                    }
                }
            }

            return forumId;
        }


    }
}