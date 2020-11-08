﻿using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.QuestionHandlers;
using System.Threading;

namespace UnitTests.GRCWeb.Features.Questions
{
    public class DeleteHandlerTests
    {

        private Mock<IAsyncRepository<Question>> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int questionId, int returnCount)
        {
            var question = new Question { Id = questionId, ControlId = 1, Text = "Test" };
            _mockRepository = new Mock<IAsyncRepository<Question>>();
            _mockRepository.Setup(x => x.DeleteAsync(It.Is<Question>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new DeleteHandler.Request(question);
            var handler = new DeleteHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
