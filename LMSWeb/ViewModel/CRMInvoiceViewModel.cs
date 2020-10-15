using LMSBL.DBModels.CRMNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSWeb.ViewModel
{
    public class CRMInvoiceViewModel
    {
        public tblCRMInvoice ObjCRMInvoivce { get; set; }
        public List<tblCRMInvoiceItem> ObjCRMInvoiceItemLST { get; set; }
        public List<SelectListItem> lstCRMclient { get; set; }
        
        public string JsonData { get; set; }
        public int Client { get; set; }
    }
}