import { Player } from "./player";
import { Table } from "./table";

export class BjRoom{
    roomId : number;
    name : string;
    roundNo : number;
    players : Player[] = [];
    table : Table;
    isAcceptingBets : boolean;
}