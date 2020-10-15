using System.ComponentModel.DataAnnotations;

namespace GRC.Core.Entities
{
    public class BaseEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }
    }
}
