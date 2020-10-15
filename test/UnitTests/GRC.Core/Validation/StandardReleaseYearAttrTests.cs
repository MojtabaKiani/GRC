using GRC.Core.Entities;
using GRC.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.GRCCore.Validation
{
    public class StandardReleaseYearAttrTests
    {
        [Theory]
        [InlineData(1990)]
        [InlineData(2010)]
        [InlineData(2020)]
        [InlineData(2018)]
        public void Should_Validate_On_Valid_Input(int year)
        {
            var standard = new Standard
            {
                Name="ISMS",
                ReleaseYear=year,
            };

            var validator = new YearValidationAttribute();
            var result=validator.GetValidationResult(year,new System.ComponentModel.DataAnnotations.ValidationContext(standard));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(1970)]
        [InlineData(2030)]
        [InlineData(2021)]
        [InlineData(18)]
        public void Should_Return_Error_On_Invalid_Input(int year)
        {
            var standard = new Standard
            {
                Name = "ISMS",
                ReleaseYear = year,
            };

            var validator = new YearValidationAttribute();
            var result = validator.GetValidationResult(year, new System.ComponentModel.DataAnnotations.ValidationContext(standard));
            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
