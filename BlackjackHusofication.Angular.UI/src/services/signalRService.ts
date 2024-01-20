import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  private logSubject = new Subject<string>();

  log$ = this.logSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7255/blackjackhub') 
      .build();

    this.hubConnection.start().catch(err => console.error(err));

    this.hubConnection.on('SendLog', (log: string) => {
      this.logSubject.next(log);
    });
  }

  sendAction(action: string, data?: any): void {
    this.hubConnection.invoke('SendLog', action, data).catch(err => console.error(err));
  }
}
