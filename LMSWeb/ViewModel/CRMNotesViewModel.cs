﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSBL.DBModels.CRMNew;

namespace LMSWeb.ViewModel
{
    public class CRMNotesViewModel
    {
        public tblCRMNote objCRMnotes { get; set;}

        public List<tblCRMNote> lstNotes { get; set; }

        public List<SelectListItem> lstCRMclient { get; set; }
    }
}