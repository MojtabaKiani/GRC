using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardHandlers;
using System.Threading;
using System.Collections.Generic;
using GRC.Core.Specifications;
using Ardalis.Specification;

namespace UnitTests.GRCWeb.Features.Standards
{
    public class GetByIdHandlerTests
    {

        private Mock<IStandardInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var standard = new Standard { Id = 1, StandardCategoryId = 1 };

            _mockRepository = new Mock<IStandardInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(standard);

            var request = new GetByIDHandler.Request(id: 1);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            _mockRepository = new Mock<IStandardInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Standard)null);

            var request = new GetByIDHandler.Request(id: 1);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
