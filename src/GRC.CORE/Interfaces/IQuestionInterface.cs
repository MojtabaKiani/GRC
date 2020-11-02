using GRC.Core.Entities;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IQuestionInterface : IAsyncRepository<Standard>
    {
        Task<int> UpdateAsync(Question question);
    }


}
