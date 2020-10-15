using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardCategoryHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<StandardCategory>>
    {
        private readonly IAsyncRepository<StandardCategory> repository;

        public GetAllHandler(IAsyncRepository<StandardCategory> repository)
        {
            this.repository = repository;
        }

        public async Task<List<StandardCategory>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.ListAllAsync();
        }

        public class Request : IRequest<List<StandardCategory>>
        {
        }
    }
}