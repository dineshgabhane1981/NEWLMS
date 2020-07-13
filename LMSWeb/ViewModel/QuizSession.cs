using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSWeb.ViewModel
{
    public class QuizSession
    {
        public int QuizId { get; set; }

        public bool isUsed { get; set; }
    }

    public class CourseSession
    {
        public int CourseId { get; set; }
        
    }
}