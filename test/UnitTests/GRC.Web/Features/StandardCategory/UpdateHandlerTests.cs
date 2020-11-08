using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardCategoryHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.StandardCategorys
{
    public class UpdateHandlerTests
    {

        private Mock<IAsyncRepository<StandardCategory>> _mockRepository;

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int standardCategoryId, int returnCount)
        {
            var standardCategory = new StandardCategory { Id = standardCategoryId };
            _mockRepository = new Mock<IAsyncRepository<StandardCategory>>();
            _mockRepository.Setup(x => x.UpdateAsync(It.Is<StandardCategory>(x => x.Id == 1))).ReturnsAsync(1);

            var request = new UpdateHandler.Request(standardCategory);
            var handler = new UpdateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
