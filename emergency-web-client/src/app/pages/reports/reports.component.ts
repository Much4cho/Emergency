import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GatewayService } from 'src/app/_services/gateway.service';
import { TokenStorageService } from 'src/app/_services/token-storage.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  isLoggedIn = false;

  constructor(private tokenStorageService: TokenStorageService,
              private router: Router,
              private gatewayService: GatewayService) { }

  ngOnInit(): void {
    this.isLoggedIn = !!this.tokenStorageService.getToken();

    if (!this.isLoggedIn) {
      this.router.navigate(['/home/login']);
    }
    // this.loadData();
  }

  // loadData() {
  //   this.gatewayService.getStatistics(2,2)
  // }

}
