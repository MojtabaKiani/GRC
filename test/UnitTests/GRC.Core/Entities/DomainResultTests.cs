using GRC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.GRCCore.Entities
{
    public class ControlResult
    {
        [Theory]
        [InlineData(5,4,0.8)]
        [InlineData(10,2,0.2)]
        [InlineData(0,0,0)]
        public void CompletePercentage_Should_Retuern_Correct_Value(int questionCount,int answerCount,double Percentage)
        {
            var sut = new DomainResult("FullName", questionCount);
            sut.AnswerCount = answerCount;
            Assert.Equal(Percentage, sut.CompletePercenage);
        }

        [Fact]
        public void Constructor_Should_Work_Correctly()
        {
            var sut = new DomainResult("FullName", 10);
            Assert.Equal(10, sut.QuestionCount);
            Assert.Equal("FullName", sut.FullName);
        }
    }
}
