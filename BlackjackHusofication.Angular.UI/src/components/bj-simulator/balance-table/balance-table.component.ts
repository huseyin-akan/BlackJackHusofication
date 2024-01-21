import { Component } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';

@Component({
  selector: 'app-balance-table',
  standalone: true,
  imports: [MatButtonModule, MatCardModule],
  templateUrl: './balance-table.component.html',
  styleUrl: './balance-table.component.css'
})
export class BalanceTableComponent {

}
