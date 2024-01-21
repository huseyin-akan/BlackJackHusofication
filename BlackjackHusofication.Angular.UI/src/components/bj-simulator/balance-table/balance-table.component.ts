import { BjSimulation } from './../../../models/bjSimulation';
import { Component } from '@angular/core';
import { MatButtonModule} from '@angular/material/button';
import { MatCardModule} from '@angular/material/card';
import { MatDividerModule} from '@angular/material/divider';
import { MatProgressBarModule} from '@angular/material/progress-bar';
import { MatIconModule} from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SignalRService } from '../../../services/signalRService';

@Component({
  selector: 'app-balance-table',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatDividerModule, MatProgressBarModule, MatIconModule, CommonModule, FlexLayoutModule],
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
