using GRC.Core.Entities;
using System;
using System.Linq;
using Xunit;

namespace UniteTests.GRCCore.Entities
{
    public class DomainTests
    {
        private Domain _sut;

        public DomainTests()
        {
            _sut = new Domain
            {
                Id = 1,
                Code = "A.5",
                Title = "Information security policies",
                Description = "Information security policies Description",
                StandardId = 1
            };
        }
        [Fact]
        public void Domain_Should_Binds_Correctly()
        {
            Assert.Equal("A.5", _sut.Code);
            Assert.Equal("Information security policies", _sut.Title);
            Assert.Equal("Information security policies Description", _sut.Description);
            Assert.Equal(1, _sut.StandardId);
        }

        [Fact]
        public void Adding_Control_Should_Works()
        {
            _sut.AddControl(new Control 
            { 
                Text="Some Text",
                Code="a",
                Level=1
            });

            var dom = _sut.Controls.Single();

            Assert.NotNull(dom);
        }

        [Theory]
        [InlineData("5.2", "Policy", "5.2) Policy")]
        [InlineData("5", "Policy & Control", "5) Policy & Control")]
        [InlineData("", "Policy & Control", "Policy & Control")]
        public void FullText_Property_Should_Binds_Correctly(string code, string title,string fullText)
        {
            var sut = new Domain
            {
                Id = 1,
                Code = code,
                Title = title
            };
            Assert.Equal(fullText, sut.FullText);
        }

         [Fact]
        public void Adding_Domain_With_Parent_Code_Should_Returns_Exception()
        {
            var control = new Control
            {
                Text = "Some Text",
                Code = _sut.Code,
                Level = 1
            };

            var exp = Assert.Throws<ArgumentException>(() => _sut.AddControl(control));
            Assert.Equal(nameof(control.Code), exp.ParamName);
        }

        [Fact]
        public void Adding_Control_With_Duplicate_Code_Should_Returns_Exception()
        {
            var control = new Control
            {
                Text = "Some Text",
                Code = "a",
                Level = 1
            };

            _sut.AddControl(control);
            var exp = Assert.Throws<ArgumentException>(() => _sut.AddControl(control));
            Assert.Equal(nameof(control.Code), exp.ParamName);
        }

        [Fact]
        public void Adding_Null_Control_Should_Returns_Exception()
        {
            var exp = Assert.Throws<ArgumentNullException>(() => _sut.AddControl(null));
            Assert.Equal(nameof(Control), exp.ParamName, ignoreCase: true);
        }

    }

}
