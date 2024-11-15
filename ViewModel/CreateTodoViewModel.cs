using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModel;

public class CreateTodoViewModel
{
    [Required]
    public string? Title { get; set; }
}