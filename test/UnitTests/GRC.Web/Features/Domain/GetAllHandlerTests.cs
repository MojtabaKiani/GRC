using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.DomainHandlers;
using System.Threading;
using System.Collections.Generic;

namespace UnitTests.GRCWeb.Features.Domains
{
    public class GetAllHandlerTests
    {

        private Mock<IDomainInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_DomainId_Exists()
        {

            int standardId = 3;
            var domainList = new List<Domain>
            {
                 new Domain { Id = 1, StandardId = standardId },
                 new Domain { Id = 2, StandardId = standardId }
            };

            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.ListAllAsync(standardId)).ReturnsAsync(domainList);

            var request = new GetAllHandler.Request(standardId);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_DomainId_Not_Exists()
        {
            int standardId = 3;
            var domainList = new List<Domain>
            {
                 new Domain { Id = 1, StandardId = standardId },
                 new Domain { Id = 2, StandardId = standardId }
            };

            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.ListAllAsync(standardId)).ReturnsAsync(domainList);

            var request = new GetAllHandler.Request(1);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
