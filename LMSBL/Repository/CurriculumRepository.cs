﻿using LMSBL.Common;
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
    }
}