import { FormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { SignalRService } from '../../services/signalRService';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-bj',
  standalone: true,
  imports: [FormsModule, NgFor],
  templateUrl: './bj.component.html',
  styleUrl: './bj.component.css'
})
export class BjComponent {
  logs: string[] = [];
  betAmount;

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.log$.subscribe(log => {
      console.log("I work mazafaka")
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
