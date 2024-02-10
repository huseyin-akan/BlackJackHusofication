import { Card } from "../card";
import { Player } from "../player";

export class DealCardAction{
    cardToDeal : Card;
    isDealForDealer : boolean;
    playerToDeal : Player;
}