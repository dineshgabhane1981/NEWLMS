using LMSBL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LMSWeb.Controllers
{
    public class callbackController : Controller
    {
        // GET: callback
        
        public string Index(string code)
        {
            Exceptions newException = new Exceptions();
            newException.AddDummyException(code);
            //var json = JsonConvert.SerializeObject(code);
            return code;

        }
    }
}