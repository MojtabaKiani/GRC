using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.ControlHandlers;
using System.Threading;
using System.Collections.Generic;

namespace UnitTests.GRCWeb.Features.Controls
{
    public class GetAllHandlerTests
    {

        private Mock<IControlInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_List_When_ControlId_Exists()
        {

            int DomainId = 3;
            var controlList = new List<Control>
            {
                 new Control { Id = 1, DomainId = DomainId, Text = "Test" },
                 new Control { Id = 2, DomainId = DomainId, Text = "Test" }
            };
            _mockRepository = new Mock<IControlInterface>();
            _mockRepository.Setup(x => x.ListAllAsync(It.Is<int>(x => x == DomainId))).ReturnsAsync(controlList);

            var request = new GetAllHandler.Request(DomainId);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_ControlId_Not_Exists()
        {
            int DomainId = 3;
            var controlList = new List<Control>
            {
                 new Control { Id = 1, DomainId = DomainId, Text = "Test" },
                 new Control { Id = 2, DomainId = DomainId, Text = "Test" }
            };
            _mockRepository = new Mock<IControlInterface>();
            _mockRepository.Setup(x => x.ListAllAsync(It.Is<int>(x => x == DomainId))).ReturnsAsync(controlList);

            var request = new GetAllHandler.Request(1);
            var handler = new GetAllHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
