using GRC.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IDomainInterface : IAsyncRepository<Domain>
    {
        Task<List<Domain>> ListAllAsync(int StandardId);

    }
}
