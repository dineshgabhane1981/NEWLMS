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


    }
}