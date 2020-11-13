using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Entities
{

    public class Question : BaseEntity
    {
        public Question()
        {
            Answers = new List<Answer>();
            QuestionAnswers = new List<QuestionAnswer>();
        }

        public Question(int answerCount):this()
        {
            for (var i = 0; i < answerCount; i++)
                QuestionAnswers.Add(new QuestionAnswer());
        }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Question weight must be between 1 and 10")]
        public int Weight { get; set; }

        [MaxLength(200)]
        public string Group { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public int ControlId { get; set; }

        public Control Control { get; set; }

        public List<Answer> Answers { get; set; }

        public List<QuestionAnswer> QuestionAnswers { get; set; }

    }
}
