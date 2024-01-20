import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../services/signalRService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  
})
@Component({
  selector: 'app-blackjack',
  imports: [ CommonModule, FormsModule],
  standalone: true,
  templateUrl: './blackjack.component.html',
  styleUrls: ['./blackjack.component.css']  
})
export class BlackjackComponent implements OnInit {
  logs: string[] = [];
  betAmount;

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.log$.subscribe(log => {
      this.logs.push(log);
    });
  }

  stand(): void {
    this.signalRService.sendAction('Stand');
  }

  hit(): void {
    this.signalRService.sendAction('Hit');
  }

  double(): void {
    this.signalRService.sendAction('Double');
  }

  bet(amount: number): void {
    this.signalRService.sendAction('Bet', amount);
  }

  leaveTable(): void {
    this.signalRService.sendAction('LeaveTable');
  }

  joinTable(): void {
    this.signalRService.sendAction('JoinTable');
  }
}
