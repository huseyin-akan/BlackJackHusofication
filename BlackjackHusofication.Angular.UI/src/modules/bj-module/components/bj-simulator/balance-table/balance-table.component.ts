import { BjSimulation } from '../../../../../models/bjSimulation';
import { Component } from '@angular/core';
import { SignalRService } from '../../../../../services/signalRService';

@Component({
  selector: 'app-balance-table',
  templateUrl: './balance-table.component.html',
  styleUrl: './balance-table.component.css'
})
export class BalanceTableComponent {
  bjSimulation : BjSimulation = new BjSimulation();

  constructor(private signalRService: SignalRService){}

  ngOnInit(): void {
    this.signalRService.bjSimulation$.subscribe(data => {
      this.bjSimulation = data;
    });
  }
}
