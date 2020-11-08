using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.StandardCategoryHandlers;
using System.Threading;
using System.Collections.Generic;

namespace UnitTests.GRCWeb.Features.StandardCategories
{
    public class GetAllHandlerTests
    {

        private Mock<IAsyncRepository<StandardCategory>> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_StandardCategoryId_Exists()
        {

            var standardCategoryList = new List<StandardCategory>
            {
                 new StandardCategory { Id = 1},
                 new StandardCategory { Id = 2}
            };

            _mockRepository = new Mock<IAsyncRepository<StandardCategory>>();
            _mockRepository.Setup(x => x.ListAllAsync()).ReturnsAsync(standardCategoryList);

            var request = new GetAllHandler.Request();
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }
    }
}
