using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.ControlHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<Control>>
    {
        private readonly IControlInterface repository;

        public GetAllHandler(IControlInterface repository)
        {
            this.repository = repository;
        }

        public async Task<List<Control>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.ListAllAsync(request.DoaminId);
        }

        public class Request : IRequest<List<Control>>
        {
            public Request(int doaminId)
            {
                DoaminId = doaminId;
            }
            public int DoaminId { get; set; }
        }
    }
}