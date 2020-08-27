using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSWeb.ViewModel
{
    public class CurriculumActivities
    {
        public string CurriculumName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityText { get; set; }
        public string ActivityType { get; set; }
        public string DueDate { get; set; }
        
        public string ActivityStatus { get; set; }
        public string CompletionDate { get; set; }
        public string Duration { get; set; }
        public string Score { get; set; }
        public string CourseSubType { get; set; }
        public string CourseModuleURL { get; set; }

    }
}