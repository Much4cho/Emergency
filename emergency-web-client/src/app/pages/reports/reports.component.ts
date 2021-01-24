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
  timeStats1: any;
  timeStats2: any;
  timeStats3: any;

  numYear: number;
  numMonth: number;

  formGroup: FormGroup;

  // options
  view: any[] = [400, 400];
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = false;
  showXAxisLabel = true;
  showYAxisLabel = true;
  yAxisLabel1 = 'Liczba zgłoszeń';
  yAxisLabel2 = 'Czas w sekundach';
  xAxisLabel1 = 'Sumy zgłoszeń w miesiącu';
  xAxisLabel2 = 'Średni czas przydzielenia zespołu';
  xAxisLabel3 = 'Średni czas od zgłoszenia do zakończenia akcji';
  xAxisLabel4 = 'Średni czas od przydzielenia zespołu do zakończenia akcji';



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
        this.stats = new Array<{
          name: string,
          value: string
        }>();
        res.forEach(s => {
          this.stats.push({
            name: s.name,
            value: s.count
          });
        });
      }
    );
    this.gatewayService.getTimeStatistics(year, month).subscribe(
      (res) => {
        this.timeStats = res;
        this.timeStats1 = new Array<{
          name: string,
          value: string
        }>();
        this.timeStats.filter(s => s.statisticType === 1).forEach(s => {
          this.timeStats1.push({
            name: s.emergencyType,
            value: this.getSeconds(s.timeAverage)
          });
        });
        this.timeStats2 = new Array<{
          name: string,
          value: string
        }>();
        this.timeStats.filter(s => s.statisticType === 2).forEach(s => {
          this.timeStats2.push({
            name: s.emergencyType,
            value: this.getSeconds(s.timeAverage)
          });
        });
        this.timeStats3 = new Array<{
          name: string,
          value: string
        }>();
        this.timeStats.filter(s => s.statisticType === 3).forEach(s => {
          this.timeStats3.push({
            name: s.emergencyType,
            value: this.getSeconds(s.timeAverage)
          });
        });
      }
    );
  }

  submitForm() {
    this.numYear = this.formGroup.value.year;
    this.numMonth = this.formGroup.value.month;
    this.getStatistics(this.numYear, this.numMonth);
  }

  getSeconds(time: string): number {
    const h = parseInt(time.substr(0, 2), 10);
    const m = parseInt(time.substr(4, 6), 10);
    const s = parseInt(time.substr(8, 10), 10);
    const ms = parseInt(time.substr(12, 14), 10);

    return (h * 3600) + (m * 60) + s + (ms * 0.001);
  }
}
