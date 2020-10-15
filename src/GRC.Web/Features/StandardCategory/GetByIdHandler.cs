using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardCategoryHandlers
{
    public class GetByIDHandler : IRequestHandler<GetByIDHandler.Request, StandardCategory>
    {
        private readonly IAsyncRepository<StandardCategory> repository;

        public GetByIDHandler(IAsyncRepository<StandardCategory> repository)
        {
            this.repository = repository;
        }

        public async Task<StandardCategory> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(request.Id);
        }

        public class Request : IRequest<StandardCategory>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}