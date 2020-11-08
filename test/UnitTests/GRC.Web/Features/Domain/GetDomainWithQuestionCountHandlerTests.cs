using GRC.Core.Entities;
using GRC.Core.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using GRC.Web.Features.DomainHandlers;
using System.Threading;
using GRC.Core.Specifications;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UnitTests.GRCWeb.Features.Domains
{
    public class GetDomainWithQuestionCountHandlerTests
    {

        private Mock<IDomainInterface> _mockRepository;

        [Fact]
        public async Task Handler_Should_Retrun_object_When_Id_Exists()
        {
            var domains = new List<Tuple<string, int>>
            {
                Tuple.Create("Test1",3),
                Tuple.Create("Test2",2),
                Tuple.Create("Test3",0),
                Tuple.Create("Test4",10),
            };

            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.GetDomainWithQuestionCount(It.IsAny<int>())).ReturnsAsync(domains);


            var request = new GetDomainWithQuestionCountHandler.Request(standardId: 1);
            var handler = new GetDomainWithQuestionCountHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(4, result.Count);
            Assert.Equal(15, result.Sum(s=> s.Item2));
        }


        [Fact]
        public async Task Handler_Should_Retrun_Null_When_Id_Not_Exists()
        {
            _mockRepository = new Mock<IDomainInterface>();
            _mockRepository.Setup(x => x.GetDomainWithQuestionCount(It.IsAny<int>())).ReturnsAsync((List<Tuple<string, int>>)null);

            var request = new GetDomainWithQuestionCountHandler.Request(standardId: 1);
            var handler = new GetDomainWithQuestionCountHandler(_mockRepository.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
