using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Entities
{
    public class Answer : BaseEntity
    {
        [Required]
        public int QuestionaryId { get; set; }

        public Questionary Questionary { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public Question Question { get; set; }

        [Required]
        [Range(0,1)]
        public double AnswerValue { get; set; }

        public string AnswerText { get; set; }

        [Required]
        public double Point { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

    }
}
