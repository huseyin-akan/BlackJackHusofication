namespace BlackJackHusofication.Model.Models;

//internal record Husoka(int Id, Player? HusokaBettedFor, bool HusokaIsMorting, decimal CurrentHusokaBet, string Name, decimal Balance);

public class Husoka
{
    public int Id { get; set; }
    public Player? HusokaBettedFor { get; set; }
    public bool HusokaIsMorting { get; set; }
    public decimal CurrentHusokaBet { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}