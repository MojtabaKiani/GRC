using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardHandlers
{
    public class GetByIDHandler : IRequestHandler<GetByIDHandler.Request, Standard>
    {
        private readonly IStandardInterface repository;

        public GetByIDHandler(IStandardInterface repository)
        {
            this.repository = repository;
        }

        public async Task<Standard> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(request.Id);
        }

        public class Request : IRequest<Standard>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}