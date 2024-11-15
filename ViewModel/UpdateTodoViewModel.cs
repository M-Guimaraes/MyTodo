using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModel;

public class UpdateTodoViewModel
{
    [Required]
    public string? Title { get; set; }
}