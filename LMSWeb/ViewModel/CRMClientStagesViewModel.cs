using LMSBL.DBModels.CRMNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSWeb.ViewModel
{
    public class CRMClientStagesViewModel
    {
        public tblCRMClientStage objtblCRMClientStage { get; set; }

        public tblCRMClientSubStage objtblCRMClientSubStage { get; set; }
    }
}