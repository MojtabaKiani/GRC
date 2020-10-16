using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.ControlHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IAsyncRepository<Control> repository;

        public UpdateHandler(IAsyncRepository<Control> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.Control);
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