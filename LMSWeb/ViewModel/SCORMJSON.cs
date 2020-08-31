using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSWeb.ViewModel
{
    public class SCORMJSON
    {
        public ConfigData config { get; set; }
        public QuizData quiz { get; set; }
        public ReviewQuizData reviewQuiz { get; set; }
        public List<QuestionsData> questions { get; set; }
    }

    public class ConfigData
    {
        public double scormType { get; set; }        
    }

    public class QuizData
    {
        public string title { get; set; }
        public string nameLabel { get; set; }
        public string name { get; set; }
        public string descLabel { get; set; }
        public string description { get; set; }
        public string timeLabel { get; set; }
        public int? duration { get; set; }
        public int passingScore { get; set; }
        public int minScore { get; set; }
        public int maxScore { get; set; }
        public Boolean multipleAttempts { get; set; }
        public string timeoutMessage { get; set; }
    }
    public class ReviewQuizData
    {
        public string title { get; set; }
        public string nameLabel { get; set; }
        public string name { get; set; }
        public string descLabel { get; set; }
        public string description { get; set; }
        public string scoreLabel { get; set; }
         
    }
    public class QuestionsData
    {
        public string type { get; set; }
        public string questionText { get; set; }
        public string instructionText { get; set; }
        public Boolean randomOptions { get; set; }
        public int[] answer { get; set; }
        public int points { get; set; }
        public string placeholderText { get; set; }
        public string path { get; set; }

        public List<OptionsData> options { get; set; }
        public CorrectFeedbackData correctFeedback { get; set; }
        public IncorrectFeedbackData incorrectFeedback { get; set; }
    }
    public class OptionsData
    {
        public string text { get; set; }
        public string feedback { get; set; }
    }
    public class CorrectFeedbackData
    {
        public string text { get; set; }
    }
    public class IncorrectFeedbackData
    {
        public string text { get; set; }
    }
}