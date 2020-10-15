using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.DomainHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<Domain> repository;

        public DeleteHandler(IAsyncRepository<Domain> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.Domain);
        }

        public class Request : IRequest<int>
        {
            public Domain Domain { get; set; }

            public Request(Domain sc)
            {
                Domain = sc;
            }
        }
    }
}