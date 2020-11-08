using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardHandlers;
using System.Threading;
using System.Collections.Generic;
using GRC.Core.Specifications;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnitTests.GRCWeb.Features.Standards
{
    public class GetAllHandlerTests
    {

        private Mock<IStandardInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_StandardId_Exists()
        {

            int standardCategoryId = 3;
            var standardList = new List<Standard>
            {
                 new Standard { Id = 1, StandardCategoryId = standardCategoryId },
                 new Standard { Id = 2, StandardCategoryId = standardCategoryId }
            };

            _mockRepository = new Mock<IStandardInterface>();
            _mockRepository.Setup(x => x.ListAllAsync()).ReturnsAsync(standardList);

            var request = new GetAllHandler.Request();
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_StandardId_Not_Exists()
        {
            _mockRepository = new Mock<IStandardInterface>();
            _mockRepository.Setup(x => x.ListAllAsync()).ReturnsAsync((List<Standard>)null);

            var request = new GetAllHandler.Request();
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
