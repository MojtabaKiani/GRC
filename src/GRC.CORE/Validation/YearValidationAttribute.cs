using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRC.Core.Validation
{
    public class YearValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int year = (int)value;
            if (year >= 1980 && year <= DateTime.Now.Year)
                return ValidationResult.Success;
            return new ValidationResult($"Release Date Must be between 1980 and {DateTime.Now.Year}");
        }

    }
}
