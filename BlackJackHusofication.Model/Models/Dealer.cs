﻿namespace BlackJackHusofication.Model.Models;

public class Dealer 
{
    public Dealer()
    {
        Hand = new();
    }
    public required string Id { get; set; }
    public required string Name { get; set; }
    public Hand Hand { get; set; }
    public Card? SecretCard { get; set; }
}
