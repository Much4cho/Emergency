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

  constructor(private formBuilder: FormBuilder,
              private gatewayService: GatewayService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      pesel: ['', Validators.required],
    });
  }

  getEmergencyStatus() {
    this.gatewayService.getEmergencyStatus(this.form.value.pesel).subscribe(
      (res) => {
        console.log(res);
        this.emergencyStatus = res;
      }
    );
  }

}
