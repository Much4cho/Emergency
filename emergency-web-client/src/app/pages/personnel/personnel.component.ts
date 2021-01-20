import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { emergencyTypes } from 'src/app/_helpers/data';
import { Emergency } from 'src/app/_model/Emergency';
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
  teamColumns = ['name', 'location', 'dispatchBtn'];
  emergencyTypes = emergencyTypes;

  selectedEmergency: any;
  isSelected = false;

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
        this.emergencies = data.filter((d) => d.status < 3);
      }
    );
    this.gatewayService.getTeams().subscribe(
      (data) => {
        this.teams = data;
        console.log(this.teams);
      }
    );
  }

  selectEm(emergency) {
    this.selectedEmergency = emergency;
    this.isSelected = true;
    console.log(this.selectedEmergency.status);
  }

  dispatch(team): void {
    this.selectedEmergency.status = 3;
    this.selectedEmergency.assignedToTeamId = team.id;
    this.selectedEmergency.assignedToTeam = team;
    team.assignedEmergencyId = this.selectedEmergency.id;
    console.log(team);
    this.gatewayService.updateEmergency(this.selectedEmergency).subscribe(
      (res) => {
        console.log(res);
        this.isSelected = null;
        this.gatewayService.updateTeam(team).subscribe(
          (next) => {
            console.log(next);
            this.loadData();
          }
        );
      },
      (error) => {
        console.log(error);
      }
    );


    this.emergencies = this.emergencies.filter((d) => d.status < 3);
    this.isSelected = false;
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
