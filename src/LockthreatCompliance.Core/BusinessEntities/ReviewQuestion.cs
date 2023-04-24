using Abp.Domain.Entities;
using LockthreatCompliance.Questions;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("ReviewQuestions")]
    public class ReviewQuestion : Entity
    {
        public ReviewQuestion()
        {

        }

        public ReviewQuestion(int questionId, string comment, int? selectedAnswerOptionId)
        {
            QuestionId = questionId;
            SelectedAnswerOptionId = selectedAnswerOptionId;
            Comment = comment;
            Attachments = new List<DocumentPath>();
        }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public string Comment { get; set; }
        public int? SelectedAnswerOptionId { get; set; }

    }
}
