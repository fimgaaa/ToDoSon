using Microsoft.AspNetCore.Identity;

namespace MauiToDo.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ek kullanıcı özellikleri eklenecekse buraya yazabilirsin
        // Örneğin:
        // public string FullName { get; set; }

        // Kullanıcının görevleri
        public ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
