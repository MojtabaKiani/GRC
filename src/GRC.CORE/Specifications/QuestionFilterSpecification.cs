using Ardalis.Specification;
using GRC.Core.Entities;
using System;


namespace GRC.Core.Specifications
{

    public class QuestionFilterSpecification : Specification<Question>
    {
        public QuestionFilterSpecification(int controlId)
        {
            Query.Where(q => q.ControlId == controlId);
        }

        public QuestionFilterSpecification(int controlId, int questionId)
        {
            Query.Where(q => q.Id == questionId && q.ControlId == controlId).Include(q => q.QuestionAnswers);
        }
    }

}