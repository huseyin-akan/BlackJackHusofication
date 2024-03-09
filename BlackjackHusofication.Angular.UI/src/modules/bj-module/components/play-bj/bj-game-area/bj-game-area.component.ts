import { Component } from '@angular/core';
import { BjGame } from '../../../../../models/bjGame';
import { BjGameHubService } from '../../../../../services/bjGameHubService.service';
import { BjEventType } from '../../../../../models/events/bjEventType';
import { CardDealNotification } from '../../../../../models/notifications/cardDealNotification';
import { CardAction } from '../../../../../models/card';
import { Player } from '../../../../../models/player';
import { SecretCardNotification } from '../../../../../models/notifications/secretCardNotification';
import { ToasterService } from '../../../../../services/toasterService.service';

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
  isAcceptingBets = false;

  constructor(private bjGameHubService :BjGameHubService, private toasterService: ToasterService){}

  ngOnInit(){
    this.bjGameHubService.activeRoom$.subscribe(room => {
      this.activeRoom = room
    });

    this.bjGameHubService.countDownNotification$.subscribe(ntf => {
      if(ntf.eventType == BjEventType.AcceptingBets){
        this.bjCounter = ntf.seconds;
        this.isAcceptingBets = ntf.seconds>0;
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
    this.bjGameHubService.sitPlayer(spotId)
    .catch((err) => console.error(err));
  }

  playerLeaveSpot(spotId: number): void {
    this.bjGameHubService.playerLeaveSpot(spotId)
    .catch((err) => console.error(err));
  }

  betPlayer(spotId: number): void {
    var spot = this.activeRoom.table.spots.find(x => x.id == spotId);
    if(spot.player.balance < this.tbBetAmount){
      this.toasterService.showError("Bet miktarı bakiyenizden yuksek olamaz.");
      return;
    }

    this.bjGameHubService.playerBet(spotId, this.tbBetAmount)
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
    this.bjGameHubService.playCardAction(cardAction, this.activeSpotNo, this.isActionForSplit)
  }
}