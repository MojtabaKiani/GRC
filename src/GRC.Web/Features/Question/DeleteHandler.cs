using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<Question> repository;

        public DeleteHandler(IAsyncRepository<Question> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.Question);
        }

        public class Request : IRequest<int>
        {
            public Question Question { get; set; }

            public Request(Question sc)
            {
                Question = sc;
            }
        }
    }
}