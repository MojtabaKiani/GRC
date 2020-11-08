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

namespace UnitTests.GRCWeb.Features.Questionarys
{
    public class GetByIdFullIncludeHandlerTests
    {

        private Mock<IAsyncRepository<Questionary>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var questionary = new Questionary { Id = 1, StandardId = 1 };
            var spec = new QuestionaryFilterSpecification(questionaryId: 1);

            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<QuestionaryFilterSpecification>())).ReturnsAsync(questionary);

            var request = new GetByIdFullIncludeHandler.Request(id: 1);
            var handler = new GetByIdFullIncludeHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            var spec = new QuestionaryFilterSpecification(questionaryId: 1);
            _mockRepository = new Mock<IAsyncRepository<Questionary>>();
            _mockRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<QuestionaryFilterSpecification>())).ReturnsAsync((Questionary)null);

            var request = new GetByIdFullIncludeHandler.Request(id: 1);
            var handler = new GetByIdFullIncludeHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
