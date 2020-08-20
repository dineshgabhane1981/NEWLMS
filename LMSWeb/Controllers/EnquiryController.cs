using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LMSWeb.ViewModel;
using LMSBL.Common;
using LMSBL.DBModels;
using LMSBL.Repository;
using System.Data;

namespace LMSWeb.Controllers
{
    public class EnquiryController : Controller
    {
        // GET: Enquiry
        UserRepository userRepository = new UserRepository();       
        Exceptions newException = new Exceptions();
        public ActionResult Index()
        {
            return View();
        }

        public string GetEnquiry(string emailContent)
        {
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objTblQue = (object[])json_serializer.DeserializeObject(emailContent);
                foreach (Dictionary<string, object> item in objTblQue)
                {
                    //var emailBody = objTblQue.Message;
                    var emailSubject = "New Enquiry ";
                    tblEmails objEmail = new tblEmails();


                    var emailBody = "";
                    emailBody += "<p></p>Name :- " + Convert.ToString(item["Name"]);
                    emailBody += "<p></p>Email Id :- " + Convert.ToString(item["Email"]);
                    emailBody += "<p></p>Contact No :- " + Convert.ToString(item["Contact"]);
                    emailBody += "<p></p>Service Enquired About :- " + Convert.ToString(item["Service"]);
                    emailBody += "<p></p>Message :- " + Convert.ToString(item["Message"]);
                    
                    objEmail.EmailTo = "sales@logicieltech.com";
                    objEmail.EmailSubject = emailSubject;
                    objEmail.EmailBody = emailBody;
                    var emailResult = userRepository.InsertEmail(objEmail);

                    return "Sent Successfully";
                }
            }
            catch(Exception ex)
            {
                newException.AddDummyException(ex.ToString());
                return "Failded";

            }
            return "Failed";

        }
    }
}