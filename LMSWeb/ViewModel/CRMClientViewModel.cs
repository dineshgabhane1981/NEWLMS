using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMSBL.DBModels.CRMNew;
using LMSBL.DBModels;

namespace LMSWeb.ViewModel
{
    public class CRMClientViewModel
    {
        public List<tblCRMClientSubStage> lstClientSubStages { get; set; }
        public List<ClientTicket> objClientTicketLST { get; set; }
    }
   
}