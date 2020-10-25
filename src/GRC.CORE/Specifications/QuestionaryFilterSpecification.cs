using Ardalis.Specification;
using GRC.Core.Entities;

namespace GRC.Core.Specifications
{
    public class QuestionaryFilterSpecification : Specification<Questionary>
    {
        public QuestionaryFilterSpecification(string uid, bool IsAdministrator)
        {
            Query.Where(i => i.OwnerUid == uid || IsAdministrator == true)
                 .Include(q => q.Standard);
        }

        public QuestionaryFilterSpecification(string uid, int questionaryId)
        {
            Query.Where(q => q.Id == questionaryId && q.OwnerUid == uid)
                 .Include(q => q.Standard);
        }

        public QuestionaryFilterSpecification(int questionaryId)
        {
            Query.Where(q => q.Id == questionaryId)
                 .Include(q => q.Answers)
                 .ThenInclude(Q => Q.Question)
                 .ThenInclude(Q => Q.Control)
                 .ThenInclude(Q => Q.Domain);
        }
    }
}
