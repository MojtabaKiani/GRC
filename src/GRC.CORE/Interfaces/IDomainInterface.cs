using GRC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Core.Interfaces
{
    public interface IDomainInterface : IAsyncRepository<Domain>
    {
        Task<List<Domain>> ListAllAsync(int StandardId);

        Task<List<Tuple<string, int>>> GetDomainWithQuestionCount(int standardId);
    }
}
