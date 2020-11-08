using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardCategoryHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.StandardCategories
{
    public class GetByIdHandlerTests
    {

        private Mock<IAsyncRepository<StandardCategory>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var standardCategory = new StandardCategory { Id = 1 };

            _mockRepository = new Mock<IAsyncRepository<StandardCategory>>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(standardCategory);

            var request = new GetByIDHandler.Request(id: 1);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            _mockRepository = new Mock<IAsyncRepository<StandardCategory>>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((StandardCategory)null);

            var request = new GetByIDHandler.Request(id: 1);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
