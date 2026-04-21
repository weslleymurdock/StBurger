namespace StBurger.Domain.Menu.Entities;

public class MenuCatalog
{
    private readonly List<MenuItem> _items = [];

    public IReadOnlyCollection<MenuItem> Items => _items.AsReadOnly();

    public void AddItem(MenuItem item)
    {
        _items.Add(item);
    }

    public MenuItem? GetItemByName(string name)
    {
        return _items.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
