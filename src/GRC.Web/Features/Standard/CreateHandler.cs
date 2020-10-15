using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardHandlers
{
    public class CreateHandler : IRequestHandler<CreateHandler.Request, Standard>
    {
        private readonly IAsyncRepository<Standard> repository;

        public CreateHandler(IAsyncRepository<Standard> repository)
        {
            this.repository = repository;
        }

        public async Task<Standard> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(request.Standard);
        }

        public class Request : IRequest<Standard>
        {
            public Standard Standard { get; set; }

            public Request(Standard  sc)
            {
                Standard = sc;
            }
        }
    }
}