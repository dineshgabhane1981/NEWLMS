using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class tblCurriculumActivity
    {
        public int ActivityId { get; set; }

        public string ActivityText { get; set; }
        public string ActivityType { get; set; }
        public int ActivityOrder { get; set; }
    }
}
