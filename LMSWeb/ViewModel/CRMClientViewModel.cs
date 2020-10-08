using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMSBL.DBModels.CRMNew;

namespace LMSWeb.ViewModel
{
    public class CRMClientViewModel
    {
        public List<tblCRMClientSubStage> lstClientSubStages { get; set; }
        public List<tblCRMUser> objCRMUserLST { get; set; }
    }
}