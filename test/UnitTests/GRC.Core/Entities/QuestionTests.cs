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
        [Fact]
        public void Question_Should_Bind_Corrctly()
        {

            var sut = new Question
            {
                Id = 1,
                ControlId = 3,
                Description = "Description",
                Text = "Text",
                Weight = 8,
                QuestionAnswers = new List<QuestionAnswer> { new QuestionAnswer(), new QuestionAnswer() },
                Answers = new List<Answer> { new Answer() }

            };

            Assert.Equal(1, sut.Id);
            Assert.Equal(3, sut.ControlId);
            Assert.Equal("Description", sut.Description);
            Assert.Equal("Text", sut.Text);
            Assert.Equal(8, sut.Weight);
            Assert.Equal(2, sut.QuestionAnswers.Count());
            Assert.Single(sut.Answers);
        }

    }
}
