import { Dealer } from "./dealer";
import { Player } from "./player";
import { Spot } from "./spot";

export  class Table {
    dealer : Dealer;
    players : Player[] = [];
    balance : number;
    spots : Spot[] = Array(7);
}