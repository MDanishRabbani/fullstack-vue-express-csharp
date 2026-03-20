using csharp_rest_api.DTOs;
using csharp_rest_api.Models;

namespace csharp_rest_api.Services;

public sealed class InMemoryTodoService : ITodoService
{
    private readonly List<TodoItem> _items =
    [
        new TodoItem { Id = 1, Title = "Run backend", IsDone = true },
        new TodoItem { Id = 2, Title = "Run frontend", IsDone = true }
    ];

    private readonly object _sync = new();

    public IReadOnlyList<TodoItem> GetAll()
    {
        lock (_sync)
        {
            return _items
                .OrderBy(x => x.Id)
                .Select(Clone)
                .ToList();
        }
    }

    public TodoItem? GetById(int id)
    {
        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return item is null ? null : Clone(item);
        }
    }

    public TodoItem Create(CreateTodoRequest request)
    {
        lock (_sync)
        {
            var nextId = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;
            var item = new TodoItem
            {
                Id = nextId,
                Title = request.Title.Trim(),
                IsDone = request.IsDone
            };

            _items.Add(item);
            return Clone(item);
        }
    }

    public TodoItem? Update(int id, CreateTodoRequest request)
    {
        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null) return null;

            item.Title = request.Title.Trim();
            item.IsDone = request.IsDone;
            return Clone(item);
        }
    }

    public bool Delete(int id)
    {
        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null) return false;
            _items.Remove(item);
            return true;
        }
    }

    private static TodoItem Clone(TodoItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        IsDone = item.IsDone
    };
}
