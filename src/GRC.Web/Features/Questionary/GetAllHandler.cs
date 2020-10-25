using GRC.Core.Entities;
using GRC.Core.Interfaces;
using GRC.Core.Specifications;
using GRC.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.QuestionaryHandlers
{
    public class GetAllHandler : IRequestHandler<GetAllHandler.Request, List<Questionary>>
    {
        private readonly IAsyncRepository<Questionary> repository;

        public GetAllHandler(IAsyncRepository<Questionary> repository)
        {
            this.repository = repository;
        }

        public async Task<List<Questionary>> Handle(Request request, CancellationToken cancellationToken)
        {
            var questionaryFilterSpecification = new QuestionaryFilterSpecification(request.UserName, request.IsAdministrator);
            return await repository.ListAsync(questionaryFilterSpecification);
        }

        public class Request : IRequest<List<Questionary>>
        {
            public Request(string userName,bool IsAdministrator)
            {
                UserName = userName;
                this.IsAdministrator = IsAdministrator;
            }

            public string UserName { get; }
            public bool IsAdministrator { get; }
        }
    }
}