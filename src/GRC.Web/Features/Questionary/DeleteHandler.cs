using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionaryHandlers
{
    public class DeleteHandler : IRequestHandler<DeleteHandler.Request, int>
    {
        private readonly IAsyncRepository<Questionary> repository;

        public DeleteHandler(IAsyncRepository<Questionary> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.DeleteAsync(request.Questionary);
        }

        public class Request : IRequest<int>
        {
            public Questionary Questionary { get; set; }

            public Request(Questionary sc)
            {
                Questionary = sc;
            }
        }
    }
}