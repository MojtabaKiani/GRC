using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Entities
{
    public class Questionary : BaseEntity
    {
        public Questionary()
        {
            Answers = new List<Answer>();
            DomainResults = new List<DomainResult>();
        }

        public Questionary(string uid):this()
        {
            OwnerUid = uid;
            CreateDate = DateTime.Now;
        }

        [DisplayName("Creator")]
        public string OwnerUid { get; private set; }

        [DisplayName("Standard")]
        [Required]
        public int StandardId { get; set; }

        [DisplayName("Standard")]
        public Standard Standard { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Name length must be less than 100")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Create Date")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Required]
        [DisplayName("Compliance Level")]
        [Range(1, 10, ErrorMessage = "Compliance Level must be between 1 and 10")]
        public int ComplianceLevel { get; set; }

        [DisplayName("Result")]
        public double CalculatedCompliance { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public List<Answer> Answers { get; set; }

        [NotMapped]
        public List<DomainResult> DomainResults { get; set; }

        [DisplayFormat(DataFormatString = "{0:P0}")]
        public double CompletePercentage => DomainResults.Average(q => q.CompletePercenage);

        public void AddAnswer(Answer answer)
        {
            Guard.Against.Null<Answer>(answer, nameof(answer));
            Answers.Add(answer);
        }

        public void AddDomainResultsWithAnswer(string DomainName, double AnswerPoint)
        {
            Guard.Against.NullOrEmpty(DomainName, nameof(DomainName));
            var DomainResult = DomainResults.SingleOrDefault(q => q.FullName == DomainName);
            DomainResult.Result += AnswerPoint;
            DomainResult.AnswerCount++;
        }

        public void AddDomainResults(string DomainName, int QuestionCount)
        {
            Guard.Against.NullOrEmpty(DomainName,nameof(DomainName));
            DomainResults.Add(new DomainResult(DomainName, QuestionCount));
        }

        public void CalculateResult(List<Tuple<string, int>> domains)
        {
            if (domains == null || domains?.Count==0) return;
            foreach (var Domain in domains)
            {
                AddDomainResults(Domain.Item1, Domain.Item2);
            }
           foreach(var answer in Answers)
            {
                AddDomainResultsWithAnswer(answer.Question.Control.Domain.FullText, answer.Point);
            }
        }

         
    }
}
