using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.Core.Specifications;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionaryHandlers
{
    public class GetByIdFullIncludeHandler : IRequestHandler<GetByIdFullIncludeHandler.Request, Questionary>
    {
        private readonly IAsyncRepository<Questionary> repository;

        public GetByIdFullIncludeHandler(IAsyncRepository<Questionary> repository)
        {
            this.repository = repository;
        }

        public async Task<Questionary> Handle(Request request, CancellationToken cancellationToken)
        {
            var specification = new QuestionaryFilterSpecification(request.Id);
            return await repository.FirstOrDefaultAsync(specification);
        }

        public class Request : IRequest<Questionary>
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}