import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SimulationLog } from '../models/log-models/simulationLogs';
import { BjSimulation } from '../models/bjSimulation';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  private logSubject = new Subject<SimulationLog>();
  private bjSimulationSubject = new Subject<BjSimulation>();

  log$ = this.logSubject.asObservable();
  bjSimulation$ = this.bjSimulationSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7255/blackjackhub')
      .withAutomaticReconnect() //Bağlantı koparsa yeni bağlantı isteği gönderir. (0, 2, 10 ,30 saniyede istek atar.) veya ms cinsinden kendin de array verebilirsin.
      .build();

    this.start();

    this.hubConnection.on('SendLog', (log: SimulationLog) => {
      this.logSubject.next(log);
    });

    this.hubConnection.on('UpdateSimulation', (info: BjSimulation) => {
      this.bjSimulationSubject.next(info);
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
}