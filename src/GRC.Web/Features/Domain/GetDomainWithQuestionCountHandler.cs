using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Web.Features.DomainHandlers
{
    public class GetDomainWithQuestionCountHandler : IRequestHandler<GetDomainWithQuestionCountHandler.Request, List<Tuple<string, int>>>
    {
        private readonly IDomainInterface repository;

        public GetDomainWithQuestionCountHandler(IDomainInterface repository)
        {
            this.repository = repository;
        }

        public async Task<List<Tuple<string,int>>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await repository.GetDomainWithQuestionCount(request.StandardId);
        }

        public class Request : IRequest<List<Tuple<string, int>>>
        {
            public Request(int standardId)
            {
                StandardId = standardId;
            }

            public int StandardId { get; }
        }
    }
}