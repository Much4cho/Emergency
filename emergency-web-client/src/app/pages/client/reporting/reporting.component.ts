import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { emergencyTypes } from 'src/app/_helpers/data';
import {Emergency} from '../../../_model/Emergency';
import {GatewayService} from '../../../_services/gateway.service';

@Component({
  selector: 'app-reporting',
  templateUrl: './reporting.component.html',
  styleUrls: ['./reporting.component.scss']
})
export class ReportingComponent implements OnInit {
  form: FormGroup;
  emergencyTypes = emergencyTypes;

  constructor(private fb: FormBuilder,
              private gatewayService: GatewayService) {
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      location: ['', Validators.required],
      emergencyType: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  onSubmit() {
    const emergency = new Emergency(
      1, // this.emergencyTypes.indexOf(this.form.value.emergencyType),
      this.form.value.location,
      this.form.value.description
    );

    // TODO: emergencyType and modUser
    emergency.ModUser = 'REGULAR_USER';

    this.gatewayService.addEmergency(emergency).subscribe(
      next => {
        console.log(next);
      },
      error => {
        console.log(error.message);
      }
    );
  }
}
