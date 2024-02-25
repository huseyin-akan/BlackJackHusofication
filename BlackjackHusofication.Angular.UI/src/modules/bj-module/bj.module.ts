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
import { MatInputModule} from '@angular/material/input';
import { BjRoomsComponent } from './components/play-bj/bj-rooms/bj-rooms.component';
import { BjGameAreaComponent } from './components/play-bj/bj-game-area/bj-game-area.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { ToasterService } from '../../services/toasterService.service';

@NgModule({
  declarations: [
    PlayBjComponent,
    BalanceTableComponent,
    BjSimulatorComponent,
    BjRoomsComponent,
    BjGameAreaComponent,
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
    MatInputModule,
    ToastrModule.forRoot({
      positionClass :'toast-bottom-right'
    }), 
  ],
  exports:[
    PlayBjComponent,
    BalanceTableComponent,
    BjSimulatorComponent
  ],
  providers:[
    ToasterService,
    ToastrService
  ]
})
export class BjModule { }