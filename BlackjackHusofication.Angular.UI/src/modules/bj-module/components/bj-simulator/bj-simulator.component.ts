import { Component } from '@angular/core';
import { SimulationLog } from '../../../../models/log-models/simulationLogs';
import { BjGameHubService } from '../../../../services/bjGameHubService';

@Component({
  selector: 'app-bj-simulator',
  templateUrl: './bj-simulator.component.html',
  styleUrl: './bj-simulator.component.css'
})
export class BjSimulatorComponent {
  logs: SimulationLog[] = [];
  betAmount;

  constructor(private signalRService: BjGameHubService) {}

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