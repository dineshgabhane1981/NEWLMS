using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
   public class tblLearnerActivityAssignment
    {
        public int LearnerId { get; set; }
        public string UserName { get; set; }
        public string DueDate { get; set; }
    }
}
