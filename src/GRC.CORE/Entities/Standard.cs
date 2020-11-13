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


        private readonly List<Domain> _domains;

        public Standard()
        {
            _domains = new List<Domain>();
            Questionaries = new List<Questionary>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [DisplayName("Release Year")]
        [YearValidation]
        public int ReleaseYear { get; set; }

        [DisplayName("Category")]
        [Required]
        public int StandardCategoryId { get; set; }

        [DisplayName("Category")]
        public StandardCategory StandardCategory { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [DisplayName("Standard Name")]
        [NotMapped]
        public string FullName => $"{Name} - {ReleaseYear}";

        public IReadOnlyCollection<Domain> Domains => _domains.AsReadOnly();


        public List<Questionary> Questionaries { get; set; }

        public void AddDomains(Domain domain)
        {
            Guard.Against.Null<Domain>(domain,nameof(domain));
            if (Domains.Any(c => c.Code == domain.Code))
                throw (new ArgumentException("Doamin with specified code, already exists.",nameof(domain.Code)));
            _domains.Add(domain);
        }
    }
}