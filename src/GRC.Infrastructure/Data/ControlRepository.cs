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
    public class ControlRepository : BaseRepository<Control>,IControlInterface
    {
        private readonly GRCContext context;

        public ControlRepository(GRCContext context) : base(context) => this.context = context;

        public async Task<List<Control>> ListAllAsync(int domainId)
        {
            return await _dbContext.Controls.Where(q => q.DomainId == domainId).ToListAsync();
        }

        public async override  Task<Control> GetByIdAsync(int id)
        {
            return await _dbContext.Controls.Include(x => x.Domain).SingleAsync(q => q.Id == id);
        }

    }
}
