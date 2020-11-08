using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardCategoryHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.StandardCategories
{
    public class CreateHandlerTests
    {

        private Mock<IAsyncRepository<StandardCategory>> _mockRepository;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int standardCategoryId)
        {
            var standardCategory = new StandardCategory { Id = standardCategoryId};
            _mockRepository = new Mock<IAsyncRepository<StandardCategory>>();
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<StandardCategory>())).ReturnsAsync(standardCategory);

            var request = new CreateHandler.Request(standardCategory);
            var handler = new CreateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(standardCategoryId, result.Id);
        }
    }
}
