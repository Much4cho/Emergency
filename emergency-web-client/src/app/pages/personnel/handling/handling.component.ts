import { Component, OnInit } from '@angular/core';
import { emergencyTypes } from 'src/app/_helpers/data';
import { Emergency } from 'src/app/_model/Emergency';
import {GatewayService} from '../../../_services/gateway.service';

@Component({
  selector: 'app-handling',
  templateUrl: './handling.component.html',
  styleUrls: ['./handling.component.scss']
})
export class HandlingComponent implements OnInit {

  emergencies: Array<any>;
  teams: Array<any>;

  emergencyColumns = ['location', 'type', 'description', 'reportTime', 'dispatchBtn'];
  teamColumns = ['name', 'location', 'num', 'dispatchBtn'];
  emergencyTypes = emergencyTypes;

  constructor(private gatewayService: GatewayService) {
    this.emergencies = new Array();
    this.teams = new Array();
   }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    // this.gatewayService.get
    const em1 = new Emergency(1, 'Location 1', 'Description 1');
    const em2 = new Emergency(2, 'Location 2', 'Description 2');
    const em3 = new Emergency(3, 'Location 3', 'Description 3');
    const em4 = new Emergency(4, 'Location 4', 'Description 4');
    this.emergencies.push(em1);
    this.emergencies.push(em2);
    this.emergencies.push(em3);
    this.emergencies.push(em4);
  }

  dispatch(emergencyId: number): void {

  }
}
