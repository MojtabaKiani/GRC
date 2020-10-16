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
    public class DomainRepository : BaseRepository<Domain>, IDomainInterface
    {
        private readonly GRCContext context;

        public DomainRepository(GRCContext context) : base(context) => this.context = context;

        public async Task<List<Domain>> ListAllAsync(int StandardId)
        {
            return await _dbContext.Domains.Where(q => q.StandardId == StandardId).ToListAsync();
        }

        public async override Task<Domain> GetByIdAsync(int id)
        {
            return await _dbContext.Domains.Include(x => x.Standard).Include(x => x.Controls).SingleAsync(q => q.Id == id);
        }
    }
}
