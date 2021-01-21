import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GatewayService } from 'src/app/_services/gateway.service';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.scss']
})
export class StatusComponent implements OnInit {

  form: FormGroup;
  emergencyStatus: any;
  identifier: any;

  constructor(private formBuilder: FormBuilder,
              private gatewayService: GatewayService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      pesel: ['', Validators.required],
    });
  }

  getEmergencyStatus() {
    this.identifier = this.form.value.pesel;
    console.log(this.identifier);
    this.gatewayService.getEmergencyStatus(this.identifier).subscribe(
      (res) => {
        console.log(res);
        this.emergencyStatus = res;
      }
    );
  }

  refreshStatus() {
    this.gatewayService.getEmergencyStatus(this.identifier).subscribe(
      (res) => {
        console.log(res);
        this.emergencyStatus = res;
      }
    );
  }

}
