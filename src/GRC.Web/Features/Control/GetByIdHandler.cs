using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.ControlHandlers
{
    public class GetByIDHandler : IRequestHandler<GetByIDHandler.Request, Control>
    {
        private readonly IControlInterface repository;

        public GetByIDHandler(IControlInterface repository)
        {
            this.repository = repository;
        }

        public async Task<Control> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(request.Id);
        }

        public class Request : IRequest<Control>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}