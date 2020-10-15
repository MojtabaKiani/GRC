using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GRC.Core.Entities
{
    public class StandardCategory : BaseEntity
    {
        public StandardCategory()
        {
            Standards = new List<Standard>();
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public List<Standard> Standards { get; set; }
    }
}