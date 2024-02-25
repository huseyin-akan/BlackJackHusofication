import { BjSimulation } from '../../../../../models/bjSimulation';
import { Component } from '@angular/core';
import { BjGameHubService } from '../../../../../services/bjGameHubService.service';

@Component({
  selector: 'app-balance-table',
  templateUrl: './balance-table.component.html',
  styleUrl: './balance-table.component.css'
})
export class BalanceTableComponent {
  bjSimulation : BjSimulation = new BjSimulation();

  constructor(private signalRService: BjGameHubService){}

  ngOnInit(): void {
    this.signalRService.bjSimulation$.subscribe(data => {
      this.bjSimulation = data;
    });
  }
}
