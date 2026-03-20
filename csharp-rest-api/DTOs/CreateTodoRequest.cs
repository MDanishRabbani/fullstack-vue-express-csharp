using System.ComponentModel.DataAnnotations;

namespace csharp_rest_api.DTOs;

public class CreateTodoRequest
{
    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
}
