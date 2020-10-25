using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionaryHandlers
{
    public class UpdateHandler : IRequestHandler<UpdateHandler.Request, int>
    {
        private readonly IAsyncRepository<Questionary> repository;

        public UpdateHandler(IAsyncRepository<Questionary> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(request.Questionary);
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