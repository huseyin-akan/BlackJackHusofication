import { Component } from '@angular/core';
import { BlackJackGame } from '../../../../models/blackjackGame';

@Component({
  selector: 'app-play-bj',
  templateUrl: './play-bj.component.html',
  styleUrl: './play-bj.component.css'
})
export class PlayBjComponent {
  players: boolean[] = Array(7).fill(false); 
  dealerOccupied: boolean = false;
  game : BlackJackGame;

  sitPlayer(index: number): void {
    this.players[index] = true;
  }

  standPlayer(index: number): void {
    this.players[index] = false;
  }

  sitDealer(): void {
    this.dealerOccupied = true;
  }

  standDealer(): void {
    this.dealerOccupied = false;
  }
}
