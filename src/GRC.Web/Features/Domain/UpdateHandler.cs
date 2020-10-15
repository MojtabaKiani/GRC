using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.DomainHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IAsyncRepository<Domain> repository;

        public UpdateHandler(IAsyncRepository<Domain> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.Domain);
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