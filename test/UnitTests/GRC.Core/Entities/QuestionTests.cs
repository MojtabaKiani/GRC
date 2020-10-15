using GRC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.GRCCore.Entities
{
    public class QuestionTests
    {
        private Question _sut;
        private DateTime _dt = DateTime.Now;

        public QuestionTests()
        {
            _sut = new Question
            {
                Id = 1,
                AnswersList = "1,2,3,4",
                ControlId = 3,
                CorrectAnswerIndex = 4,
                Description = "Description",
                Text = "Text",
                Weight = 8
            };
        }

        [Fact]
        public void Question_Should_Bind_Corrctly()
        {

            Assert.Equal(1, _sut.Id);
            Assert.Equal("1,2,3,4", _sut.AnswersList);
            Assert.Equal(3, _sut.ControlId);
            Assert.Equal(4, _sut.CorrectAnswerIndex);
            Assert.Equal("Description", _sut.Description);
            Assert.Equal("Text", _sut.Text);
            Assert.Equal(8, _sut.Weight);
        }

    }
}
