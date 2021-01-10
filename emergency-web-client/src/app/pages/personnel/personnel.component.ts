import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { emergencyTypes } from 'src/app/_helpers/data';
import { GatewayService } from 'src/app/_services/gateway.service';
import { TokenStorageService } from 'src/app/_services/token-storage.service';

@Component({
  selector: 'app-personnel',
  templateUrl: './personnel.component.html',
  styleUrls: ['./personnel.component.scss']
})
export class PersonnelComponent implements OnInit {

  emergencies: Array<any>;
  teams: Array<any>;

  emergencyColumns = ['location', 'type', 'description', 'reportTime', 'dispatchBtn'];
  teamColumns = ['name', 'location', 'num', 'dispatchBtn'];
  emergencyTypes = emergencyTypes;

  constructor(private gatewayService: GatewayService,
              private tokenStorageService: TokenStorageService,
              private router: Router) {
    this.emergencies = new Array();
    this.teams = new Array();
   }

  ngOnInit(): void {
    if (this.checkLogin()) {
      this.loadData();
    }
  }

  loadData(): void {
    this.gatewayService.getEmergencies().subscribe(
      (data) => {
        this.emergencies = data;
      }
    );
    // const em1 = new Emergency(1, 'Location 1', 'Description 1');
    // const em2 = new Emergency(2, 'Location 2', 'Description 2');
    // const em3 = new Emergency(3, 'Location 3', 'Description 3');
    // const em4 = new Emergency(4, 'Location 4', 'Description 4');
    // this.emergencies.push(em1);
    // this.emergencies.push(em2);
    // this.emergencies.push(em3);
    // this.emergencies.push(em4);
  }

  dispatch(emergencyId: number): void {

  }

  checkLogin(): boolean {
    const isLoggedIn = !!this.tokenStorageService.getToken();

    if (!isLoggedIn) {
      this.router.navigate(['/home/login']);
      return false;
    } else {
      return true;
    }
  }

}
