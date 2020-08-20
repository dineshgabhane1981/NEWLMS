using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSBL.DBModels
{
    public class EnquiryResult
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }
        public string AssessmentLink { get; set; }

        public int NoOfQuestions { get; set; }
    }
}
