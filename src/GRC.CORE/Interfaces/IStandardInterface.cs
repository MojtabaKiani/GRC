using GRC.Core.Entities;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IStandardInterface : IAsyncRepository<Standard>
    {
        Task<Standard> GetByIdFullInclude(int Id);
    }


}
