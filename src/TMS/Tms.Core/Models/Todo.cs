using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tms.Core.Models
{
    [Table("Todos")]
    public class Todo : Entity
    {
        private List<Task> _tasks = new List<Task>();

        [Required]
        [StringLength(80)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name="Todo State")]
        public TodoState State { get; set; }

        public virtual ICollection<Task> Tasks
        {
            get { return _tasks; }
            set { _tasks = (List<Task>)value; }
        }
    }
}