import { Component } from '@angular/core';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss']
})
export class PagesComponent {

  sideBarOpened = true;

  constructor() { }

  toggleSideBar(): void {
    this.sideBarOpened = !this.sideBarOpened;
  }

}
