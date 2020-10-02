using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSWeb.Controllers
{
    public class ClientsController : Controller
    {
        // GET: Clients
        public ActionResult Enquiry()
        {
            return View();
        }
        public ActionResult PotentialClients()
        {
            return View();
        }
        public ActionResult Clients()
        {
            return View();
        }
    }
}