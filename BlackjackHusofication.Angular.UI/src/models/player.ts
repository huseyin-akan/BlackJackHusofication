import { Hand } from "./hand";

export class Player{
    id : string;
    name : string;
    spot : number;
    balance : number;
    losingStreak : number;
    notWinningStreak : number;
    winningStreak : number;
}