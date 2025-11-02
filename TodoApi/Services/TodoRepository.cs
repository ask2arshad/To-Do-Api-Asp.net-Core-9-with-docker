using TodoApi.Models;

namespace TodoApi.Services;

public class TodoRepository
{
    private readonly List<TodoItem> _items = new();
    private int _nextId = 1;

    public IEnumerable<TodoItem> GetAll() => _items;
    public TodoItem? GetById(int id) => _items.FirstOrDefault(t => t.Id == id);

    public TodoItem Add(TodoItem item)
    {
        item.Id = _nextId++;
        _items.Add(item);
        return item;
    }

    public bool Update(int id, TodoItem updated)
    {
        var item = GetById(id);
        if (item is null) return false;
        item.Title = updated.Title;
        item.IsComplete = updated.IsComplete;
        return true;
    }

    public bool Delete(int id)
    {
        var item = GetById(id);
        if (item is null) return false;
        _items.Remove(item);
        return true;
    }
}
