using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IAsyncRepository<Standard> repository;

        public UpdateHandler(IAsyncRepository<Standard> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.Standard);
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