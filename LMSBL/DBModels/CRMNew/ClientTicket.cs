using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class ClientTicket
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string VisaIntrested { get; set; }
        public int? CurrentSubStage { get; set; }
        public string ContactNo { get; set; }
}
}
