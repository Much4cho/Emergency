import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GatewayService } from 'src/app/_services/gateway.service';
import { TokenStorageService } from 'src/app/_services/token-storage.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  years = ['2017', '2018', '2019', '2020', '2021', '2022', '2023'];
  months = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];

  isLoggedIn = false;

  stats: any;
  timeStats: any;

  numYear: number;
  numMonth: number;

  formGroup: FormGroup;

  // options
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Typ zdarzenia';
  showYAxisLabel = true;
  yAxisLabel = 'Liczba zgłoszeń';

  constructor(private tokenStorageService: TokenStorageService,
              private router: Router,
              private gatewayService: GatewayService,
              private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      year: ['2021', Validators.required],
      month: ['01', Validators.required]
    });
    this.isLoggedIn = !!this.tokenStorageService.getToken();

    if (!this.isLoggedIn) {
      this.router.navigate(['/home/login']);
    } else {
      this.loadData();
    }
  }

  loadData() {
    this.getStatistics(2021, 1);
  }

  getStatistics(year, month) {
    this.gatewayService.getStatistics(year, month).subscribe(
      (res) => {
        this.stats = res;
        console.log(this.stats);
      }
    );
    this.gatewayService.getTimeStatistics(year, month).subscribe(
      (res) => {
        this.timeStats = res;
        console.log(this.timeStats);
      }
    );
  }

  submitForm() {
    this.numYear = this.formGroup.value.year;
    this.numMonth = this.formGroup.value.month;
    this.getStatistics(this.numYear, this.numMonth);
  }
}
