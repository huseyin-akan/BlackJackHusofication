import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SimulationLog } from '../models/log-models/simulationLogs';
import { BjSimulation } from '../models/bjSimulation';
import { BjRoom } from '../models/bjRoom';

@Injectable({
  providedIn: 'root',
})
export class BjGameHubService {
  private hubConnection: signalR.HubConnection;

  private logSubject = new Subject<SimulationLog>();
  private bjSimulationSubject = new Subject<BjSimulation>();
  private roomsSubject = new Subject<string[]>();
  private activeRoomSubject = new Subject<BjRoom>();

  log$ = this.logSubject.asObservable();
  bjSimulation$ = this.bjSimulationSubject.asObservable();
  rooms$ = this.roomsSubject.asObservable();
  activeRoom$ = this.activeRoomSubject.asObservable();

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

    this.hubConnection.on('PlayerJoinedRoom', (room: BjRoom) => {
      this.activeRoomSubject.next(room);
    });

    this.hubConnection.on('SitPlayer', (room: BjRoom) => {
      this.activeRoomSubject.next(room);
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
  
  // getAllRooms() : BjRoom[] {
  //   let dataToReturn : BjRoom[] = [];
  //   this.hubConnection
  //     .invoke('GetAllBjRooms')
  //     .then(data => dataToReturn =data)
  //     .catch((err) => console.error(err));
  //     return dataToReturn;
  // }

  sendAction(action: string, data?: any): void {
    this.hubConnection
      .invoke('SendLog', action, data)
      .catch((err) => console.error(err));
  }

  joinGroup(groupName : string){
    return this.hubConnection
      .invoke('PlayerJoinRoom', groupName);
  }

  sitPlayer(groupName : string, spotIndex : number){
    return this.hubConnection
      .invoke('SitPlayer', groupName, spotIndex);     
  }
}