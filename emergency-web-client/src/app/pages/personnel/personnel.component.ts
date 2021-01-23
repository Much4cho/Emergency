import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
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
  teamsSource: MatTableDataSource<any>;

  emergencyColumns = ['location', 'type', 'description', 'reportTime', 'ejectBtn', 'dispatchBtn'];
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
    this.teamsSource = null;
    this.teams = new Array();
    this.gatewayService.getEmergencies().subscribe(
      (data) => {
        this.emergencies = data.filter((d) => d.status < 2);
      }
    );
    this.gatewayService.getTeams().subscribe(
      (data) => {
        data.forEach((t) => this.isTeamBusy(t));
      }
    );
  }

  selectEm(emergency) {
    this.selectedEmergency = emergency;
    this.isSelected = true;
    console.log(this.selectedEmergency.status);
  }

  dispatch(team): void {
    this.selectedEmergency.status = 2;
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
    this.emergencies = this.emergencies.filter((d) => d.status < 2);
    this.isSelected = false;
    this.loadData();
  }

  reject(emergency) {
    emergency.status = 4;
    this.gatewayService.updateEmergency(this.selectedEmergency).subscribe(
      (res) => {
        this.isSelected = null;
        this.gatewayService.getEmergencies().subscribe(
          (data) => {
            this.emergencies = data.filter((d) => d.status < 2);
          }
        );
      },
      (error) => {
        console.log(error);
      });
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

  isTeamBusy(team) {
    this.gatewayService.getTeamsEmergency(team.id).subscribe(
      (res) => {
        console.log(res);
        if (res == null) {
          if (this.teams.filter(e => e.id === team.id).length === 0) {
            this.teams.push(team);
          }
          this.teamsSource = new MatTableDataSource(this.teams);
        }
      });
  }
}
