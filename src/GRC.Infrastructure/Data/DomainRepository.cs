using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Infrastructure.Data
{
    public class DomainRepository : BaseRepository<Domain>, IDomainInterface
    {
        private readonly GRCContext context;

        public DomainRepository(GRCContext context) : base(context) => this.context = context;

        public async Task<List<Domain>> ListAllAsync(int StandardId) => await _dbContext.Domains.Where(q => q.StandardId == StandardId).ToListAsync();

        public async override Task<Domain> GetByIdAsync(int id) => await _dbContext.Domains.Include(x => x.Standard)
                                                                                           .Include(x => x.Controls)
                                                                                           .SingleAsync(q => q.Id == id);

        public async Task<List<Tuple<string, int>>> GetDomainWithQuestionCount(int standardId)
        {
            return await _dbContext.Domains.Include(q => q.Controls)
                                             .ThenInclude(q => q.Questions)
                                             .Where(q => q.StandardId == standardId)
                                             .Select(q => new Tuple<string, int>($"{q.Code}) {q.Title}", q.Controls.SelectMany(c => c.Questions).Count()))
                                             .ToListAsync();
        }
    }
}
