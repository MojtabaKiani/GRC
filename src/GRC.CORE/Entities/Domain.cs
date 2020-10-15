using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GRC.Core.Entities
{
    public class Domain : BaseEntity
    {
        public Domain()
        {
            Domains = new List<Domain>();
            Controls = new List<Control>();
        }
        [Required]
        [MaxLength(10 , ErrorMessage = "Code length must be less than 10")]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public int StandardId { get; set; }

        public Standard Standard { get; set; }

        [NotMapped]
        public List<Domain> Domains { get; set; }

        public List<Control> Controls { get; set; }

        [DisplayName("Domain Title")]
        [NotMapped]
        public string FullText => (Code == "") ? Title : $"{Code}) {Title}";

        public void AddControl(Control control)
        {
            Guard.Against.Null<Control>(control, nameof(control));

            if (Controls.Any(c => c.Code == control.Code) | Code == control.Code)
                throw (new ArgumentException("Control with specified code, already exists.", nameof(control.Code)));

            control.DomainId = Id;
            Controls.Add(control);
        }
    }
}