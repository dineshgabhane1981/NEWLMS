using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
   public class tblQuizAssignment
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DueDate { get; set; }
    }
}
