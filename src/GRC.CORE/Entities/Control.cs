using Ardalis.GuardClauses;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace GRC.Core.Entities
{
    public class Control : BaseEntity
    {
        public Control()
        {
            Questions = new List<Question>();
        }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
      
        [Required]
        [Range(1,10,ErrorMessage = "Level must be between 1 and 10")]
        public int Level { get; set; }
     
        [Required]
        public int DomainId { get; set; }

        public Domain Domain { get; set; }

        [MaxLength(10,ErrorMessage = "Code length must be less than 10")]
        public string Code { get; set; }
     
        [DisplayName("Control Text")]
        [NotMapped]
        public string FullText => (Code == "") ? Text : $"{Code}) {Text}";

        public List<Question> Questions { get; set; }

        public void AddQuestion(Question question)
        {
            Guard.Against.Null<Question>(question, nameof(question));
            Questions.Add(question);
        }

    }
}