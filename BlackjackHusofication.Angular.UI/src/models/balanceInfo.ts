import { DealerInfo } from "./dealerInfo";
import { HusokaInfo } from "./husokaInfo";
import { PlayerInfo } from "./playerInfo";

export class BalanceInfo{
    playerInfos : PlayerInfo [] = [];
    dealerInfo : DealerInfo
    husokaInfo : HusokaInfo;
}