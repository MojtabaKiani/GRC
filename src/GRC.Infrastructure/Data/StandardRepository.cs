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
    public class StandardRepository : BaseRepository<Standard>,IStandardInterface
    {
        private readonly GRCContext context;

        public StandardRepository(GRCContext context) : base(context) => this.context = context;

        public override async Task<List<Standard>> ListAllAsync()
        {
            return await _dbContext.Standards.Include(x=> x.StandardCategory).ToListAsync();
        }

        public override async Task<Standard> GetByIdAsync(int Id)
        {
            return await _dbContext.Standards.Include(x => x.StandardCategory).Include(x => x.Domains).SingleAsync(q => q.Id == Id);
        }
    }
}
