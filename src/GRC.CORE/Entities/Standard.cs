using Ardalis.GuardClauses;
using GRC.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GRC.Core.Entities
{
    public class Standard : BaseEntity
    {
        public Standard()
        {
            Domains = new List<Domain>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [YearValidation]
        public int ReleaseYear { get; set; }

        [Required]
        public int StandardCategoryId { get; set; }

        [DisplayName("Category")]
        public StandardCategory StandardCategory { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [DisplayName("Standard Name")]
        [NotMapped]
        public string Fullname => $"{Name} - {ReleaseYear}";

        public List<Domain> Domains { get; set; }

        public void AddDomains(Domain domain)
        {
            Guard.Against.Null<Domain>(domain,nameof(domain));
            if (Domains.Any(c => c.Code == domain.Code))
                throw (new ArgumentException("Doamin with specified code, already exists.",nameof(domain.Code)));
            Domains.Add(domain);
        }
    }
}