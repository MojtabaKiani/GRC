using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.CORE.Specifications;
using GRC.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<Question>>
    {
        private readonly IAsyncRepository<Question> repository;

        public GetAllHandler(IAsyncRepository<Question> repository)
        {
            this.repository = repository;
        }

        public async Task<List<Question>> Handle(Request request, CancellationToken cancellationToken)
        {
            var spec = new QuestionFilterSpecification(request.ControlId);
            return await repository.ListAsync(spec);
        }

        public class Request : IRequest<List<Question>>
        {
            public Request(int controlId)
            {
                ControlId = controlId;
            }
            public int ControlId { get; set; }
        }
    }
}