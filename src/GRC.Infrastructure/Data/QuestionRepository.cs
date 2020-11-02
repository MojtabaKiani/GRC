using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Infrastructure.Data
{
    public class QuestionRepository : BaseRepository<Standard>, IQuestionInterface
    {
        private readonly GRCContext context;

        public QuestionRepository(GRCContext context) : base(context) => this.context = context;

        public async Task<int> UpdateAsync(Question question)
        {
            context.Entry(question).State = EntityState.Modified;
            foreach (var questionAnswer in question.QuestionAnswers)
                context.Entry(questionAnswer).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}
