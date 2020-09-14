using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LMSWeb.ViewModel;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace LMSWeb.Controllers
{
    public class QuizController : Controller
    {
        UserRepository userRepository = new UserRepository();
        QuizRepository quizRepository = new QuizRepository();
        Exceptions newException = new Exceptions();
        // GET: Quiz
        public ActionResult Index()
        {
            try
            {

                TblUser sessionUser = (TblUser)Session["UserSession"];

                List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
                lstAllQuiz = quizRepository.GetAllQuiz(sessionUser.TenantId);

                return View("QuizList", lstAllQuiz);
            }
            catch (Exception ex)
            {
                //newException.AddDummyException("222222");
                newException.AddException(ex);
                return View("QuizList");
            }
        }

        public ActionResult AddQuiz()
        {
            try
            {
                TblQuiz objQuiz = new TblQuiz();

                return View("AddNewQuiz", objQuiz);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                return View("AddNewQuiz");
            }
        }
        public ActionResult Getuser(int qId)
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

            DataSet ds = quizRepository.GetAssignedQuizUsers(qId);
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
        public ActionResult GetAssignedUsers(int qId)
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

            List<tblQuizAssignment> lstquizAssignedUsers = new List<tblQuizAssignment>();
            lstquizAssignedUsers = quizRepository.GetQuizAssingedUsers(qId);

            return Json(lstquizAssignedUsers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignQuiztouser(string jsonData, int qId)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = null;
            if (!string.IsNullOrEmpty(jsonData))
            {
                objData = (object[])json_serializer.DeserializeObject(jsonData);
            }
            var result = quizRepository.AssignQuizToDB(objData, qId);

            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignQuiz(int id)
        {
            List<SelectListItem> userItems = new List<SelectListItem>();
            List<TblQuiz> objQuiz = new List<TblQuiz>();
            QuizAssignViewModel quizAssignVieewModel = new QuizAssignViewModel();
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
            DataSet ds = quizRepository.GetAssignedQuizUsers(id);
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
            quizAssignVieewModel.usetList = userItems;
            objQuiz = quizRepository.GetQuizByID(id);
            quizAssignVieewModel.quiz = objQuiz[0];
            return View(quizAssignVieewModel);
        }

        [HttpPost]
        public ActionResult AddQuiz(TblQuiz objQuiz, string submit)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objTblQue = (object[])json_serializer.DeserializeObject(objQuiz.hdnData);
            objQuiz.questionObject = objTblQue;
            try
            {
                if (ModelState.IsValid)
                {
                    TblUser sessionUser = (TblUser)Session["UserSession"];

                    if (sessionUser != null)
                    {
                        objQuiz.TenantId = sessionUser.TenantId;
                        int rows = 0;
                        if (objQuiz.QuizId == 0)
                        {
                            rows = quizRepository.CreateQuiz(objQuiz);
                        }
                        else
                        {
                            rows = quizRepository.UpdateQuiz(objQuiz);

                        }
                        if (rows > 0)
                        {

                            TempData["QuizMessage"] = "Quiz Saved Successfully";
                            if (submit == "Exit")
                            {
                                return View("AuthorContent", objQuiz);
                            }
                            if (submit == "Save")
                            {                                
                                return CreateScormCourse(objQuiz, rows);
                            }
                            //return RedirectToAction("Index");
                            //if (submit == "Save")
                            //{
                            //    return RedirectToAction("EditQuiz", new { id = rows });
                            //}
                        }
                        else if (rows == 0)
                        {
                            TempData["ErrorMessage"] = "There is some problem while saving Quiz";
                            return View("AuthorContent", objQuiz);
                        }
                        else if (rows == -2)
                        {
                            TempData["ErrorMessage"] = "Quiz Not Saved";
                            return View("AuthorContent", objQuiz);
                        }
                        else
                        {

                            return View("AuthorContent", objQuiz);
                        }
                    }
                }

                return View("AuthorContent", objQuiz);

            }
            catch (Exception ex)
            {
                //newException.AddDummyException("11111");
                newException.AddException(ex);
                return View("AuthorContent");
            }
        }

        public FileResult CreateScormCourse(TblQuiz objQuiz, int rows)
        {

            string coursePath = string.Empty;
            try
            {
                string SourcePath = System.Configuration.ConfigurationManager.AppSettings["ScormSourcePath"];
                string DestinationPath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                DestinationPath = DestinationPath + "\\" + objQuiz.QuizName;
                //Delete existing package
                if (Directory.Exists(DestinationPath))
                {
                    Directory.Delete(DestinationPath, true);
                }
                //create new folders for package
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                {
                    System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                }
                //replace SCORM files to root folder
                SourcePath = DestinationPath + "\\compliance\\SCORM12";

                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                {
                    System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                }
                //Delete compliance folder as it is not necessary for the package
                SourcePath = DestinationPath + "\\compliance";
                if (Directory.Exists(SourcePath))
                {
                    Directory.Delete(SourcePath, true);
                }
                //Do changes in imsmanifest.xml

                XmlDocument xmlDoc = new XmlDocument();
                string filePath = DestinationPath + "\\imsmanifest.xml";
                xmlDoc.Load(filePath);
                XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

                foreach (XmlNode xNode in xmlNodeList)
                {
                    if (xNode.Name == "organizations")
                    {
                        XmlNodeList innerNode = xNode.ChildNodes;
                        foreach (XmlNode xNode1 in innerNode)
                        {
                            if (xNode1.Name == "organization")
                            {
                                xNode1["title"].InnerText = objQuiz.QuizName;
                                XmlNodeList innerNodeNew = xNode1.ChildNodes;
                                foreach (XmlNode xNode2 in innerNodeNew)
                                {
                                    if (xNode2.Name == "item")
                                    {
                                        xNode2["title"].InnerText = objQuiz.QuizName;
                                    }
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(filePath);

                //create JSON file

                CreateQuizJSON(objQuiz, rows);
                string FolderPathToZip = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                //ZIP the folder and return path
                string startPath = FolderPathToZip + "\\" + objQuiz.QuizName;
                string zipPath = FolderPathToZip + "\\course.zip";

                var baseOutputStream = new MemoryStream();
                ZipOutputStream zipOutput = new ZipOutputStream(baseOutputStream);
                zipOutput.IsStreamOwner = false;

                zipOutput.SetLevel(3);
                byte[] buffer = new byte[4096];
                SharpZipLibHelper.ZipFolder(startPath, zipOutput);

                zipOutput.Finish();
                zipOutput.Close();

                /* Set position to 0 so that cient start reading of the stream from the begining */
                baseOutputStream.Position = 0;
                return new FileStreamResult(baseOutputStream, "application/x-zip-compressed")
                {
                    FileDownloadName = objQuiz.QuizName + ".zip"
                };
               
            }
            catch (Exception ex)
            {
                //newException.AddDummyException("11111");
                newException.AddException(ex);

            }
            return null;
        }
        public void CreateQuizJSON(TblQuiz objQuiz, int rows)
        {
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objTblQue = (object[])json_serializer.DeserializeObject(objQuiz.hdnData);
                var QuizData = quizRepository.GetQuizByID(rows);
                var ques = QuizData[0].TblQuestions;
                List<int> queIds = new List<int>();
                foreach (var que in ques)
                {
                    queIds.Add(que.QuestionId);
                }
                int index = 0;
                foreach (Dictionary<string, object> item in objQuiz.questionObject)
                {
                    item["QuestionId"] = Convert.ToString(queIds[index]);
                    index++;
                }

                    SCORMJSON jsonData = new SCORMJSON();
                ConfigData config = new ConfigData();
                config.scormType = 1.2;
                jsonData.config = config;

                QuizData quiz = new QuizData();
                quiz.title = "Quiz";
                quiz.nameLabel = "Name";
                quiz.name = objQuiz.QuizName;
                quiz.descLabel = "Description";
                quiz.description = objQuiz.QuizDescription;
                quiz.timeLabel = "Time remaining: ";
                quiz.duration = objQuiz.Duration==0?null: objQuiz.Duration;
                quiz.passingScore = 80;//This need to be change
                quiz.minScore = 0;
                quiz.maxScore = 100;
                quiz.multipleAttempts = false;
                quiz.timeoutMessage = "Exam time ended.";
                jsonData.quiz = quiz;

                ReviewQuizData reviewQuiz = new ReviewQuizData();
                reviewQuiz.title = "Review Quiz";
                reviewQuiz.nameLabel = "Name:";
                reviewQuiz.name = objQuiz.QuizName;
                reviewQuiz.descLabel = "Description:";
                reviewQuiz.description = objQuiz.QuizDescription;
                reviewQuiz.scoreLabel = "Score:";
                jsonData.reviewQuiz = reviewQuiz;

                List<QuestionsData> lstQuestions = new List<QuestionsData>();

                foreach (Dictionary<string, object> item in objQuiz.questionObject)
                {
                    QuestionsData que = new QuestionsData();
                    que.questionText = Convert.ToString(item["QuestionText"]);
                    if (Convert.ToInt32(item["QuestionTypeId"]) == 1)
                    {
                        que.type = "mcq";
                        que.instructionText = "Options";
                        que.randomOptions = Convert.ToBoolean(item["isRandomOption"]);
                        List<OptionsData> lstOptions = new List<OptionsData>();
                        var optionsCount = (object[])item["Options"];
                        int[] arrAnswers = new int[optionsCount.Length];
                        int counter = 0;


                        foreach (Dictionary<string, object> itemNew1 in (object[])item["Options"])
                        {
                            OptionsData ops = new OptionsData();
                            ops.text = Convert.ToString(itemNew1["OptionText"]);
                            ops.feedback = Convert.ToString(itemNew1["OptionFeedback"]);
                            arrAnswers[counter] = Convert.ToInt32(itemNew1["CorrectOption"]);
                            lstOptions.Add(ops);
                            counter++;
                        }
                        que.answer = arrAnswers;
                        que.points = 1;
                        que.options = lstOptions;
                    }
                    if (Convert.ToInt32(item["QuestionTypeId"]) == 2)
                    {
                        que.type = "mrq";
                        que.instructionText = "Options";
                        que.randomOptions = Convert.ToBoolean(item["isRandomOption"]);
                        List<OptionsData> lstOptions = new List<OptionsData>();
                        var optionsCount = (object[])item["Options"];
                        int[] arrAnswers = new int[optionsCount.Length];
                        int counter = 0;
                        foreach (Dictionary<string, object> itemNew1 in (object[])item["Options"])
                        {

                            OptionsData ops = new OptionsData();
                            ops.text = Convert.ToString(itemNew1["OptionText"]);
                            arrAnswers[counter] = Convert.ToInt32(itemNew1["CorrectOption"]);
                            lstOptions.Add(ops);
                            counter++;
                        }
                        que.answer = arrAnswers;
                        que.points = 1;
                        que.options = lstOptions;
                        CorrectFeedbackData correctFeedback = new CorrectFeedbackData();
                        correctFeedback.text = Convert.ToString(item["CorrectFeedback"]);
                        que.correctFeedback = correctFeedback;

                        IncorrectFeedbackData inCorrectFeedback = new IncorrectFeedbackData();
                        inCorrectFeedback.text = Convert.ToString(item["InCorrectFeedback"]);
                        que.incorrectFeedback = inCorrectFeedback;

                    }
                    if (Convert.ToInt32(item["QuestionTypeId"]) == 3)
                    {
                        que.type = "para";
                        que.placeholderText = "Write your answer here";
                        que.answer = null;
                        que.points = 1;
                        CorrectFeedbackData correctFeedback = new CorrectFeedbackData();
                        correctFeedback.text = Convert.ToString(item["CorrectFeedback"]);
                        que.correctFeedback = correctFeedback;

                        IncorrectFeedbackData inCorrectFeedback = new IncorrectFeedbackData();
                        inCorrectFeedback.text = Convert.ToString(item["InCorrectFeedback"]);
                        que.incorrectFeedback = inCorrectFeedback;

                    }
                    if (Convert.ToInt32(item["QuestionTypeId"]) == 4)
                    {
                        que.type = "video";
                        string base64String = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["mediaFile"])))
                        {
                            base64String = Convert.ToString(item["mediaFile"]);
                            byte[] newBytes = Convert.FromBase64String(base64String);
                            MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                            ms.Write(newBytes, 0, newBytes.Length);
                            string fileName = Convert.ToString(item["qTypeId"]);
                            string DestinationPath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                            DestinationPath = DestinationPath + "\\" + objQuiz.QuizName + "\\data\\media";
                            FileStream file = new FileStream(DestinationPath + "\\" + fileName, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            ms.Close();
                            que.path = "data//media//" + fileName;
                        }
                        else
                        {
                            string fileName = Convert.ToString(item["qTypeId"]);
                            var extn = fileName.Split('.');
                            int count = extn.Length;
                            fileName = Convert.ToString(item["QuestionId"]) + "." + extn[count - 1];
                            string DestinationPath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                            DestinationPath = DestinationPath + "\\" + objQuiz.QuizName + "\\data\\media";
                            string path = System.Configuration.ConfigurationManager.AppSettings["QuizMediaPath"];
                            System.IO.File.Move(path + "\\" + fileName, DestinationPath + "\\" + fileName);
                            que.path = "data//media//" + fileName;
                        }

                        
                    }
                    if (Convert.ToInt32(item["QuestionTypeId"]) == 5)
                    {
                        que.type = "audio";
                        string base64String = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["mediaFile"])))
                        {
                            base64String = Convert.ToString(item["mediaFile"]);
                            byte[] newBytes = Convert.FromBase64String(base64String);
                            MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                            ms.Write(newBytes, 0, newBytes.Length);
                            string fileName = Convert.ToString(item["qTypeId"]);
                            string DestinationPath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                            DestinationPath = DestinationPath + "\\" + objQuiz.QuizName + "\\data\\media";
                            FileStream file = new FileStream(DestinationPath + "\\" + fileName, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            ms.Close();
                            que.path = "data//media//" + fileName;
                        }
                        else
                        {
                            string fileName = Convert.ToString(item["qTypeId"]);
                            var extn = fileName.Split('.');
                            int count = extn.Length;
                            fileName = Convert.ToString(item["QuestionId"]) + "." + extn[count-1];

                            string DestinationPath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                            DestinationPath = DestinationPath + "\\" + objQuiz.QuizName + "\\data\\media";
                            string path = System.Configuration.ConfigurationManager.AppSettings["QuizMediaPath"];
                            System.IO.File.Move(path + "\\" + fileName, DestinationPath + "\\" + fileName);
                            que.path = "data//media//" + fileName;
                        }
                    }

                    lstQuestions.Add(que);
                }
                jsonData.questions = lstQuestions;
                string jsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ScormDestinationPath"];
                jsonFilePath = jsonFilePath + "\\" + objQuiz.QuizName + "\\data\\json\\";
                var json = new JavaScriptSerializer().Serialize(jsonData);
                System.IO.File.WriteAllText(jsonFilePath + "quizdata.json", json);

            }
            catch (Exception ex)
            {
                newException.AddException(ex);
            }

        }
        public ActionResult EditQuiz(int id)
        {
            List<TblQuiz> objQuiz = new List<TblQuiz>();
            try
            {

                objQuiz = quizRepository.GetQuizByID(id);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;

                objQuiz[0].hdnEditData = json_serializer.Serialize(objQuiz[0]);
                return View("EditNewQuiz", objQuiz[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                return View("EditNewQuiz", null);
            }
        }
        [HttpPost]
        public ActionResult AssignQuizToUsers(QuizAssignViewModel quizAssignViewModel)
        {
            var index = quizRepository.DeleteAssignedUser(quizAssignViewModel.quiz.QuizId);

            foreach (var userId in quizAssignViewModel.userIds)
            {
                var result = quizRepository.AssignQuiz(quizAssignViewModel.quiz.QuizId, userId, quizAssignViewModel.DueDate);

                var emailBody = quizAssignViewModel.quiz.QuizName + " - assigned to you. Please go through it. <br /> Your Due Date is - " + quizAssignViewModel.DueDate;
                var emailSubject = "Course Assigned - " + quizAssignViewModel.quiz.QuizName;
                tblEmails objEmail = new tblEmails();
                var objUser = userRepository.GetUserById(userId);
                objEmail.EmailTo = objUser[0].EmailId;
                objEmail.EmailSubject = emailSubject;
                objEmail.EmailBody = emailBody;
                var emailResult = userRepository.InsertEmail(objEmail);
            }

            return RedirectToAction("Index");
        }
        public ActionResult ViewQuiz(int id)
        {
            try
            {
                TblUser sessionUser = (TblUser)Session["UserSession"];
                List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
                lstAllQuiz = quizRepository.GetQuizByID(id);

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                lstAllQuiz[0].hdnViewData = json_serializer.Serialize(lstAllQuiz[0]);

                return View("ViewAdminQuiz", lstAllQuiz[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                return View("ViewAdminQuiz");
            }
        }


        //New Section of Quiz Authoring

        public ActionResult AuthorContent(string id)
        {
            
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    List<TblQuiz> objQuiz = new List<TblQuiz>();
                    objQuiz = quizRepository.GetQuizByID(Convert.ToInt32(id));                   
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    json_serializer.MaxJsonLength = int.MaxValue;

                    objQuiz[0].hdnEditData = json_serializer.Serialize(objQuiz[0]);
                    return View(objQuiz[0]);
                }
                else
                {
                    TblQuiz objQuiz = new TblQuiz();
                    return View(objQuiz);
                }
                
            }
            catch (Exception ex)
            {
                newException.AddException(ex);                
            }
            return View();
            //return View(objQuiz[0]);
        }


        public ActionResult TestYourKnowledge()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            TblEnquiry tblEnquiry = new TblEnquiry();
            try
            {
                DataSet ds = quizRepository.GetCountries();
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
                                    Value = Convert.ToString(dr["CountryName"])
                                });
                            }
                        }
                    }
                }
                Session["countriesItem"] = countriesItem;
                ViewBag.CountryList = countriesItem;
            }
            catch (Exception ex)
            {
                newException.AddException(ex);

            }
            return View(tblEnquiry);

        }

        public ActionResult SubmitEnquiry(TblEnquiry tblEnquiry)
        {
            try
            {
                List<SelectListItem> countriesItem = new List<SelectListItem>();
                countriesItem = (List<SelectListItem>)Session["countriesItem"];
                ViewBag.CountryList = countriesItem;
                var result = quizRepository.InsertEnquiryData(tblEnquiry);
                if (result > 0)
                {
                    Session["Enquiry"] = result;
                    TempData["Added"] = "Enquiry added succefully";
                    return RedirectToAction("StartQuiz");
                }
                else
                {
                    TempData["Message"] = "Email Id already exist";
                }
            }
            catch (Exception ex)
            {
                newException.AddException(ex);

            }
            return View("TestYourKnowledge");
        }

        public ActionResult StartQuiz()
        {
            try
            {
                int UserId = Convert.ToInt32(Session["Enquiry"]);

                int QuizId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SurveyId"]);
                var result = quizRepository.AssignQuiz(QuizId, UserId, null);

                var lstAllQuiz = quizRepository.GetQuizForLaunch(QuizId, UserId);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                return View("InitialAssessment", lstAllQuiz[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                TempData["Issue"] = "Issue Occured";
                List<SelectListItem> countriesItem = new List<SelectListItem>();
                countriesItem = (List<SelectListItem>)Session["countriesItem"];
                return View("TestYourKnowledge");
            }
        }

        public ActionResult SubmitInitialAssessment(TblQuiz objQuiz)
        {
            List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
            try
            {
                int UserId = Convert.ToInt32(Session["Enquiry"]);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                List<QueOptions> lstQueOptions = new List<QueOptions>();
                object[] objQueResponse = (object[])json_serializer.DeserializeObject(objQuiz.hdnResponseData);

                int attempt = 1;

                if (objQuiz.completeTime == "0" || string.IsNullOrEmpty(objQuiz.completeTime))
                {
                    var durationInSeconds = Convert.ToInt32(objQuiz.Duration) * 60;
                    TimeSpan t = TimeSpan.FromSeconds(durationInSeconds);
                    objQuiz.completeTime = string.Format("{0:D2}:{1:D2}", (int)t.Minutes, t.Seconds);
                }
                else
                {

                    var cTime = objQuiz.completeTime;
                    int index1 = cTime.IndexOf(":");
                    int index2 = cTime.IndexOf("Minutes");
                    int index3 = cTime.IndexOf(",");
                    int index4 = cTime.IndexOf("Seconds");
                    var cMin = cTime.Substring(index1 + 1, (index2 - (index1 + 2)));
                    var cSec = cTime.Substring(index3 + 1, (index4 - (index3 + 2)));
                    var remainingTime = (Convert.ToInt32(cMin) * 60) + Convert.ToInt32(cSec);
                    remainingTime = Convert.ToInt32(objQuiz.Duration * 60) - remainingTime;
                    TimeSpan t = TimeSpan.FromSeconds(remainingTime);
                    objQuiz.completeTime = string.Format("{0:D2}:{1:D2}", (int)t.Minutes, t.Seconds);
                }


                foreach (var item in objQueResponse)
                {
                    QuizResponse quizResponse = new QuizResponse();
                    quizResponse.QuizId = objQuiz.QuizId;
                    quizResponse.UserId = UserId;
                    quizResponse.Attempt = attempt;

                    foreach (Dictionary<string, object> newItem in (object[])item)
                    {
                        var questionId = newItem["questionId"];
                        quizResponse.QuestionId = Convert.ToInt32(newItem["questionId"]);
                        quizResponse.QuestionFeedback = Convert.ToString(newItem["queFeedback"]);
                        if (string.IsNullOrEmpty(quizResponse.OptionIds))
                            quizResponse.OptionIds = Convert.ToString(newItem["optionId"]);
                        else
                            quizResponse.OptionIds += "," + Convert.ToString(newItem["optionId"]);
                    }
                    QueOptions newQueOption = new QueOptions();
                    newQueOption.QuestionId = quizResponse.QuestionId;
                    newQueOption.OptionsIds = quizResponse.OptionIds;
                    lstQueOptions.Add(newQueOption);

                    var result = quizRepository.CaptureResponses(quizResponse);
                }


                lstAllQuiz = quizRepository.GetQuizForLaunch(objQuiz.QuizId, UserId);
                int noOfQues = 0;
                var score = 0;
                foreach (var question in lstAllQuiz[0].TblQuestions)
                {
                    if (question.QuestionTypeId == 1)
                    {
                        noOfQues++;
                        foreach (var option in question.TblQuestionOptions)
                        {
                            if (option.CorrectOption == true)
                            {
                                foreach (var que in lstQueOptions)
                                {
                                    if (que.QuestionId == question.QuestionId)
                                    {
                                        if (option.OptionId == Convert.ToInt32(que.OptionsIds))
                                        {
                                            score++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (question.QuestionTypeId == 2)
                    {
                        noOfQues++;
                        int correctCount = 0;
                        int[] Ids = new int[question.TblQuestionOptions.Count];
                        foreach (var option in question.TblQuestionOptions)
                        {
                            if (option.CorrectOption == true)
                            {
                                Ids[correctCount] = option.OptionId;
                                correctCount++;
                            }
                        }
                        foreach (var item in lstQueOptions)
                        {
                            if (item.QuestionId == question.QuestionId)
                            {
                                var optionIds = item.OptionsIds.Split(',');
                                if (correctCount == optionIds.Length)
                                {
                                    var correct = 0;
                                    foreach (var option in optionIds)
                                    {
                                        foreach (var id in Ids)
                                        {
                                            if (id == Convert.ToInt32(option))
                                            {
                                                correct++;
                                            }
                                        }
                                    }
                                    if (correctCount == correct)
                                    {
                                        score++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (noOfQues == 0)
                {
                    score = 0;
                }
                var scoreResult = quizRepository.CaptureScore(objQuiz, UserId, score, attempt);

                // newException.AddDummyException("Responses Saved Successfully");
                TempData["Message"] = "Responses Saved Successfully";
                //return View("InitialAssessment", lstAllQuiz[0]);
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                TempData["Issue"] = "Issue Occured";
                return View("InitialAssessment", lstAllQuiz[0]);
            }
            //return RedirectToAction("Result");
        }

        public ActionResult Result()
        {
            EnquiryResult enquiryResult = new EnquiryResult();
            try
            {
                int UserId = Convert.ToInt32(Session["Enquiry"]);
                int QuizId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SurveyId"]);
                enquiryResult = quizRepository.GetEnquiryResult(UserId, QuizId);
                if (enquiryResult.Score > 0)
                    enquiryResult.Score = (enquiryResult.Score * 100) / enquiryResult.NoOfQuestions;
                //enquiryResult.AssessmentLink = System.Configuration.ConfigurationManager.AppSettings["AssessmentLink"];
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                TempData["Issue"] = "Issue Occured";                
            }
            return View(enquiryResult);
        }

        public ActionResult ShareLink(string emailId)
        {
            string AssessmentLink = System.Configuration.ConfigurationManager.AppSettings["AssessmentLink"];
            TblEnquiry tblEnquiry = new TblEnquiry();
            tblEnquiry.EmailId = emailId;
            tblEnquiry.isShared = true;
            var result = quizRepository.InsertEnquiryData(tblEnquiry);
            if (result > 0)
            {
                var link = "<a href='" + AssessmentLink + "'>Click here to test your knowledge</a>";
                var emailBody = "Hello <br> Your friend shared assessment to test your knowledge. Please click on below link to take a test  - " + link;
                var emailSubject = "Test your knowledge";
                tblEmails objEmail = new tblEmails();

                objEmail.EmailTo = emailId;
                objEmail.EmailSubject = emailSubject;
                objEmail.EmailBody = emailBody;
                var emailResult = userRepository.InsertEmail(objEmail);

            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}