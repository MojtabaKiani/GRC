using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionaryHandlers
{
    public class CreateHandler : IRequestHandler<CreateHandler.Request, Questionary>
    {
        private readonly IAsyncRepository<Questionary> repository;

        public CreateHandler(IAsyncRepository<Questionary> repository)
        {
            this.repository = repository;
        }

        public async Task<Questionary> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(request.Questionary);
        }

        public class Request : IRequest<Questionary>
        {
            public Questionary Questionary { get; set; }

            public Request(Questionary  sc)
            {
                Questionary = sc;
            }
        }
    }
}