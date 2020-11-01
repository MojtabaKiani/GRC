using GRC.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Entities
{
    public class QuestionAnswer :BaseEntity
    {
        [Required]
        [Display(Name = "Answer", Prompt = "Answer Text")]
        public string AnswerText { get; set; }

        [Display(Name = "Value", Prompt = "A decimal between 0 and 1")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Range(0, 1)]
        [Required]
        public double Value { get; set; }

        [Display(Name = "Correct Answer")]
        public bool IsCorrectAnswer { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

    }
}
