import { Hand } from "./hand";

export class Dealer{
    id: number;
    name : string;
    balance : number;
    hand: Hand = new Hand();
}