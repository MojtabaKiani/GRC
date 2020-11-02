using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.CORE.Specifications;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionHandlers
{
    public class GetByIdHandler : IRequestHandler<GetByIdHandler.Request, Question>
    {
        private readonly IAsyncRepository<Question> repository;

        public GetByIdHandler(IAsyncRepository<Question> repository)
        {
            this.repository = repository;
        }

        public async Task<Question> Handle(Request request, CancellationToken cancellationToken)
        {
            var spec = new QuestionFilterSpecification(request.ControlId, request.Id);
            return await repository.FirstOrDefaultAsync(spec);
        }

        public class Request : IRequest<Question>
        {
            public Request(int controlId, int id)
            {
                ControlId = controlId;
                Id = id;
            }

            public int ControlId { get; }
            public int Id { get; private set; }
        }
    }
}