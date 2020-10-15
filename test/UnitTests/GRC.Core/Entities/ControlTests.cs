using GRC.Core.Entities;
using Xunit;

namespace UniteTests.GRCCore.Entities
{
    public class ControlTests
    {

        [Fact]
        public void Control_Should_Binds_Correctly()
        {
        string text = "ensuring that the resources needed for the information security management system are available;";
        var sut = new Control
            {
                Text = text,
                Level = 1,
                DomainId = 1,
                Code = "a"
            };

            Assert.Equal(text, sut.Text);
            Assert.Equal(1, sut.Level);
            Assert.Equal(1, sut.DomainId);
            Assert.Equal("a", sut.Code);
            Assert.Equal($"a) {text}", sut.FullText);
        }

        [Theory]
        [InlineData("5.2", "Policy", "5.2) Policy")]
        [InlineData("5", "Policy & Control", "5) Policy & Control")]
        [InlineData("", "Policy & Control", "Policy & Control")]
        public void FullText_Property_Should_Binds_Correctly(string code, string text, string fullText)
        {
            var sut = new Control
            {
                Id = 1,
                Code = code,
                Text = text
            };
            Assert.Equal(fullText, sut.FullText);
        }

    }
}
