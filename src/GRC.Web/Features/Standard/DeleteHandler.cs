using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<Standard> repository;

        public DeleteHandler(IAsyncRepository<Standard> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.Standard);
        }

        public class Request : IRequest<int>
        {
            public Standard Standard { get; set; }

            public Request(Standard sc)
            {
                Standard = sc;
            }
        }
    }
}