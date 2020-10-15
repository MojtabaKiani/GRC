using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Entities
{
    public class Questionary : BaseEntity
    {
        public Questionary()
        {
            Answers = new List<Answer>();
        }

        [Required]
        public string OwnerUid { get; set; }

        [Required]
        public int StandardId { get; set; }

        public Standard Standard { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Name length must be less than 100")]
        public string Name { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Compliance Level must be between 1 and 10")]
        public int ComplianceLevel { get; set; }

        public double CalculatedCompliance { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [NotMapped]
        public int CompletePercentage { get { return Answers.Count() / Standard.Domains.SelectMany(q => q.Controls).SelectMany(q => q.Questions).Count(); } }

        public List<Answer> Answers { get; set; }

        public void AddAnswer(Answer answer)
        {
            Guard.Against.Null<Answer>(answer, nameof(answer));
            Answers.Add(answer);
        }
    }
}
