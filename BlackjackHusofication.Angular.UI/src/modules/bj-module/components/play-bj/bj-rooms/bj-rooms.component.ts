import { Component } from '@angular/core';
import { BjGameHubService } from '../../../../../services/bjGameHubService.service';
import { BjGame } from '../../../../../models/bjGame';

@Component({
  selector: 'app-bj-rooms',
  templateUrl: './bj-rooms.component.html',
  styleUrl: './bj-rooms.component.css',
})
export class BjRoomsComponent {
  rooms: string[] = [];
  activeRoom: BjGame;

  constructor(private bjGameHubService: BjGameHubService) {}

  ngOnInit() {
    this.bjGameHubService.rooms$.subscribe((rooms) => (this.rooms = rooms));

    this.bjGameHubService.activeRoom$.subscribe(
      (room) => (this.activeRoom = room)
    );
  }

  joinRoom(roomName: string) {
    this.bjGameHubService
      .joinGroup(roomName)
      .catch((err) => console.error(err));
  }
}
