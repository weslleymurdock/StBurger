using StBurger.Domain.Core.Abstractions;

namespace StBurger.Domain.Menu.Entities;

public class Sandwich : MenuItem
{
	public Sandwich() : base(string.Empty, string.Empty, 0)
    {
	}
    public Sandwich(string name, string description, decimal price) : base(name, description, price) { }
}

