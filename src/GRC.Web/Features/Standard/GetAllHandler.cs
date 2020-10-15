using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<Standard>>
    {
        private readonly IStandardInterface repository;

        public GetAllHandler(IStandardInterface repository)
        {
            this.repository = repository;
        }

        public async Task<List<Standard>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.ListAllAsync();
        }

        public class Request : IRequest<List<Standard>>
        {
        }
    }
}