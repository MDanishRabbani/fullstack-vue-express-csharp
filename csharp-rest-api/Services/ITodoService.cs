using csharp_rest_api.DTOs;
using csharp_rest_api.Models;

namespace csharp_rest_api.Services;

public interface ITodoService
{
    IReadOnlyList<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem Create(CreateTodoRequest request);
    TodoItem? Update(int id, CreateTodoRequest request);
    bool Delete(int id);
}
