using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using LMSBL.Common;
using LMSBL.DBModels;

namespace LMSBL.Repository
{
    public class HomeRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();

        public List<HomeModel> GetAllLearnerActivities(int TenantId, int UserId, string status)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                db.AddParameter("@UserId", SqlDbType.Int, UserId);
                db.AddParameter("@status", SqlDbType.Text, status);
                DataSet ds = db.FillData("sp_HomeActivitiesList");
                List<HomeModel> activityDetails = ds.Tables[0].AsEnumerable().Select(dr => new HomeModel
                {
                    ActivityId = Convert.ToInt32(dr["ActivityId"]),
                    ActivityName = Convert.ToString(dr["ActivityName"]),
                    ActivityType = Convert.ToString(dr["ActivityType"]),
                    ActivitySubType= Convert.ToString(dr["ActivitySubType"]),
                    ActivityURL = Convert.ToString(dr["ActivityURL"]),
                    ActivityDescription = Convert.ToString(dr["ActivityDescription"]),
                    ActivityDuration = Convert.ToString(dr["ActivityDuration"]),
                    ActivityStartDate = Convert.ToString(dr["ActivityStartDate"]),
                    ActivityCompletionDate = Convert.ToString(dr["ActivityCompletionDate"]),
                    ActivityStatus = Convert.ToString(dr["ActivityStatus"]),
                    ActivityRating = Convert.ToString(dr["ActivityRating"])

                }).ToList();

                foreach(var item in activityDetails)
                {
                    if(item.ActivityType== "Curriculum")
                    {
                        db.parameters.Clear();
                        db.AddParameter("@CurriculumId", SqlDbType.Int, item.ActivityId);
                        db.AddParameter("@UserId", SqlDbType.Int, UserId);                        
                        DataSet ds1 = db.FillData("sp_CurriculumGetActivityStatus");
                        if(ds1!=null)
                        {
                            if(ds1.Tables.Count>0)
                            {
                                int totalCount = 0;
                                int inProgress = 0;
                                int completed = 0;
                                foreach(DataRow row in ds1.Tables[0].Rows)
                                {
                                    if(Convert.ToString(row["ActivityType"])!= "Forum")
                                    {
                                        totalCount++;
                                        if(Convert.ToString(row["ActivityStatus"])== "InProgress")
                                        {
                                            inProgress++;
                                        }
                                        if (Convert.ToString(row["ActivityStatus"]) == "Completed")
                                        {
                                            completed++;
                                        }

                                    }
                                }
                                if(inProgress>0)
                                {
                                    item.ActivityStatus = "InProgress";
                                }
                                else if(completed>0 && completed==totalCount)
                                {
                                    item.ActivityStatus = "Completed";
                                }
                                else if (completed > 0)
                                {
                                    item.ActivityStatus = "InProgress";
                                }
                                else
                                {
                                    item.ActivityStatus = "Not Started";
                                }
                            }
                        }
                    }
                }


                return activityDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }
        public List<AdminHomeListModel> GetAllAdminActivities(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);

                DataSet ds = db.FillData("sp_AdminDashboardList");
                List<AdminHomeListModel> activityDetails = ds.Tables[0].AsEnumerable().Select(dr => new AdminHomeListModel
                {
                    ActivityId = Convert.ToInt32(dr["ActivityId"]),
                    ActivityName = Convert.ToString(dr["ActivityName"]),
                    ActivityType = Convert.ToString(dr["ActivityType"]),
                    ActivityDescription = Convert.ToString(dr["ActivityDescription"]),
                    ActivityDuration = Convert.ToString(dr["ActivityDuration"]),
                    ActivityCreationDate = Convert.ToString(dr["ActivityCreationDate"]),
                    ActivityAssignedUsers = Convert.ToInt32(dr["ActivityAssignedUsers"])


                }).ToList();
                return activityDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public AdminHomeCountModel GetAllAdminHomeCounts(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);

                DataSet ds = db.FillData("sp_AdminDashboardCounts");
                AdminHomeCountModel countDetails = new AdminHomeCountModel();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            countDetails.TotalUserCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalUserCount"]);
                            countDetails.ActiveUserCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ActiveUserCount"]);
                            countDetails.TotalQuizCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalQuizCount"]);
                            countDetails.TotalCourseCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCourseCount"]);
                            countDetails.TotalHighScoreQuizUserCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHighScoreQuizUserCount"]);
                            countDetails.TotalHighScoreCourseUserCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalHighScoreCourseUserCount"]);
                        }
                    }
                }

                return countDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }


    }
}
