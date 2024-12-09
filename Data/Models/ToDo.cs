using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Data.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
