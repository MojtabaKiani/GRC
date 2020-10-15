using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardCategoryHandlers
{
    public class CreateHandler : IRequestHandler<CreateHandler.Request, StandardCategory>
    {
        private readonly IAsyncRepository<StandardCategory> repository;

        public CreateHandler(IAsyncRepository<StandardCategory> repository)
        {
            this.repository = repository;
        }

        public async Task<StandardCategory> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(request.StandardCategory);
        }

        public class Request : IRequest<StandardCategory>
        {
            public StandardCategory StandardCategory { get; set; }

            public Request(StandardCategory  sc)
            {
                StandardCategory = sc;
            }
        }
    }
}