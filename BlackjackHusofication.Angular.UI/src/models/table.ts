import { Dealer } from "./dealer";
import { Player } from "./player";

export  class Table {
    dealer : Dealer;
    players : Player[] = [];
    balance : number;
    spots : boolean[] = Array(7);
}