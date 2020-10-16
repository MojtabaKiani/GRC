using GRC.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IControlInterface : IAsyncRepository<Control>
    {
        Task<List<Control>> ListAllAsync(int domainId);

    }
}
