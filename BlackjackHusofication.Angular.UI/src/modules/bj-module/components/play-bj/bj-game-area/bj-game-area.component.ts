import { Component } from '@angular/core';
import { BjGame } from '../../../../../models/bjGame';
import { BjGameHubService } from '../../../../../services/bjGameHubService';
import { BjEventType } from '../../../../../models/events/bjEventType';
import { CardDealNotification } from '../../../../../models/notifications/cardDealNotification';
import { CardAction } from '../../../../../models/card';
import { Player } from '../../../../../models/player';

@Component({
  selector: 'app-bj-game-area',
  templateUrl: './bj-game-area.component.html',
  styleUrl: './bj-game-area.component.css'
})
export class BjGameAreaComponent {
  players: boolean[] = Array(7).fill(false); 
  activeRoom :BjGame = new BjGame();
  currentUser: Player;
  winningAmount : number;

  bjCounter : number = 0;
  bjCounterForAction : number = 0;
  isBtnHitShow = false;
  isBtnStandShow = false;
  isBtnSplitShow = false;
  isBtnDoubleShow = false;
  isActionforSpotNo;
  isActionForSplit = false;
  isRenderActionButtons = false;

  constructor(private bjGameHubService :BjGameHubService){}

  ngOnInit(){
    console.log(this.activeRoom);

    this.bjGameHubService.joinGroup("Blackjack - 1");

    this.bjGameHubService.activeRoom$.subscribe(room => {
      this.activeRoom = room
    });

    this.bjGameHubService.countDownNotification$.subscribe(ntf => {
      if(ntf.eventType == BjEventType.AcceptingBets){
        this.bjCounter = ntf.seconds;
      }
    })

    this.bjGameHubService.cardDealNotifiation$.subscribe(ntf => {
        this.dealNewCard(ntf);
    })

    this.bjGameHubService.awaitCardActionNotification$.subscribe(ntf => {
      this.bjCounterForAction = ntf.seconds;
      this.renderActionButtons(ntf.spotNo, ntf.isForSplitHand);
    })

    this.bjGameHubService.roundWinningsNotification$.subscribe(ntf => {
      this.currentUser.balance = ntf.balance;
      this.winningAmount = ntf.earning;
    })
  }

  sitPlayer(spotId: number): void {
    this.bjGameHubService.sitPlayer(this.activeRoom.name, spotId)
    .catch((err) => console.error(err));
  }

  betPlayer(spotId: number): void {
    this.bjGameHubService.playerBet(this.activeRoom.name, spotId, 100)
    .catch((err) => console.error(err));
  }

  standPlayer(index: number): void {
    this.players[index] = false;
  }

  dealNewCard(ntf:CardDealNotification ){
    if(ntf.spotNo == 0){
      let dealerHand = this.activeRoom.table.dealer.hand;
      dealerHand.cards.push(ntf.newCard);
      return;
    }

    let spot = this.activeRoom.table.spots.find(s => s.id == ntf.spotNo);
    spot.hand.cards.push(ntf.newCard);
  }

  renderActionButtons(spotNo : number, isForSplitHand : boolean){
    this.isRenderActionButtons = true;
    this.isActionForSplit = isForSplitHand;
    this.isActionforSpotNo = spotNo;
    this.isBtnHitShow = true;
    this.isBtnStandShow  =true;
  }

  playAction(action: string){
    const cardAction: CardAction = CardAction[action as keyof typeof CardAction];
    this.bjGameHubService.playCardAction(cardAction, this.activeRoom.name, this.isActionforSpotNo, this.isActionForSplit)
  }
}