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
    public class GetAllHandlerTests
    {

        private Mock<IAsyncRepository<Question>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_QuestionId_Exists()
        {

            int controlId = 3;
            var questionList = new List<Question>
            {
                 new Question { Id = 1, ControlId = controlId, Text = "Test" },
                 new Question { Id = 2, ControlId = controlId, Text = "Test" }
            };
            var spec = new QuestionFilterSpecification(controlId);

            _mockRepository = new Mock<IAsyncRepository<Question>>();
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<QuestionFilterSpecification>())).ReturnsAsync(questionList);

            var request = new GetAllHandler.Request(controlId);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_QuestionId_Not_Exists()
        {
            int controlId = 3;
            var questionList = new List<Question>
            {
                 new Question { Id = 1, ControlId = controlId, Text = "Test" },
                 new Question { Id = 2, ControlId = controlId, Text = "Test" }
            };
            _mockRepository = new Mock<IAsyncRepository<Question>>();
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<QuestionFilterSpecification>())).ReturnsAsync((List<Question>)null);

            var request = new GetAllHandler.Request(controlId);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
