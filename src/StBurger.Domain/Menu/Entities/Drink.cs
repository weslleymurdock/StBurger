namespace StBurger.Domain.Menu.Entities;

public class Drink : MenuItem
{
    public Drink() : base(string.Empty, string.Empty, 0)
    {
    }
    public Drink(string name, string description, decimal price) : base(name, description, price) { }
}

