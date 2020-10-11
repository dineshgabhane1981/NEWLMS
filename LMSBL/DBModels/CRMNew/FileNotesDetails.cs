using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels.CRMNew
{
    public class FileNotesDetails
    {
        public int ClientId { get; set; }
        public string Notes { get; set; }
        public int SubStage { get; set; }
        public string SubStageName { get; set; }
        public string CommunicationSource { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
