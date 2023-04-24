using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Questions
{
    public static class ExtensionsForQuestions
    {
        public static List<AnswerOption> GetAnswerOptions(this string str)
        {
            return str.Split(",").ToList()
                .Select(e => new AnswerOption
                {
                    Value = e.GetAnswerOptionValue(),
                    Score = e.GetAnswerOptionScore()
                }).ToList();
        }

        public static List<ExternalQuestionAnswerOption> GetExternalAnswerOptions(this string str)
        {
            return str.Split(",").ToList()
                .Select(e => new ExternalQuestionAnswerOption
                {
                    Value = e.GetAnswerOptionValue(),
                    Score = e.GetAnswerOptionScore()
                }).ToList();
        }


        public static string GetAnswerOptionValue(this string str)
        {
            return str.Substring(0, str.IndexOf('-'));
        }
        public static double GetAnswerOptionScore(this string str)
        {
            var separatorIndex = str.IndexOf('-');
            return double.Parse(str.Substring(separatorIndex + 1, str.Length - separatorIndex - 1));
        }
    }
}
