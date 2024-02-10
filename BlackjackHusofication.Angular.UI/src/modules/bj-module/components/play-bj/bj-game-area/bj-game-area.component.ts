import { Component } from '@angular/core';
import { BjGame } from '../../../../../models/bjGame';
import { BjGameHubService } from '../../../../../services/bjGameHubService';

@Component({
  selector: 'app-bj-game-area',
  templateUrl: './bj-game-area.component.html',
  styleUrl: './bj-game-area.component.css'
})
export class BjGameAreaComponent {
  players: boolean[] = Array(7).fill(false); 
  activeRoom :BjGame ;

  constructor(private bjGameHubService :BjGameHubService){}

  ngOnInit(){
    this.bjGameHubService.joinGroup("Blackjack - 1");

    this.bjGameHubService.activeRoom$.subscribe(room => {
      this.activeRoom = room

      if(this.activeRoom.isDealingAllCards){
        
      }

      console.log(room)
    });
  }

  sitPlayer(spotId: number): void {
    this.bjGameHubService.sitPlayer(this.activeRoom.name, spotId)
    .catch((err) => console.error(err));
  }

  betPlayer(spotId: number): void {
    this.bjGameHubService.playerBet(this.activeRoom.name, spotId, 100)
    .catch((err) => console.error(err));
  }

  standPlayer(index: number): void {
    this.players[index] = false;
  }

  dealAllCards(){

  }
}
