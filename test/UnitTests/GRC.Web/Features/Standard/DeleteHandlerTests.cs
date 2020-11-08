using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Standards
{
    public class DeleteHandlerTests
    {

        private Mock<IAsyncRepository<Standard>> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int standardId, int returnCount)
        {
            var standard = new Standard { Id = standardId, StandardCategoryId = 1};
            _mockRepository = new Mock<IAsyncRepository<Standard>>();
            _mockRepository.Setup(x => x.DeleteAsync(It.Is<Standard>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new DeleteHandler.Request(standard);
            var handler = new DeleteHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
