using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.QuestionaryHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Questionaries
{
    public class DeleteHandlerTests
    {

        private Mock<IAsyncRepository<Questionary>> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int questionaryId, int returnCount)
        {
            var questionary = new Questionary { Id = questionaryId, StandardId = 1};
            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.DeleteAsync(It.Is<Questionary>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new DeleteHandler.Request(questionary);
            var handler = new DeleteHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
