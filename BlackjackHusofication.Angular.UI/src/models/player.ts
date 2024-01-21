import { Hand } from "./hand";

export class Player{
    id : number;
    name : string;
    spot : number;
    balance : number;
    losingStreak : number;
    notWinningStreak : number;
    winningStreak : number;
    hasBetted : boolean;
    hand : Hand;
    splittedHand : Hand;
}