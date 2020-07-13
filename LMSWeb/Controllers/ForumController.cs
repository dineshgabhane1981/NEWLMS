﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Mvc;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using LMSWeb.App_Start;
using LMSWeb.ViewModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMSWeb.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        ForumRepository fr = new ForumRepository();
        Exceptions newException = new Exceptions();
        public ActionResult Index()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstForum = fr.GetAllForums(sessionUser.TenantId);
            return View(lstForum);
        }

        public ActionResult AddForum()
        {
            tblForum objForum = new tblForum();
            return View(objForum);
        }
        [HttpPost]
        public ActionResult AddForum(tblForum objForum)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var id = 0;
                    TblUser sessionUser = (TblUser)Session["UserSession"];
                    objForum.CreatedBy = sessionUser.UserId;
                    objForum.TenantId = sessionUser.TenantId;
                    if (objForum.ForumType == 2)
                    {
                        objForum.IsBrodcast = false;
                    }

                    if (objForum.ForumId == 0)
                    {
                        id = fr.AddForum(objForum);
                    }
                    else
                    {
                        id = fr.EditForum(objForum);
                    }
                    if (id > 0)
                    {
                        TempData["Message"] = "1";
                    }
                    else
                    {
                        TempData["Message"] = "2";
                    }

                    //return View("objForum");
                }
                return View("AddForum", objForum);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "2";
                newException.AddException(ex);
                return View("AddForum", objForum);
            }
        }

        public ActionResult EditForum(int id)
        {
            try
            {
                List<tblForum> ForumDetails = new List<tblForum>();
                ForumDetails = fr.GetForumById(id);

                return View("AddForum", ForumDetails[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                TempData["Message"] = "-1";
                return RedirectToAction("Index");
                //return View("Index");
            }

        }
    }
}