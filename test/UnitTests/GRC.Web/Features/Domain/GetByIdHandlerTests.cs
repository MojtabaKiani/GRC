using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.DomainHandlers;
using System.Threading;
using GRC.Core.Specifications;

namespace UnitTests.GRCWeb.Features.Domains
{
    public class GetByIdHandlerTests
    {

        private Mock<IDomainInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var domain = new Domain { Id = 1, StandardId = 1, Title = "Test" };

            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(domain);

            var request = new GetByIdHandler.Request(id: 1);
            var handler = new GetByIdHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Title);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Domain)null);

            var request = new GetByIdHandler.Request(id: 1);
            var handler = new GetByIdHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
