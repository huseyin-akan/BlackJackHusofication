import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BjModule } from '../modules/bj-module/bj.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BjModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'BlackJackHusofication';
}