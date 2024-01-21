import { Card } from "./card";

export class Hand{
    cards : Card[] = [];
    isBlackJack : boolean;
    isSoft : boolean;
    isBusted : boolean;
    handValue : number;
    betAmount : number;
}