using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Mvc;
using System.Data;
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
        UserRepository userRepository = new UserRepository();
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


        public ActionResult ForumList()
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstForum = fr.GetAllForumsForLearner(sessionUser.UserId);
            return View(lstForum);
        }

        public ActionResult PostComment(int forumId)
        {
            tblForumReply objForumReply = new tblForumReply();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstReply = fr.GetForumReplyForLearner(forumId, sessionUser.UserId);
            objForumReply.lstReply = lstReply;
            return View(objForumReply);

        }

        public ActionResult ForumReport(int forumId)
        {
            tblForumReply objForumReply = new tblForumReply();
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var lstReply = fr.GetForumReplyReportLearner(forumId, sessionUser.UserId);
            objForumReply.lstReply = lstReply;
            return View(objForumReply);

        }

        public ActionResult AddReply(int forumId,string forumReply)
        {

            TblUser sessionUser = (TblUser)Session["UserSession"];
            tblForumReply objForumReply = new tblForumReply();
            objForumReply.ForumId = forumId;
            objForumReply.UserId = sessionUser.UserId;
            objForumReply.ForumReply = forumReply;

            var status = fr.AddForumReply(objForumReply);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Getuser(int fId)
        {
            List<SelectListItem> userItems = new List<SelectListItem>();
            TblUser sessionUser = (TblUser)Session["UserSession"];

            var Users = userRepository.GetAllUsers(sessionUser.TenantId);

            foreach (var user in Users)
            {
                userItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(user.FirstName + " " + user.LastName),
                    Value = Convert.ToString(user.UserId)
                });
            }

            DataSet ds = fr.GetAssignedForumUsers(fId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (var item in userItems)
                        {
                            DataRow[] foundUsers = ds.Tables[0].Select("UserId = " + item.Value + "");
                            if (foundUsers.Length != 0)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }

            return Json(userItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssignedUsers(int fId)
        {
            List<SelectListItem> userItems = new List<SelectListItem>();
            TblUser sessionUser = (TblUser)Session["UserSession"];

            var Users = userRepository.GetAllUsers(sessionUser.TenantId);
            foreach (var user in Users)
            {
                userItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(user.FirstName + " " + user.LastName),
                    Value = Convert.ToString(user.UserId)
                });
            }

            List<tblForumAssign> lstforumAssignedUsers = new List<tblForumAssign>();
            lstforumAssignedUsers = fr.GetForumAssingedDetailedUserslist(fId);

            return Json(lstforumAssignedUsers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignForumTouser(string jsonData, int fId)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = null;
            if (!string.IsNullOrEmpty(jsonData))
            {
                objData = (object[])json_serializer.DeserializeObject(jsonData);
            }
            var result = fr.AssignForumToDB(objData, fId);

            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
        }

    }
}