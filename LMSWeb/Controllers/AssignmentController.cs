using System;
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
using System.Data;

namespace LMSWeb.Controllers
{
    public class AssignmentController : Controller
    {
        QuizRepository quizRepository = new QuizRepository();
        CoursesRepository cr = new CoursesRepository();
        UserRepository ur = new UserRepository();
        CurriculumRepository cc = new CurriculumRepository();
        Exceptions newException = new Exceptions();
        public static List<QuizSession> lstQuiz = new List<QuizSession>();

        HomeRepository hm = new HomeRepository();
        // GET: Assignment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyAssignments()
        {
            try
            {
                var model = (TblUser)Session["UserSession"];
                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.lstActivities = hm.GetAllLearnerActivities(model.TenantId, model.UserId, "Total");
                return View("MyLearning", homeViewModel.lstActivities);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                return View("MyLearning");
            }

        }

        public ActionResult AssignCources()
        {
            //Admin -> show selected course details in popup with user list. 
            return View();
        }

        public ActionResult code()
        {
            return null;
        }


        public async Task<ActionResult> LaunchQuiz(int QuizId, string code)
        //public ActionResult LaunchQuiz(int QuizId, string code)
        {

            //newException.AddDummyException("Quiz ID First - " + Convert.ToString(QuizId));
            bool isPlek = false;
            
            try
            {
                TblUser sessionUser = (TblUser)Session["UserSession"];
                if (string.IsNullOrEmpty(code))
                {

                    QuizSession quizForSession = new QuizSession();
                    quizForSession.QuizId = Convert.ToInt32(QuizId);
                    quizForSession.isUsed = false;
                    lstQuiz.Add(quizForSession);
                    Session["QuizId"] = lstQuiz;

                    if (string.IsNullOrEmpty(Convert.ToString(Session["QuizId"])))
                    {
                        newException.AddDummyException("Quiz Id in LaunchQuiz is Null");
                    }

                }
                List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
                if (sessionUser == null)
                {

                    TenantRepository tr = new TenantRepository();
                    List<TblTenant> tenantDetails = new List<TblTenant>();
                    string host = Request.Url.Host;

                    var tenantList1 = tr.VerifyTenantDomain(host);
                    if (tenantList1.Count > 0)
                    {
                        if (tenantList1[0].TenantId == 6)
                        {
                            if (string.IsNullOrEmpty(code))
                            {
                                return Redirect("https://pumplace.plek.co/oidc/auth?client_id=1663f8ca-7b8a-42eb-ae3b-845e32be1baf&scope=openid&response_type=code&redirect_uri=https://quiz.rockettech.co.nz/");
                            }
                            else
                            {

                                using (var client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Accept.Clear();
                                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                                    string tokenApi = "https://pumplace.plek.co/oidc/token";
                                    var values = new Dictionary<string, string>();
                                    values.Add("client_id", "1663f8ca-7b8a-42eb-ae3b-845e32be1baf");
                                    values.Add("client_secret", "TZD4Jr837e6uymsJdyxH2jk8qxGQiK7gXDCmYqtC");
                                    values.Add("grant_type", "authorization_code");
                                    values.Add("code", code);
                                    values.Add("redirect_uri", "https://quiz.rockettech.co.nz/");

                                    var content = new FormUrlEncodedContent(values);

                                    var responseTask = await client.PostAsync(tokenApi, content);
                                    var result = await responseTask.Content.ReadAsStringAsync();
                                    var data = (JObject)JsonConvert.DeserializeObject(result);
                                    string token = data["access_token"].Value<string>();//Access Token Received                           

                                    //Now get User Details
                                    string userApi = "https://pumplace.plek.co/oidc/me";

                                    client.DefaultRequestHeaders.Accept.Clear();
                                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                                    HttpResponseMessage response = await client.GetAsync(userApi);
                                    if (response.IsSuccessStatusCode)
                                    {

                                        string jsondata = await response.Content.ReadAsStringAsync();

                                        var userData = (JObject)JsonConvert.DeserializeObject(jsondata);
                                        string email = userData["email"].Value<string>();
                                        string name = userData["username"].Value<string>();

                                        var userId = ur.IsUserExist(email, Request.Url.Host);

                                        if (userId == 0)
                                        {
                                            try
                                            {

                                                //Create user
                                                TblUser newUser = new TblUser();
                                                newUser.FirstName = name;
                                                newUser.EmailId = email;
                                                newUser.RoleId = 3;
                                                newUser.IsActive = true;
                                                newUser.CreatedBy = 1;
                                                CommonFunctions common = new CommonFunctions();
                                                newUser.Password = common.GetEncodePassword("123456");


                                                var tenantList = tr.VerifyTenantDomain(Request.Url.Host);
                                                newUser.TenantId = tenantList[0].TenantId;

                                                var newUserId = ur.AddUser(newUser);


                                                if (newUserId > 0)
                                                {
                                                    //Assign Quiz to Newly Created User
                                                    //newException.AddDummyException("111 - " + Convert.ToString(QuizId));
                                                    var objQuiz = quizRepository.GetQuizByID(Convert.ToInt32(QuizId));
                                                    if (objQuiz != null)
                                                    {

                                                        if (objQuiz[0] != null)
                                                        {
                                                            if (objQuiz[0].TenantId == 6)
                                                            {
                                                                var assign = quizRepository.AssignQuiz(Convert.ToInt32(QuizId), newUserId, null);
                                                            }
                                                        }
                                                    }
                                                    userId = newUserId;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                newException.AddException(ex);
                                            }
                                        }
                                        else
                                        {
                                            //check Quiz assigned or not                                    

                                            var isAssigned = quizRepository.CheckQuizAssignedUser(Convert.ToInt32(QuizId), userId);
                                            if (!isAssigned)
                                            {
                                                newException.AddDummyException("222 - " + QuizId);
                                                var objQuiz = quizRepository.GetQuizByID(Convert.ToInt32(QuizId));
                                                if (objQuiz != null)
                                                {
                                                    if (objQuiz[0] != null)
                                                    {
                                                        if (objQuiz[0].TenantId == 6)
                                                        {
                                                            var assign = quizRepository.AssignQuiz(Convert.ToInt32(QuizId), userId, null);
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                        List<TblUser> tblUser = ur.GetUserById(userId);
                                        Session["UserSession"] = tblUser[0];
                                        lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), userId);

                                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                                        json_serializer.MaxJsonLength = int.MaxValue;
                                        lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);

                                        isPlek = true;
                                    }
                                }
                            }
                        }
                        if (tenantList1[0].TenantId == 9)
                        {
                            //newException.AddDummyException("In - 111 ");
                            CommonFunctions common = new CommonFunctions();
                            var password = common.GetEncodePassword("123456");
                            TblUser tblUser = ur.IsValidUser("jeaninhetpanhuisilms@gmail.com", password, Request.Url.Host);
                            lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), tblUser.UserId);

                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            json_serializer.MaxJsonLength = int.MaxValue;
                            lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                            //newException.AddDummyException("In - 222 ");
                            isPlek = true;
                        }                        
                    }

                }
                else
                {

                    if (sessionUser.TenantId == 6)
                    {
                        isPlek = true;
                        //check Quiz assigned or not

                        var isAssigned = quizRepository.CheckQuizAssignedUser(Convert.ToInt32(QuizId), sessionUser.UserId);
                        //newException.AddDummyException(Convert.ToString(isAssigned));
                        if (!isAssigned)
                        {
                            //newException.AddDummyException("333 - " + QuizId);
                            var objQuiz = quizRepository.GetQuizByID(Convert.ToInt32(QuizId));
                            if (objQuiz != null)
                            {
                                if (objQuiz[0] != null)
                                {
                                    if (objQuiz[0].TenantId == 6)
                                    {
                                        var assign = quizRepository.AssignQuiz(Convert.ToInt32(QuizId), sessionUser.UserId, null);
                                        lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), sessionUser.UserId);
                                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                                        json_serializer.MaxJsonLength = int.MaxValue;
                                        lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), sessionUser.UserId);
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            json_serializer.MaxJsonLength = int.MaxValue;
                            lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                        }
                    }
                    if (sessionUser.TenantId == 9)
                    {
                        //newException.AddDummyException("In - 111 ");
                        CommonFunctions common = new CommonFunctions();
                        var password = common.GetEncodePassword("123456");
                        TblUser tblUser = ur.IsValidUser("jeaninhetpanhuisilms@gmail.com", password, Request.Url.Host);
                        lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), tblUser.UserId);

                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        json_serializer.MaxJsonLength = int.MaxValue;
                        lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                        //newException.AddDummyException("In - 222 ");
                        isPlek = true;
                    }
                    else
                    {
                        lstAllQuiz = quizRepository.GetQuizForLaunch(Convert.ToInt32(QuizId), sessionUser.UserId);
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        json_serializer.MaxJsonLength = int.MaxValue;
                        lstAllQuiz[0].hdnLaunchData = json_serializer.Serialize(lstAllQuiz[0]);
                    }
                }
                if (isPlek)
                    return View("LaunchQuizForPlek", lstAllQuiz[0]);
                else
                    return View("LaunchQuizNew", lstAllQuiz[0]);
            }
            catch (Exception ex)
            {
                newException.AddException(ex);
                if (isPlek)
                    return View("LaunchQuizForPlek");
                else
                    return View("LaunchQuizNew");
            }
        }

        [HttpPost]
        public ActionResult SubmitAssignCources()
        {
            //Admin -> in popup submit click. 
            //assign select course to selected users. 
            return View();
        }

        [HttpPost]
        public ActionResult SubmitQuiz(TblQuiz objQuiz)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();

            TblUser sessionUser = (TblUser)Session["UserSession"];
            if (sessionUser == null)
            {
                //newException.AddDummyException("In - 333 ");
                CommonFunctions common = new CommonFunctions();
                var password = common.GetEncodePassword("123456");
                sessionUser = ur.IsValidUser("jeaninhetpanhuisilms@gmail.com", password, Request.Url.Host);
            }
            if (sessionUser.RoleId == 2)
            {
                int result = quizRepository.DeleteResponse(objQuiz.QuizId, sessionUser.UserId, 1);
            }
            List<QueOptions> lstQueOptions = new List<QueOptions>();
            object[] objQueResponse = (object[])json_serializer.DeserializeObject(objQuiz.hdnResponseData);
            var attempt = quizRepository.GetQuizAttemptByUserID(objQuiz.QuizId, sessionUser.UserId);
            attempt = attempt + 1;

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
                quizResponse.UserId = sessionUser.UserId;
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

            List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
            lstAllQuiz = quizRepository.GetQuizForLaunch(objQuiz.QuizId, sessionUser.UserId);
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
            var scoreResult = quizRepository.CaptureScore(objQuiz, sessionUser.UserId, score, attempt);
            var emailBody = "Thank you for taking Quiz.";
            if (noOfQues > 0)
            {
                emailBody = "Thank you for taking Quiz. </br> Your score is " + ((score * 100) / noOfQues) + "% <br />";
            }
            //var emailBody = "Thank you for taking Quiz. </br> Your score is " + ((score * 100) / noOfQues) + "% <br />";
            var emailSubject = "Quiz Result";
            tblEmails objEmail = new tblEmails();
            objEmail.EmailTo = sessionUser.EmailId;
            objEmail.EmailSubject = emailSubject;
            objEmail.EmailBody = emailBody;
            if (sessionUser.TenantId != 6)
            {
                var emailResult = ur.InsertEmail(objEmail);
            }
            newException.AddDummyException("Responses Saved Successfully");
            TempData["Message"] = "Responses Saved Successfully";
            return RedirectToAction("ReviewQuiz", new { @QuizId = objQuiz.QuizId });
        }

        public ActionResult RatingAndFeedback(int ActivityId, string LearningType)
        {
            tblRatings objRating = new tblRatings();
            objRating.ActivityId = ActivityId;
            objRating.ActivityType = LearningType;
            //objRating.Attempt = attempt;
            return View(objRating);
        }

        public ActionResult SubmitRating(tblRatings objRating)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            objRating.UserId = sessionUser.UserId;
            objRating.TenantId = sessionUser.TenantId;
            //objRating.ActivityType = "Quiz";
            var result = quizRepository.CaptureRatings(objRating);
            return RedirectToAction("MyAssignments");
        }

        public ActionResult ReviewQuiz(int QuizId)
        {
            bool isPlek = false;
            try
            {
                //newException.AddDummyException("In - 1 ");

                TblUser sessionUser = (TblUser)Session["UserSession"];
                if (sessionUser == null)
                {
                    //newException.AddDummyException("In - 444 ");
                    CommonFunctions common = new CommonFunctions();
                    var password = common.GetEncodePassword("123456");
                    sessionUser = ur.IsValidUser("jeaninhetpanhuisilms@gmail.com", password, Request.Url.Host);
                    Session["UserSession"] = sessionUser;
                    isPlek = true;
                }
                if (sessionUser.TenantId == 9)
                {
                    CommonFunctions common = new CommonFunctions();
                    var password = common.GetEncodePassword("123456");
                    sessionUser = ur.IsValidUser("jeaninhetpanhuisilms@gmail.com", password, Request.Url.Host);
                    Session["UserSession"] = sessionUser;
                    isPlek = true;
                }
                if (sessionUser.TenantId == 6)
                {
                    isPlek = true;
                }
                List<TblQuiz> lstAllQuiz = new List<TblQuiz>();
                lstAllQuiz = quizRepository.GetQuizForLaunch(QuizId, sessionUser.UserId);
                var attempt = quizRepository.GetQuizAttemptByUserID(QuizId, sessionUser.UserId);
                List<TblRespons> quizResponses = new List<TblRespons>();
                quizResponses = quizRepository.GetQuizResponsesByUserID(QuizId, sessionUser.UserId, attempt);
                lstAllQuiz[0].TblResponses = quizResponses;

                var noOfQuestions = lstAllQuiz[0].TblQuestions.Where(x => x.QuestionTypeId == 1 || x.QuestionTypeId == 2).Count();
                //newException.AddDummyException("In - noOfQuestions " + noOfQuestions);
                var score = quizRepository.GetQuizScoreByUserID(QuizId, sessionUser.UserId, attempt);

                //newException.AddDummyException("In - Score " + score.Score);
                if (score.Score == 0)
                {
                    lstAllQuiz[0].Score = "0";
                }
                else
                {
                    lstAllQuiz[0].Score = Convert.ToString(Math.Round(Convert.ToDecimal(score.Score * 100 / noOfQuestions), 2));
                }
                //newException.AddDummyException("In - 6 ");
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                lstAllQuiz[0].hdnReviewData = json_serializer.Serialize(lstAllQuiz[0]);
                //newException.AddDummyException("In - 7 ");
                if (isPlek)
                {
                    //newException.AddDummyException("In - PLEK ");
                    return View("ReviewQuizLearnerForPlek", lstAllQuiz[0]);
                }
                else
                {
                    //newException.AddDummyException("Out - PLEK ");
                    return View("ReviewQuizLearner", lstAllQuiz[0]);
                }
            }
            catch (Exception ex)
            {
                newException.AddDummyException(ex.ToString());
                if (isPlek)
                    return View("ReviewQuizLearnerForPlek", null);
                else
                    return View("ReviewQuizLearner", null);
            }
        }

        public async Task<ActionResult> LaunchCourse(int CourseId, string code)
        {
            TenantRepository tr = new TenantRepository();
            var tenantList = tr.VerifyTenantDomain(Request.Url.Host);
            TblUser sessionUser = (TblUser)Session["UserSession"];
            var courseBaseURL = System.Configuration.ConfigurationManager.AppSettings["CourseBaseURL"];
            if (sessionUser == null)
            {

                if (string.IsNullOrEmpty(code))
                {

                    CourseSession courseSession = new CourseSession();
                    courseSession.CourseId = CourseId;
                    Session["CourseId"] = CourseId;
                    return Redirect("https://pumplace.plek.co/oidc/auth?client_id=1663f8ca-7b8a-42eb-ae3b-845e32be1baf&scope=openid&response_type=code&redirect_uri=https://quiz.rockettech.co.nz/");
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                        string tokenApi = "https://pumplace.plek.co/oidc/token";
                        var values = new Dictionary<string, string>();
                        values.Add("client_id", "1663f8ca-7b8a-42eb-ae3b-845e32be1baf");
                        values.Add("client_secret", "TZD4Jr837e6uymsJdyxH2jk8qxGQiK7gXDCmYqtC");
                        values.Add("grant_type", "authorization_code");
                        values.Add("code", code);
                        values.Add("redirect_uri", "https://quiz.rockettech.co.nz/");

                        var content = new FormUrlEncodedContent(values);

                        var responseTask = await client.PostAsync(tokenApi, content);
                        var result = await responseTask.Content.ReadAsStringAsync();
                        var data = (JObject)JsonConvert.DeserializeObject(result);
                        string token = data["access_token"].Value<string>();//Access Token Received                           

                        //Now get User Details
                        string userApi = "https://pumplace.plek.co/oidc/me";

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        HttpResponseMessage response = await client.GetAsync(userApi);
                        if (response.IsSuccessStatusCode)
                        {
                            string jsondata = await response.Content.ReadAsStringAsync();

                            var userData = (JObject)JsonConvert.DeserializeObject(jsondata);
                            string email = userData["email"].Value<string>();
                            string name = userData["username"].Value<string>();

                            var userId = ur.IsUserExist(email, Request.Url.Host);

                            if (userId == 0)
                            {
                                try
                                {
                                    //Create user
                                    TblUser newUser = new TblUser();
                                    newUser.FirstName = name;
                                    newUser.EmailId = email;
                                    newUser.RoleId = 3;
                                    newUser.IsActive = true;
                                    newUser.CreatedBy = 1;
                                    CommonFunctions common = new CommonFunctions();
                                    newUser.Password = common.GetEncodePassword("123456");

                                    
                                    newUser.TenantId = tenantList[0].TenantId;

                                    var newUserId = ur.AddUser(newUser);
                                    var assign = cr.AssignCourse(CourseId, newUserId, null);
                                    userId = newUserId;

                                }
                                catch (Exception ex)
                                {
                                    newException.AddException(ex);
                                }
                            }
                            else
                            {
                                try
                                {
                                    var isAssigned = cr.CheckCourseAssignedUser(CourseId, userId);
                                    if (!isAssigned)
                                    {
                                        var assign = cr.AssignCourse(CourseId, userId, null);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    newException.AddException(ex);
                                }
                            }

                            var urlNew = courseBaseURL + "?AID=" + CourseId + "&LID=" + userId;
                            return Redirect(urlNew);
                            //return Content("<script>window.open('" + urlNew + "','_blank')</script>");
                        }
                    }
                }
            }

            List<tblCourse> CourseDetails = new List<tblCourse>();
            CoursesRepository courseRepo = new CoursesRepository();
            CourseDetails = courseRepo.GetCourseById(CourseId);
            var url = courseBaseURL + "?AID=" + CourseId + "&LID=" + sessionUser.UserId;
            CourseDetails[0].ContentModuleURL = url;
            if(tenantList[0].TenantId==6)
            {
                return Content("<script>window.open('" + url + "','_blank')</script>");
            }
            else
            {
                return View(CourseDetails[0]);
            }
            
            //return Redirect(url);
            //return Content("<script>window.open('" + url + "','_blank')</script>");


        }

        public ActionResult GetCurriculumActivities(string cId)
        {
            TblUser sessionUser = (TblUser)Session["UserSession"];
            List<CurriculumActivities> lstCurriculumActivities = new List<CurriculumActivities>();
            DataSet Data = cc.GetCurriculumActivitiesById(Convert.ToInt32(cId), sessionUser.UserId);

            if (Data != null)
            {
                if (Data.Tables.Count > 0)
                {
                    if (Data.Tables[0].Rows.Count > 0)
                    {
                        foreach (var item in Data.Tables[0].Rows)
                        {
                            lstCurriculumActivities = Data.Tables[0].AsEnumerable().Select(dr => new CurriculumActivities
                            {
                                ActivityId = Convert.ToInt32(dr["ActivityId"]),
                                ActivityText = Convert.ToString(dr["ActivityText"]),
                                ActivityType = Convert.ToString(dr["ActivityType"]),
                                DueDate = Convert.ToString(dr["DueDate"]),
                                ActivityStatus = Convert.ToString(dr["ActivityStatus"]),
                                CompletionDate = Convert.ToString(dr["CompletionDate"]),
                                Duration = Convert.ToString(dr["Duration"])
                            }).ToList();
                        }
                    }
                }
            }
            return Json(lstCurriculumActivities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LaunchCurriculum(string cId)
        {
            TblUser sessionUser = new TblUser();
            sessionUser = (TblUser)Session["UserSession"];
            List<CurriculumActivities> lstCurriculumActivities = new List<CurriculumActivities>();
            if (!string.IsNullOrEmpty(cId))
            {
                
                DataSet Data = cc.GetCurriculumActivitiesById(Convert.ToInt32(cId), sessionUser.UserId);

                if (Data != null)
                {
                    if (Data.Tables.Count > 0)
                    {
                        if (Data.Tables[0].Rows.Count > 0)
                        {
                            foreach (var item in Data.Tables[0].Rows)
                            {
                                lstCurriculumActivities = Data.Tables[0].AsEnumerable().Select(dr => new CurriculumActivities
                                {
                                    ActivityId = Convert.ToInt32(dr["ActivityId"]),
                                    ActivityText = Convert.ToString(dr["ActivityText"]),
                                    ActivityType = Convert.ToString(dr["ActivityType"]),
                                    DueDate = Convert.ToString(dr["DueDate"]),
                                    ActivityStatus = Convert.ToString(dr["ActivityStatus"]),
                                    CompletionDate = Convert.ToString(dr["CompletionDate"]),
                                    Duration = Convert.ToString(dr["Duration"]),
                                    Score = Convert.ToString(dr["Score"]),
                                    CurriculumName = Convert.ToString(dr["CurriculumTitle"]),
                                    CourseSubType = Convert.ToString(dr["CourseSubType"]),
                                    CourseModuleURL = Convert.ToString(dr["CourseModuleURL"])                                    
                                }).ToList();
                            }
                        }
                    }
                }
                //return View("CurriculumActivities", lstCurriculumActivities);
            }
            return View(lstCurriculumActivities);
        }
    }
}