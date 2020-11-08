using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.QuestionaryHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Questionaries
{
    public class CreateHandlerTests
    {

        private Mock<IAsyncRepository<Questionary>> _mockRepository;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int questionaryId)
        {
            var questionary = new Questionary { Id = questionaryId, StandardId = 1};
            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Questionary>())).ReturnsAsync(questionary);

            var request = new CreateHandler.Request(questionary);
            var handler = new CreateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(questionaryId, result.Id);
        }
    }
}
