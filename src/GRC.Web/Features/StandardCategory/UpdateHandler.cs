using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.StandardCategoryHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IAsyncRepository<StandardCategory> repository;

        public UpdateHandler(IAsyncRepository<StandardCategory> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.StandardCategory);
        }

        public class Request : IRequest<int>
        {
            public StandardCategory StandardCategory { get; set; }

            public Request(StandardCategory sc)
            {
                StandardCategory = sc;
            }
        }
    }
}