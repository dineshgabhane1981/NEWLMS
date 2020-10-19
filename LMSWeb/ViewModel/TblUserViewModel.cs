using LMSBL.DBModels;
using LMSBL.DBModels.CRMNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSWeb.ViewModel
{
    public class TblUserViewModel
    {
        public TblUser objtbluser { get; set; }

        public tblCRMClient objtblCRMClient { get; set; }

        public List<TblUser> objlsttbluser { get; set; }

        public tblCRMClientStage objtblCRMClientStage { get; set; }

        public tblCRMClientSubStage objtblCRMClientSubStage { get; set; }

        public List<tblCRMClientStage> lstCRMClientStage { get; set; }
        public List<tblCRMClientSubStage> lstCRMClientSubStage { get; set; }

    }
}