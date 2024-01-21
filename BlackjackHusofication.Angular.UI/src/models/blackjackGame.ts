import { Card } from "./card";
import { Dealer } from "./dealer";
import { Husoka } from "./husoka";
import { Player } from "./player";

export class BlackJackGame{
    roundNo : number;
    deck : Card[] = [];
    playedCards : Card[] = [];
    isShoeShouldChange :boolean = false;
    shufflerCard : Card;
    dealer : Dealer;
    players: Player[] = [];
    spots : boolean[] = [];
    husoka : Husoka;
}