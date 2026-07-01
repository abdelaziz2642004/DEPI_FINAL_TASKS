using BookStore.Models;

namespace BookStore.Repositories;

public class Repository<T> where T : IEntity
{
    private readonly List<T> _data = new();
    private int _nextId = 1;

    public void Add(T entity)
    {
        entity.Id = _nextId++;
        _data.Add(entity);
    }

    public void Load(T entity)
    {
        _data.Add(entity);
        if (entity.Id >= _nextId)
            _nextId = entity.Id + 1;
    }

    public bool Remove(int id)
    {
        var item = _data.FirstOrDefault(x => x.Id == id);
        if (item is null) return false;
        _data.Remove(item);
        return true;
    }

    public T? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);

    public IReadOnlyList<T> GetAll() => _data.AsReadOnly();

    public IEnumerable<T> Find(Func<T, bool> predicate) => _data.Where(predicate);
}
