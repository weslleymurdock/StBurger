namespace StBurger.Domain.Menu.Entities;

public class Side : MenuItem
{
	public Side() : base(string.Empty, string.Empty, 0)
	{

	}  
    public Side(string name, string description, decimal price) : base(name, description, price) { }
}
