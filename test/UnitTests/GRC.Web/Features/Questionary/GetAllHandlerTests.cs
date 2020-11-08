using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.QuestionaryHandlers;
using System.Threading;
using System.Collections.Generic;
using GRC.Core.Specifications;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnitTests.GRCWeb.Features.Questionaries
{
    public class GetAllHandlerTests
    {

        private Mock<IAsyncRepository<Questionary>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_QuestionaryId_Exists()
        {

            int standardId = 3;
            var questionaryList = new List<Questionary>
            {
                 new Questionary { Id = 1, StandardId = standardId },
                 new Questionary { Id = 2, StandardId = standardId }
            };
            var questionaryFilterSpecification = new QuestionaryFilterSpecification(uid: "Test", IsAdministrator: false);

            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<QuestionaryFilterSpecification>())).ReturnsAsync(questionaryList);

            var request = new GetAllHandler.Request(userName: "Test", IsAdministrator: false);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_QuestionaryId_Not_Exists()
        {
            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<QuestionaryFilterSpecification>())).ReturnsAsync((List<Questionary>)null);

            var request = new GetAllHandler.Request(userName: "Test", IsAdministrator: false);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
