using System.ComponentModel.DataAnnotations;

namespace Tms.Core.Models
{
    public class Task : Entity
    {
        [Required]
        public virtual Todo Todo { get; set; } // Link to parent.

        public string Description { get; set; }
        public float Hours { get; set; }
        public bool Done { get; set; }
    }
}
