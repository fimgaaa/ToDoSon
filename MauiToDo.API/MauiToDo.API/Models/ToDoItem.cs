//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace MauiToDo.API.Models
//{
//    public class ToDoItem
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        [MaxLength(100)]
//        public string Title { get; set; }

//        public string Description { get; set; }

//        public bool IsCompleted { get; set; }

//        public DateTime DueDate { get; set; }

//        // İlişkili kullanıcı
//        public string UserId { get; set; }

//        [ForeignKey("UserId")]
//        public ApplicationUser User { get; set; }

//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//        public DateTime? UpdatedAt { get; set; }
//    }
//}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MauiToDo.API.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime? DueDate { get; set; } // Nullable yaptım

        // İlişkili kullanıcı
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } // Nullable navigation property

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}