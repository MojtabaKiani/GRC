using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Standards
{
    public class CreateHandlerTests
    {

        private Mock<IAsyncRepository<Standard>> _mockRepository;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int standardId)
        {
            var standard = new Standard { Id = standardId, StandardCategoryId = 1};
            _mockRepository = new Mock<IAsyncRepository<Standard>>();
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Standard>())).ReturnsAsync(standard);

            var request = new CreateHandler.Request(standard);
            var handler = new CreateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(standardId, result.Id);
        }
    }
}
