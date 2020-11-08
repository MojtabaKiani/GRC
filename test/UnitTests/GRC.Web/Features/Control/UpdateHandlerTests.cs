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
    public class UpdateHandlerTests
    {

        private Mock<IAsyncRepository<Control>> _mockRepository;

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public async Task Handler_Should_Retrun_Modified_object_Count_On_Correct_Object(int controlId, int returnCount)
        {
            var control = new Control { Id = controlId, DomainId = 1, Text = "Test" };
            _mockRepository = new Mock<IAsyncRepository<Control>>();
            _mockRepository.Setup(x => x.UpdateAsync(It.Is<Control>(x=> x.Id==1))).ReturnsAsync(1);

            var request = new UpdateHandler.Request(control);
            var handler = new UpdateHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(returnCount, result);
        }
    }
}
