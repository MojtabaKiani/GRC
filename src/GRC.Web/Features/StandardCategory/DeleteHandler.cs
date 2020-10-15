using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardCategoryHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<StandardCategory> repository;

        public DeleteHandler(IAsyncRepository<StandardCategory> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.StandardCategory);
        }

        public class Request : IRequest<int>
        {
            public StandardCategory StandardCategory { get; set; }

            public Request(StandardCategory sc)
            {
                StandardCategory = sc;
            }
        }
    }
}