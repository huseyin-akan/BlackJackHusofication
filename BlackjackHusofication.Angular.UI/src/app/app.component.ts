import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BjSimulatorComponent } from '../components/bj-simulator/bj-simulator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BjSimulatorComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'BlackJackHusofication';
}
