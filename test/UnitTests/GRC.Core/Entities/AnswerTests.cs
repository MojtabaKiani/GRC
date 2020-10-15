using GRC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.GRCCore.Entities
{
    public class AnswerTests
    {
        private Answer _sut;
        private DateTime _dt = DateTime.Now;

        public AnswerTests()
        {
            _sut = new Answer
            {
                Id = 1,
                AnswerText = "Answer",
                AnswerValue = 3,
                Point = 1.12,
                Description = "Description",
                QuestionaryId = 10,
                QuestionId = 8
            };
        }

        [Fact]
        public void Answer_Should_Bind_Corrctly()
        {
            Assert.Equal(1, _sut.Id);
            Assert.Equal("Answer", _sut.AnswerText);
            Assert.Equal(3, _sut.AnswerValue);
            Assert.Equal(1.12, _sut.Point);
            Assert.Equal("Description", _sut.Description);
            Assert.Equal(10, _sut.QuestionaryId);
            Assert.Equal(8, _sut.QuestionId);
        }

    }
}
