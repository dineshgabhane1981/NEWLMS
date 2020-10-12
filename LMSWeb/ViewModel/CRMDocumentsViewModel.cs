using LMSBL.DBModels.CRMNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSWeb.ViewModel
{
    public class CRMDocumentsViewModel
    {
        public tblCRMDocument objCRMDocument { get; set; }
        public List<tblCRMDocument> objCRMDocumentLST { get; set; }
        public List<SelectListItem> lstCRMclient { get; set; }
        public string documentData { get; set; }
    }
}