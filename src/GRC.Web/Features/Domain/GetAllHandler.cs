using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.DomainHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<Domain>>
    {
        private readonly IDomainInterface repository;

        public GetAllHandler(IDomainInterface repository)
        {
            this.repository = repository;
        }

        public async Task<List<Domain>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.ListAllAsync(request.StandardId);
        }

        public class Request : IRequest<List<Domain>>
        {
            public Request(int standardId)
            {
                StandardId = standardId;
            }
            public int StandardId { get; set; }
        }
    }
}