using System.ComponentModel.DataAnnotations;

namespace GRC.Core.Entities
{
    public class DomainResult
    {
        public DomainResult()
        {
        }
        public DomainResult(string fullName, int questionCount)
        {
            FullName = fullName;
            QuestionCount = questionCount;
        }

        public string FullName { get; private set; }

        public double Result { get; set; }

        [DisplayFormat(DataFormatString = "{0:P0}")]
        public double CompletePercenage => QuestionCount == 0 ? 0 : (double)AnswerCount / QuestionCount;

        public int QuestionCount { get; private set; }

        public int AnswerCount { get; set; }

    }
}
