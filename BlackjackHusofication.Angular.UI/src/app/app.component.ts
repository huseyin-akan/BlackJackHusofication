import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BjComponent } from '../components/bj/bj.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BjComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'BlackJackHusofication';
}
