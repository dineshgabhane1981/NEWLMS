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
        public tblCRMUser ObjCRMUser { get; set; }
        public tblCRMClient ObjCRMClient { get; set; }
        public List<tblCRMInvoice> ObjCRMInvoivceLST { get; set; }
        public List<tblCRMInvoiceItem> ObjCRMInvoiceItemLST { get; set; }
        public List<SelectListItem> lstCRMclient { get; set; }
        public List<SelectListItem> lstCRMCurriencies { get; set; }

        public string JsonData { get; set; }
        public int Client { get; set; }
    }
}