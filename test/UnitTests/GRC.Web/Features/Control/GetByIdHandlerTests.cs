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
    public class GetByIdHandlerTests
    {

        private Mock<IControlInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var control = new Control { Id = 1, DomainId = 1, Text = "Test" };
            _mockRepository = new Mock<IControlInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.Is<int>(x => x == 1))).ReturnsAsync(control);

            var request = new GetByIDHandler.Request(1);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Text);
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            var control = new Control { Id = 1, DomainId = 1, Text = "Test" };
            _mockRepository = new Mock<IControlInterface>();
            _mockRepository.Setup(x => x.GetByIdAsync(It.Is<int>(x => x == 1))).ReturnsAsync(control);

            var request = new GetByIDHandler.Request(2);
            var handler = new GetByIDHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
