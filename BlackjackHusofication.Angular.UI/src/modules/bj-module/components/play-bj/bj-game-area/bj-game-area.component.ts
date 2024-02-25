import { Component } from '@angular/core';
import { BjGame } from '../../../../../models/bjGame';
import { BjGameHubService } from '../../../../../services/bjGameHubService';
import { BjEventType } from '../../../../../models/events/bjEventType';
import { CardDealNotification } from '../../../../../models/notifications/cardDealNotification';
import { CardAction } from '../../../../../models/card';
import { Player } from '../../../../../models/player';
import { SecretCardNotification } from '../../../../../models/notifications/secretCardNotification';

@Component({
  selector: 'app-bj-game-area',
  templateUrl: './bj-game-area.component.html',
  styleUrl: './bj-game-area.component.css'
})
export class BjGameAreaComponent {
  players: boolean[] = Array(7).fill(false); 
  activeRoom :BjGame = new BjGame();
  currentUser: Player = new Player();
  winningAmount : number;

  bjCounter : number = 0;
  bjCounterForAction : number = 0;
  isBtnHitShow = false;
  isBtnStandShow = false;
  isBtnSplitShow = false;
  isBtnDoubleShow = false;
  activeSpotNo : number;
  isActionForSplit = false;
  isRenderActionButtons = false;
  tbBetAmount : number;

  constructor(private bjGameHubService :BjGameHubService){}

  ngOnInit(){
    console.log(this.activeRoom);

    this.bjGameHubService.joinGroup("Blackjack - 1");

    this.bjGameHubService.activeRoom$.subscribe(room => {
      this.activeRoom = room
      console.log("beni de zıplattı vellaaha")
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
      this.activeSpotNo = ntf.spotNo;
      this.renderActionButtons(ntf.spotNo, ntf.isForSplitHand);
    })

    this.bjGameHubService.roundWinningsNotification$.subscribe(ntf => {
      console.log(ntf);
      this.currentUser.balance = ntf.balance;
      this.winningAmount = ntf.earning;

      this.startNewRound();
    })

    this.bjGameHubService.secretCardNotification$.subscribe(ntf => {
      this.showSecretCard(ntf);
    });

    this.bjGameHubService.currentPlayerSubject$.subscribe(player => {
      this.currentUser = player; 
    });
  }

  sitPlayer(spotId: number): void {
    this.bjGameHubService.sitPlayer(this.activeRoom.name, spotId)
    .catch((err) => console.error(err));
  }

  betPlayer(spotId: number): void {
    this.bjGameHubService.playerBet(this.activeRoom.name, spotId, this.tbBetAmount)
    .catch((err) => console.error(err));
  }

  standPlayer(index: number): void {
    this.players[index] = false;
  }

  dealNewCard(ntf:CardDealNotification ){
    if(ntf.spotNo == 0){
      this.activeRoom.table.dealer.hand.cards.push(ntf.newCard);
      this.activeRoom.table.dealer.hand.handValue = ntf.handValue;
      return;
    }

    let spot = this.activeRoom.table.spots.find(s => s.id == ntf.spotNo);
    spot.hand.cards.push(ntf.newCard);
    spot.hand.handValue = ntf.handValue;
  }

  showSecretCard(ntf : SecretCardNotification){
    this.activeRoom.table.dealer.hand.cards[1] = ntf.secretCard;
    this.activeRoom.table.dealer.hand.handValue = ntf.handValue;
    //TODO-HUS buraya döndurme efekti eklicez.
  }

  startNewRound(){
    this.bjCounterForAction = 0;
    this.bjCounter = 0;
    this.activeSpotNo = -1;
  }

  renderActionButtons(spotNo : number, isForSplitHand : boolean){
    this.isRenderActionButtons = true;
    this.isActionForSplit = isForSplitHand;
    this.activeSpotNo = spotNo;
    this.isBtnHitShow = true;
    this.isBtnStandShow  =true;
  }

  playAction(action: string){
    const cardAction: CardAction = CardAction[action as keyof typeof CardAction];
    this.bjGameHubService.playCardAction(cardAction, this.activeRoom.name, this.activeSpotNo, this.isActionForSplit)
  }
}