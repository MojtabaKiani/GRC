using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IQuestionInterface repository;

        public UpdateHandler(IQuestionInterface repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.Question);
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