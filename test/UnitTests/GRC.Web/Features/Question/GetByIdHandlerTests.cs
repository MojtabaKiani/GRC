using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.QuestionHandlers;
using System.Threading;
using System.Collections.Generic;
using GRC.Core.Specifications;
using Ardalis.Specification;

namespace UnitTests.GRCWeb.Features.Questions
{
    public class GetByIdHandlerTests
    {

        private Mock<IAsyncRepository<Question>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var question = new Question { Id = 1, ControlId = 1, Text = "Test" };
            var spec = new QuestionFilterSpecification(controlId: 1, questionId: 1);

            _mockRepository = new Mock<IAsyncRepository<Question>>();
            _mockRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<QuestionFilterSpecification>())).ReturnsAsync(question);

            var request = new GetByIdHandler.Request(controlId: 1, id: 1);
            var handler = new GetByIdHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Text);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            var spec = new QuestionFilterSpecification(controlId: 1, questionId: 1);

            _mockRepository = new Mock<IAsyncRepository<Question>>();
            _mockRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<QuestionFilterSpecification>())).ReturnsAsync((Question)null);

            var request = new GetByIdHandler.Request(controlId: 1, id: 1);
            var handler = new GetByIdHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
