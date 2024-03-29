import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SimulationLog } from '../models/log-models/simulationLogs';
import { BjSimulation } from '../models/bjSimulation';
import { BjGame } from '../models/bjGame';
import { CountDownNotification } from '../models/notifications/countDownNotification';
import { CardDealNotification } from '../models/notifications/cardDealNotification';
import { Table } from '../models/table';
import { CardAction } from '../models/card';
import { AwaitingCardActionNotification } from '../models/notifications/awaitingCardActionNotification';
import { RoundWinningsNotification } from '../models/notifications/roundWinningsNotification';
import { SecretCardNotification } from '../models/notifications/secretCardNotification';
import { Player } from '../models/player';
import { Hand } from '../models/hand';
import { Spot } from '../models/spot';

@Injectable({
  providedIn: 'root',
})
export class BjGameHubService {
  private hubConnection: signalR.HubConnection;

  private activeRoom :BjGame = new BjGame();

  private logSubject = new Subject<SimulationLog>();
  private bjSimulationSubject = new Subject<BjSimulation>();
  private roomsSubject = new Subject<string[]>();
  private activeRoomSubject = new Subject<BjGame>();
  private countDownNotification = new Subject<CountDownNotification>();
  private cardDealNotifiation = new Subject<CardDealNotification>();
  private awaitCardActionNotification = new Subject<AwaitingCardActionNotification>();
  private roundWinningsNotification = new Subject<RoundWinningsNotification>();
  private secretCardNotification = new Subject<SecretCardNotification>();
  private currentPlayer= new Subject<Player>();
  private updateHand= new Subject<Hand>();

  log$ = this.logSubject.asObservable();
  bjSimulation$ = this.bjSimulationSubject.asObservable();
  rooms$ = this.roomsSubject.asObservable();
  activeRoom$ = this.activeRoomSubject.asObservable();
  countDownNotification$ = this.countDownNotification.asObservable();
  cardDealNotifiation$ = this.cardDealNotifiation.asObservable();
  awaitCardActionNotification$ = this.awaitCardActionNotification.asObservable();
  roundWinningsNotification$ = this.roundWinningsNotification.asObservable();
  secretCardNotification$ = this.secretCardNotification.asObservable();
  currentPlayerSubject$ = this.currentPlayer.asObservable();
  updateHand$ = this.updateHand.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7255/bj-game')
      .withAutomaticReconnect() //Bağlantı koparsa yeni bağlantı isteği gönderir. (0, 2, 10 ,30 saniyede istek atar.) veya ms cinsinden kendin de array verebilirsin.
      .build();

    this.start();

    this.hubConnection.on('SendLog', (log: SimulationLog) => {
      this.logSubject.next(log);
    });

    this.hubConnection.on('UpdateSimulation', (info: BjSimulation) => {
      this.bjSimulationSubject.next(info);
    });

    this.hubConnection.on('GetAllBjRooms', (rooms: string[]) => {
      this.roomsSubject.next(rooms);
    });

    this.hubConnection.on('PlayerJoinedRoom', (room: BjGame) => {
      this.activeRoom = room;
      this.activeRoomSubject.next(room);
    });

    this.hubConnection.on('PlayerLeaveSpot', (room: BjGame) => {
      this.activeRoom = room;
      this.activeRoomSubject.next(room);
    });

    this.hubConnection.on('PlayerBet', (betAmount: number, spotIndex : number) => {
      console.log('gelen degerler' , betAmount, spotIndex)
      this.activeRoom.table.spots[spotIndex-1].betAmount = betAmount;
      console.log('this.activeRoom',this.activeRoom)
      this.activeRoomSubject.next(this.activeRoom);
    });

    //--------------------------UPDATES------------------------
    this.hubConnection.on('UpdateTable', (table: Table) => {
      this.activeRoom.table = table;
      this.activeRoomSubject.next(this.activeRoom);
    });

    this.hubConnection.on('UpdatePlayer', (player: Player) => {
      this.currentPlayer.next(player);
    });

    this.hubConnection.on('UpdateHand', (hand: Hand) => {
      this.updateHand.next(hand);
    });

    this.hubConnection.on('UpdateSpots', (spots: Spot[]) => {
      this.activeRoom.table.spots = spots;
      this.activeRoomSubject.next(this.activeRoom);
    });
    //--------------------------UPDATES------------------------

    //--------------------------NOTIFICATIONS------------------------
    this.hubConnection.on('NotifyCountDown', (notification: CountDownNotification) => {
      this.countDownNotification.next(notification);
    });

    this.hubConnection.on('NotifyCardDeal', (notification: CardDealNotification) => {
      this.cardDealNotifiation.next(notification);
    });

    this.hubConnection.on('NotifyAwaitingCardAction', (notification: AwaitingCardActionNotification) => {
      this.awaitCardActionNotification.next(notification);
    });

    this.hubConnection.on('NotifyRoundWinnigs', (notification: RoundWinningsNotification) => {
      this.roundWinningsNotification.next(notification);
    });

    this.hubConnection.on('NotifySecretCard', (notification: SecretCardNotification) => {
      this.secretCardNotification.next(notification);
    });
    //--------------------------NOTIFICATIONS------------------------

    this.hubConnection.on('RoundStarted', (roundNumber : number) => {
      
    });

    this.hubConnection.onreconnecting(error => {
      //Bağlantı kesildikten sonra tekrar bağlanmaya çalışmadan önce yapılacaklar varsa.
    });

    this.hubConnection.onreconnected(connectionId => {
      //Bağlantı tekrar sağlanınca
    });

    this.hubConnection.onclose(error => {
      //Bağlantı koğtuğunda
      //TODO-HUS buraya toastr ekleyelim ve toastr ile mesaj verelim.
    })
  }

  //Bağlantı hiç kurulamazsa 2 sn'de bir tekrar deneyecek.
  async start() {
    try {
      await this.hubConnection.start();
    } 
    catch (error) {
      console.error(error);
      setTimeout(() => this.start(), 2000);
    }
  }

  sendAction(action: string, data?: any): void {
    this.hubConnection
      .invoke('SendLog', action, data)
      .catch((err) => console.error(err));
  }

  joinGroup(roomName : string){
    return this.hubConnection
      .invoke('PlayerJoinRoom', roomName);
  }

  sitPlayer(spotId : number){
    return this.hubConnection
      .invoke('SitPlayer', this.activeRoom.name, spotId);     
  }

  playerLeaveSpot(spotId: number){
    return this.hubConnection
      .invoke('PlayerLeaveSpot', this.activeRoom.name, spotId);  
  }

  playerBet(spotId: number, betAmount : number){
    return this.hubConnection
      .invoke('PlayerBet', this.activeRoom.name, spotId, betAmount);  
  }

  playCardAction(action: CardAction, spotNo: number, isForSplittedHand : boolean = false){
    return this.hubConnection
      .invoke('PlayCardAction', action, this.activeRoom.name, spotNo, isForSplittedHand);  
  }
}