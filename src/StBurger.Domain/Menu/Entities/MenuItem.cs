using StBurger.Domain.Core.Abstractions;
using StBurger.Domain.Core.Entities;

namespace StBurger.Domain.Menu.Entities;

public abstract class MenuItem : AuditableEntity<string>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    protected MenuItem(string name, string description, decimal price)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description;
        Price = price;
    }

    public void Update(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}

