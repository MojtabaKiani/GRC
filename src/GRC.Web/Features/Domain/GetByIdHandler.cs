using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.DomainHandlers
{
    public class GetByIDHandler : IRequestHandler<GetByIDHandler.Request, Domain>
    {
        private readonly IDomainInterface repository;

        public GetByIDHandler(IDomainInterface repository)
        {
            this.repository = repository;
        }

        public async Task<Domain> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(request.Id);
        }

        public class Request : IRequest<Domain>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}