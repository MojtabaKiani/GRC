using GRC.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRC.Web.Models
{
    public class QuestionaryViewModel 
    {
        public QuestionaryViewModel()
        {
            Standards = new List<SelectListItem>();
            DomainResults = new List<DomainResult>();
        }

        [Required]
        public int Id { get; set; }

        [DisplayName("Standard")]
        [Required]
        public int StandardId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name length must be less than 100")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Compliance Level")]
        [Range(1, 10, ErrorMessage = "Compliance Level must be between 1 and 10")]
        public int ComplianceLevel { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> Standards { get; set; }

        [DisplayName("Standard")]
        public string QuestionaryStandardFullName { get; set; }

        public double Result { get; set; }

        public List<DomainResult> DomainResults { get; set; }
      
    }
}
