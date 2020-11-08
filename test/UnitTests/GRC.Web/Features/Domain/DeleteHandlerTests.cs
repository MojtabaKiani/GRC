using Moq;
using Xunit;
using System.Threading;
using GRC.Core.Entities;
using GRC.Core.Interfaces;
using System.Threading.Tasks;
using GRC.Web.Features.DomainHandlers;

namespace UnitTests.GRCWeb.Features.Domains
{
    public class DeleteHandlerTests
    {

        private Mock<IAsyncRepository<Domain>> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int domainId, int returnCount)
        {
            var domain = new Domain { Id = domainId, StandardId = 1, Title = "Test" };
            _mockRepository = new Mock<IAsyncRepository<Domain>>();
            _mockRepository.Setup(x => x.DeleteAsync(It.Is<Domain>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new DeleteHandler.Request(domain);
            var handler = new DeleteHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
