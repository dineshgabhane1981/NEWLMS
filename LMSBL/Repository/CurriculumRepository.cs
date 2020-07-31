using LMSBL.Common;
using LMSBL.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LMSBL.Repository
{
    public class CurriculumRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        public List<tblCurriculum> GetCurriculumAll(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_CurriculumGet");
                List<tblCurriculum> lstCurriculum = ds.Tables[0].AsEnumerable().Select(dr => new tblCurriculum
                {
                    CurriculumId = Convert.ToInt32(dr["CurriculumId"]),
                    CurriculumTitle = Convert.ToString(dr["CurriculumTitle"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    TenantId = Convert.ToInt32(dr["TenantId"]),
                }).ToList();
                return lstCurriculum;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblCurriculum> GetCurriculumById(int CurriculumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@curriculumId", SqlDbType.Int, CurriculumId);
                DataSet ds = db.FillData("sp_CurriculumGetById");
                List<tblCurriculum> lstCurriculum = ds.Tables[0].AsEnumerable().Select(dr => new tblCurriculum
                {
                    CurriculumId = Convert.ToInt32(dr["CurriculumId"]),
                    CurriculumTitle = Convert.ToString(dr["CurriculumTitle"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    TenantId = Convert.ToInt32(dr["TenantId"])
                }).ToList();

                List<tblCurriculumActivity> lstCurriculumActivity = ds.Tables[1].AsEnumerable().Select(dr => new tblCurriculumActivity
                {
                    ActivityId = Convert.ToInt32(dr["ActivityId"]),
                    ActivityText = Convert.ToString(dr["ActivityText"]),
                    ActivityType = Convert.ToString(dr["ActivityType"]),
                    ActivityOrder = Convert.ToInt32(dr["ActivityOrder"])
                }).ToList();

                lstCurriculum[0].TblCurriculumActivity = lstCurriculumActivity;

                return lstCurriculum;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<Param> GetCurriculumCourses(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_CurriculumCoursesGet");
                List<Param> activities = ds.Tables[0].AsEnumerable().Select(dr => new Param
                {
                    Name = Convert.ToString(dr["ContentModuleName"]),
                    Value = Convert.ToString(dr["ContentModuleId"])
                }).ToList();
                return activities;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<Param> GetCurriculumSurveys(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_CurriculumQuizsGet");
                List<Param> activities = ds.Tables[0].AsEnumerable().Select(dr => new Param
                {
                    Name = Convert.ToString(dr["QuizName"]),
                    Value = Convert.ToString(dr["QuizId"])
                }).ToList();
                return activities;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<Param> GetCurriculumForums(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_CurriculumForumsGet");
                List<Param> activities = ds.Tables[0].AsEnumerable().Select(dr => new Param
                {
                    Name = Convert.ToString(dr["Title"]),
                    Value = Convert.ToString(dr["ForumId"])
                }).ToList();
                return activities;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public int AddCurriculumToDB(object[] objData, tblCurriculum obj)
        {
            int CurriculumId = 0;

            if (obj.CurriculumId == 0)
            {
                db.parameters.Clear();
                db.AddParameter("@CurriculumTitle", SqlDbType.NText, obj.CurriculumTitle);
                db.AddParameter("@CreatedBy", SqlDbType.Int, obj.CreatedBy);
                db.AddParameter("@tenantId", SqlDbType.Int, obj.TenantId);
                db.AddParameter("@CurriculumId", SqlDbType.Int, ParameterDirection.Output);
                CurriculumId = db.ExecuteQuery("sp_CurriculumAdd");
                if (Convert.ToInt32(db.parameters[3].Value) > 0)
                {
                    CurriculumId = Convert.ToInt32(db.parameters[3].Value);
                }
            }
            else
            {
                db.parameters.Clear();
                db.AddParameter("@CurriculumId", SqlDbType.Int, obj.CurriculumId);
                db.AddParameter("@CurriculumTitle", SqlDbType.NText, obj.CurriculumTitle);
                db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                var status = db.ExecuteQuery("sp_CurriculumEdit");
                if (Convert.ToInt32(db.parameters[2].Value) > 0)
                {
                    db.parameters.Clear();
                    db.AddParameter("@CurriculumId", SqlDbType.Int, obj.CurriculumId);
                    db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                    status = db.ExecuteQuery("sp_CurriculumDeleteActivity");
                    if (Convert.ToInt32(db.parameters[1].Value) > 0)
                    {

                    }
                }
                CurriculumId = obj.CurriculumId;
            }

            if (CurriculumId > 0)
            {
                if (objData != null)
                {
                    foreach (Dictionary<string, object> item in objData)
                    {
                        db.parameters.Clear();
                        db.AddParameter("@ActivityId", SqlDbType.Int, Convert.ToInt32(item["activityId"]));
                        db.AddParameter("@CurriculumId", SqlDbType.Int, CurriculumId);
                        db.AddParameter("@ActivityType", SqlDbType.NText, Convert.ToString(item["activityType"]));
                        db.AddParameter("@ActivityOrder", SqlDbType.Int, Convert.ToInt32(item["index"]));
                        db.AddParameter("@result", SqlDbType.Int, ParameterDirection.Output);
                        var index = db.ExecuteQuery("sp_CurriculumAddActivity");
                    }
                }
            }
            if (obj.CurriculumId > 0)
            {
                var DS = GetAssignedCurriculumUsers(obj.CurriculumId);
                if (DS != null)
                {
                    if (DS.Count > 0)
                    {
                        db.parameters.Clear();
                        db.AddParameter("@CurriculumId", SqlDbType.Int, Convert.ToInt32(obj.CurriculumId));
                        db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                        var isDelete = db.ExecuteQuery("sp_CurriculumDeleteAssignedUsers");
                        if (Convert.ToInt32(db.parameters[1].Value) > 0)
                        {
                            isDelete = 1;
                        }

                        foreach (var item in DS)
                        {
                            db.parameters.Clear();
                            db.AddParameter("@CurriculumId", SqlDbType.Int, Convert.ToInt32(obj.CurriculumId));
                            db.AddParameter("@UserId", SqlDbType.Int, Convert.ToInt32(item.UserId));
                            db.AddParameter("@DueDate", SqlDbType.DateTime, Convert.ToDateTime(item.DueDate));
                            db.AddParameter("@result", SqlDbType.Int, ParameterDirection.Output);
                            var result = db.ExecuteQuery("sp_CurriculumAssign");

                            if (Convert.ToInt32(db.parameters[3].Value) > 0)
                            {
                                CurriculumId = 1;
                            }
                        }

                    }
                }
            }

            return CurriculumId;
        }

        public DataSet GetCurriculumUsers(int curriculumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@curriculumId", SqlDbType.Int, curriculumId);
                DataSet ds = db.FillData("sp_GetCurriculumUsersToAssign");
                return ds;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblCurriculumAssignment> GetAssignedCurriculumUsers(int curriculumId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@curriculumId", SqlDbType.Int, curriculumId);
                DataSet ds = db.FillData("sp_GetCurriculumAssignedUsers");

                List<tblCurriculumAssignment> lstCurriculumUsers = ds.Tables[0].AsEnumerable().Select(dr => new tblCurriculumAssignment
                {
                    UserId = Convert.ToInt32(dr["UserId"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    DueDate = Convert.ToDateTime(dr["DueDate"]).ToString("MM/dd/yyyy")
                }).ToList();

                return lstCurriculumUsers;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public int AssignCurriculumToDB(object[] objData, string cId)
        {
            int CurriculumId = 0;
            if (objData != null)
            {
                db.parameters.Clear();
                db.AddParameter("@CurriculumId", SqlDbType.Int, Convert.ToInt32(cId));
                db.AddParameter("@status", SqlDbType.Int, ParameterDirection.Output);
                var isDelete = db.ExecuteQuery("sp_CurriculumDeleteAssignedUsers");
                if (Convert.ToInt32(db.parameters[1].Value) > 0)
                {
                    isDelete = 1;
                }
                if (isDelete == 1)
                {
                    foreach (Dictionary<string, object> item in objData)
                    {
                        db.parameters.Clear();
                        db.AddParameter("@CurriculumId", SqlDbType.Int, Convert.ToInt32(cId));
                        db.AddParameter("@UserId", SqlDbType.Int, Convert.ToInt32(item["UserId"]));
                        db.AddParameter("@DueDate", SqlDbType.DateTime, Convert.ToDateTime(item["DueDate"]));
                        db.AddParameter("@result", SqlDbType.Int, ParameterDirection.Output);
                        var result = db.ExecuteQuery("sp_CurriculumAssign");

                        if (Convert.ToInt32(db.parameters[3].Value) > 0)
                        {
                            CurriculumId = 1;
                        }
                    }
                }
            }             

            return CurriculumId;
        }

        public DataSet GetCurriculumActivitiesById(int CurriculumId,int UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                db.parameters.Clear();
                db.AddParameter("@CurriculumId", SqlDbType.Int, CurriculumId);
                db.AddParameter("@UserId", SqlDbType.Int, UserId);
                ds = db.FillData("sp_CurriculumGetActivitiesForLearner");
                return ds;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            
        }
    }
}