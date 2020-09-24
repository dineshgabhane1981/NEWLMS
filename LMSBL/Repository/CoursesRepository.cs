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
    public class CoursesRepository
    {
        DataRepository db = new DataRepository();
        Exceptions newException = new Exceptions();
        public string UploadFile(HttpPostedFileBase zip, string CourseName, string courseType)
        {
            try
            {
                var path = ConfigurationManager.AppSettings["DestinationPath"].ToString();
                byte[] data;
                string FileName = "";
                if (zip != null)
                {
                    using (Stream inputStream = zip.InputStream)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            inputStream.CopyTo(ms);
                            data = ms.ToArray();
                        }
                        FileName = CourseName + "." + zip.FileName.Split('.')[1];
                        if (!File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                           
                        System.IO.File.WriteAllBytes(Path.Combine(path, FileName), data);
                        if (courseType == "Scorm1.2")
                        {
                            ZipArchive archive = ZipFile.OpenRead(Path.Combine(path, FileName));
                            if (Directory.Exists(path + "\\" + CourseName))
                            {
                                Directory.Delete(path + "\\" + CourseName, true);
                            }
                            archive.ExtractToDirectory(path + "\\" + CourseName);
                        }

                    }
                    return Path.Combine(path, FileName);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }


        }

        public List<tblCourse> GetCourseById(int CourseId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Int, CourseId);
                DataSet ds = db.FillData("sp_CourseGetById");
                List<tblCourse> coursesDetails = ds.Tables[0].AsEnumerable().Select(dr => new tblCourse
                {
                    ContentModuleId = Convert.ToInt32(dr["ContentModuleId"]),
                    ContentModuleName = Convert.ToString(dr["ContentModuleName"]),
                    ContentModuleDescription = Convert.ToString(dr["ContentModuleDescription"]),
                    ContentModuleURL = Convert.ToString(dr["ContentModuleURL"]),
                    ContentModuleType = Convert.ToString(dr["ContentModuleType"]),
                    IsActive = Convert.ToBoolean(dr["isActive"]),
                    MasteryScore = dr["MasteryScore"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MasteryScore"]),
                    createdBy = Convert.ToInt32(dr["createdBy"]),
                    createdOn = Convert.ToDateTime(dr["createdOn"]),
                    tenantId = Convert.ToInt32(dr["tenantId"]),
                    Duration = dr["Duration"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Duration"])
                    //Duration = Convert.ToInt32(dr["Duration"])


                }).ToList();
                return coursesDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public List<tblCourse> GetAllCourses(int TenantId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@tenantId", SqlDbType.Int, TenantId);
                DataSet ds = db.FillData("sp_CoursesGet");
                List<tblCourse> coursesDetails = ds.Tables[0].AsEnumerable().Select(dr => new tblCourse
                {
                    ContentModuleId = Convert.ToInt32(dr["ContentModuleId"]),
                    ContentModuleName = Convert.ToString(dr["ContentModuleName"]),
                    ContentModuleDescription = Convert.ToString(dr["ContentModuleDescription"]),
                    ContentModuleURL = Convert.ToString(dr["ContentModuleURL"]),
                    ContentModuleType = Convert.ToString(dr["ContentModuleType"]),
                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                    MasteryScore = dr["MasteryScore"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MasteryScore"]),
                    createdBy = Convert.ToInt32(dr["createdBy"]),
                    createdOn = Convert.ToDateTime(dr["createdOn"]),
                    tenantId = Convert.ToInt32(dr["tenantId"]),
                    //Duration = dr["Duration"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Duration"])
                    Duration = Convert.ToInt32(Convert.ToString(dr["Duration"]))

                }).ToList();
                return coursesDetails;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }

        public int AddCourse(tblCourse obj)
        {
            
            int status = 0;
            int ContentModuleId = 0;
            try
            {
                FileInfo f1 = new FileInfo(obj.ZipFile.FileName);
                db.parameters.Clear();
                var path = ConfigurationManager.AppSettings["DestinationPath"].ToString();    
                if(obj.ContentModuleType== "Other")
                {
                    path = f1.Extension;
                }
                db.AddParameter("@ContentModuleName", SqlDbType.Text, obj.ContentModuleName);
                db.AddParameter("@ContentModuleDescription", SqlDbType.NText, obj.ContentModuleDescription);
                db.AddParameter("@ContentModuleType", SqlDbType.Text, obj.ContentModuleType);
                db.AddParameter("@ContentModuleURL", SqlDbType.Text, path);
                db.AddParameter("@MasteryScore", SqlDbType.Int, obj.MasteryScore);
                db.AddParameter("@Duration", SqlDbType.Int, obj.Duration);
                db.AddParameter("@createdBy", SqlDbType.Int, obj.createdBy);
                db.AddParameter("@tenantId", SqlDbType.Int, obj.tenantId);
                db.AddParameter("@ContentModuleId", SqlDbType.Int, ParameterDirection.Output);
                status = db.ExecuteQuery("sp_CourseAdd");
                if (Convert.ToInt32(db.parameters[8].Value) > 0)
                {
                    ContentModuleId = Convert.ToInt32(db.parameters[8].Value);
                    string returnPath = UploadFile(obj.ZipFile, Convert.ToString(ContentModuleId), obj.ContentModuleType);
                }

                if (ContentModuleId > 0)
                {
                    status = ContentModuleId;
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

        public int EditCourse(tblCourse obj)
        {
            int status = 0;
            try
            {
                string returnPath = string.Empty;
                if (!string.IsNullOrEmpty(obj.ContentModuleURL) && obj.ZipFile == null)
                {
                    returnPath = obj.ContentModuleURL;
                }
                else
                {
                    returnPath = UploadFile(obj.ZipFile, Convert.ToString(obj.ContentModuleId), obj.ContentModuleType);
                }
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Int, obj.ContentModuleId);
                db.AddParameter("@ContentModuleName", SqlDbType.Text, obj.ContentModuleName);
                db.AddParameter("@ContentModuleDescription", SqlDbType.NText, obj.ContentModuleDescription);
                db.AddParameter("@ContentModuleType", SqlDbType.Text, obj.ContentModuleType);
                db.AddParameter("@ContentModuleURL", SqlDbType.Text, returnPath);
                db.AddParameter("@MasteryScore", SqlDbType.Int, obj.MasteryScore);
                db.AddParameter("@Duration", SqlDbType.Int, obj.Duration);
                db.AddParameter("@createdBy", SqlDbType.Int, obj.createdBy);
                db.AddParameter("@tenantId", SqlDbType.Int, obj.tenantId);
                status = db.ExecuteQuery("sp_CourseUpdate");
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                //throw ex;
                status = -2;
            }
            return status;
        }

        public int DeleteCourse(tblCourse obj)
        {
            try
            {
                db = new DataRepository();
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Int, obj.ContentModuleId);
                db.AddParameter("@IsActive", SqlDbType.Bit, obj.IsActive);
                return db.ExecuteQuery("sp_CourseActivateDeactivate");
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }
        public List<tblLearnerActivityAssignment> GetCourseAssignedUser(int CourseId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Int, CourseId);
                DataSet ds = db.FillData("sp_GetCourseAssignedUsers");

                List<tblLearnerActivityAssignment> lstCourseUsers = ds.Tables[0].AsEnumerable().Select(dr => new tblLearnerActivityAssignment
                {
                    LearnerId = Convert.ToInt32(dr["LearnerId"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    DueDate = Convert.ToString(dr["DueDate"]),
                }).ToList();

                return lstCourseUsers;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }
        public DataSet GetAssignedCourseUsers(int CourseId)
        {
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Int, CourseId);
                DataSet ds = db.FillData("sp_GetAssignedUsersForCourse");
                return ds;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
        }
        public int DeleteAssignedUserForCourse(int CourseId)
        {
            int status = 0;
            try
            {
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Text, Convert.ToString(CourseId));
                status = db.ExecuteQuery("sp_DeleteAssignedUsersForCourse");
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return status;
        }

        public int AssignCourse(int ContentModuleId, int UserId, DateTime? DueDate)
        {
            int status = 0;
            try
            {
                db.parameters.Clear();
                db.AddParameter("@UserId", SqlDbType.Text, UserId);
                db.AddParameter("@ActivityId", SqlDbType.Text, ContentModuleId);
                db.AddParameter("@DueDate", SqlDbType.DateTime, DueDate);
                status = db.ExecuteQuery("sp_CourseAssign");

            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return status;
        }
        public int AssignCourseTouser(object[] objData, int ContentModuleId)
        {
            int courseId = 0;
            int status = 0;

            if (objData != null)
            {
                db.parameters.Clear();
                db.AddParameter("@ContentModuleId", SqlDbType.Text, Convert.ToString(ContentModuleId));
                status = db.ExecuteQuery("sp_DeleteAssignedUsersForCourse");
                status = 1;
                
                if (status == 1)
                {
                    foreach (Dictionary<string, object> item in objData)
                    {
                        db.parameters.Clear();
                        db.AddParameter("@UserId", SqlDbType.Text, Convert.ToInt32(item["LearnerId"]));
                        db.AddParameter("@ActivityId", SqlDbType.Text, ContentModuleId);
                        db.AddParameter("@DueDate", SqlDbType.DateTime, Convert.ToString(item["DueDate"]));                       
                        
                        var result = db.ExecuteQuery("sp_CourseAssign");
                            courseId = 1;
                        
                    }
                }
            }

            return courseId;
        }

        public bool CheckInsertEmail(tblEmails objemail)
        {
            bool status = false;
           // DataSet ds = new DataSet();
                        db.parameters.Clear();
                        db.AddParameter("@emailTo", SqlDbType.Text, Convert.ToString(objemail.EmailTo));
                        db.AddParameter("@activityId", SqlDbType.Int, Convert.ToInt32(objemail.Activityid));
                        db.AddParameter("@activityType", SqlDbType.Text, Convert.ToString(objemail.Activitytype));
                        db.AddParameter("@dueDate", SqlDbType.DateTime, Convert.ToDateTime(objemail.Duedate));

                    DataSet  ds = db.FillData("sp_InsertEmailBycheck");
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                status = true;
                            }
                        }
                    }
                

                

            return status; 
        }
        public bool CheckCourseAssignedUser(int CourseId, int UserId)
        {
            bool status = false;
            try
            {
                db.parameters.Clear();
                db.AddParameter("@CourseId", SqlDbType.Int, CourseId);
                db.AddParameter("@UserId", SqlDbType.Int, UserId);
                DataSet ds = db.FillData("sp_CheckCourseAssignedUser");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            status = true;
                        }
                    }
                }
                // status =true;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                throw ex;
            }
            return status;
        }
    }
}
