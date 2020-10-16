using System;
using System.Linq;
using Xunit;
using GRC.Core.Entities;

namespace UniteTests.GRCCore.Entities
{
    public class StandardTests
    {
        private Standard _sut;

        public string _name { get { return "ISMS (ISO/IEC 27001)"; } }
        public int _releaseYear { get { return 2018; } }
        public string _description { get { return "ISO/IEC 27001 is an international standard on how to manage information security."; } }

        public StandardTests()
        {
            _sut = new Standard
            {
                Name = _name,
                ReleaseYear = _releaseYear,
                StandardCategoryId = 0,
                Description = _description
            };
        }

        [Fact]
        public void Standard_Should_Binds_Correctly()
        {
            Assert.Equal(_name, _sut.Name);
            Assert.Equal(_releaseYear, _sut.ReleaseYear);
            Assert.Equal(_description, _sut.Description);
            Assert.Equal($"{_name} - {_releaseYear}", _sut.FullName);
        }


        [Fact]
        public void Adding_Domain_Should_Works()
        {
            _sut.AddDomains(new Domain
            {
                Code = "A.5",
                Title = "Information security policies",
                Description = "Information security policies Description",
            });

            var domain = _sut.Domains.Single();

            Assert.NotNull(domain);
        }

        [Fact]
        public void Adding_Domain_With_Duplicate_Code_Should_Returns_Eception()
        {
            var domain = new Domain
            {
                Code = "A.5",
                Title = "Information security policies",
                Description = "Information security policies Description",
            };

            _sut.AddDomains(domain);
            var exp = Assert.Throws<ArgumentException>(() => _sut.AddDomains(domain));
            Assert.Equal(nameof(domain.Code), exp.ParamName);
        }

        [Fact]
        public void Adding_Null_Domain_Should_Returns_Eception()
        {
            var exp = Assert.Throws<ArgumentNullException>(() => _sut.AddDomains(null));
            Assert.Equal(nameof(Domain), exp.ParamName,ignoreCase:true);
        }
    }
}
