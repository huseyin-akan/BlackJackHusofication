import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayBjComponent } from './components/play-bj/play-bj.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BalanceTableComponent } from './components/bj-simulator/balance-table/balance-table.component';
import { FormsModule } from '@angular/forms';
import { BjSimulatorComponent } from './components/bj-simulator/bj-simulator.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    PlayBjComponent,
    BalanceTableComponent,
    BjSimulatorComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    FlexLayoutModule,  
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatProgressBarModule,
    MatIconModule,
  ],
  exports:[
    PlayBjComponent,
    BalanceTableComponent,
    BjSimulatorComponent
  ]
})
export class BjModule { }
