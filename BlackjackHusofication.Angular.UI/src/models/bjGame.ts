import { Player } from "./player";
import { Table } from "./table";

export class BjGame{
    roomId : number;
    name : string;
    roundNo : number;
    players : Player[] = [];
    table : Table;
    // isAcceptingBets : boolean;
    // isDealingAllCards : boolean;
    // isAskingForActions : boolean;
}