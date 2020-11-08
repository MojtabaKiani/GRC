using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.DomainHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Domains
{
    public class UpdateHandlerTests
    {

        private Mock<IDomainInterface> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int domainId, int returnCount)
        {
            var domain = new Domain { Id = domainId, StandardId = 1, Title = "Test" };
            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.UpdateAsync(It.Is<Domain>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new UpdateHandler.Request(domain);
            var handler = new UpdateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
