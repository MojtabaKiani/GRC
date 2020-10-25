using GRC.Core.Entities;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.GRCCore.Entities
{
    public class QuestionaryTests
    {
        private Questionary _sut;
        private DateTime _dt = DateTime.Now;

        public QuestionaryTests()
        {
            _sut = new Questionary("Moji")
            {
                Id = 1,
                StandardId = 2,
                ComplianceLevel = 3,
                CreateDate = _dt,
                Description = "Description",
                Name = "Name"
            };
        }

        [Fact]
        public void Questionary_Should_Bind_Corrctly()
        {

            Assert.Equal(1, _sut.Id);
            Assert.Equal(2, _sut.StandardId);
            Assert.Equal(3, _sut.ComplianceLevel);
            Assert.Equal(_dt, _sut.CreateDate);
            Assert.Equal("Description", _sut.Description);
            Assert.Equal("Name", _sut.Name);
            Assert.Equal("Moji", _sut.OwnerUid);
        }

        [Fact]
        public void Adding_Answer_Should_Works()
        {
            _sut.AddAnswer(new Answer
            {
                Id = 1,
                Point = 2
            });

            var answer = _sut.Answers.Single();
            Assert.NotNull(answer);
            Assert.Equal(1, answer.Id);
        }

        [Fact]
        public void Adding_Null_Answer_Should_Returns_Eception()
        {
            var exp = Assert.Throws<ArgumentNullException>(() => _sut.AddAnswer(null));
            Assert.Equal(nameof(Answer), exp.ParamName, ignoreCase: true);
        }

        [Fact]
        public void Percentage_Property_Should_Bind_Correctly()
        {
            var standard = new Standard { Id = 1 };
            standard.AddDomains(new Domain { Id = 1, Code = "1" });
            standard.AddDomains(new Domain { Id = 2, Code = "2" });
            standard.Domains.Single(s => s.Id == 1).AddControl(new Control { Id = 1, Code = "10" });
            standard.Domains.Single(s => s.Id == 2).AddControl(new Control { Id = 4, Code = "20" });
            standard.Domains.Single(s => s.Id == 1).Controls.First().AddQuestion(new Question { Id = 1 });
            standard.Domains.Single(s => s.Id == 1).Controls.First().AddQuestion(new Question { Id = 2 });
            standard.Domains.Single(s => s.Id == 1).Controls.First().AddQuestion(new Question { Id = 3 });
            standard.Domains.Single(s => s.Id == 2).Controls.First().AddQuestion(new Question { Id = 4 });
            standard.Domains.Single(s => s.Id == 2).Controls.First().AddQuestion(new Question { Id = 5 });
            _sut.Standard = standard;
            _sut.AddAnswer(new Answer { Id = 1 });

            Assert.Equal(1 / 5, _sut.CompletePercentage);

        }

    }
}
