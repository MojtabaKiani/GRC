using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.ControlHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<Control> repository;

        public DeleteHandler(IAsyncRepository<Control> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.Control);
        }

        public class Request : IRequest<int>
        {
            public Control Control { get; set; }

            public Request(Control sc)
            {
                Control = sc;
            }
        }
    }
}