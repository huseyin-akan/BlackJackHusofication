import { Hand } from "./hand";
import { Player } from "./player";

export class Spot{
    id : number;
    hand : Hand = new Hand(); 
    splittedHand : Hand;
    betAmount : number;
    player : Player;
}