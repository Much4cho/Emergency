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
      pesel: ['', Validators.required],
      location: ['', Validators.required],
      emergencyType: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  onSubmit() {
    const emergency = new Emergency(
      this.emergencyTypes.indexOf(this.form.value.emergencyType) + 1,
      this.form.value.location,
      this.form.value.description,
      this.form.value.pesel
    );

    // TODO: emergencyType and modUser
    emergency.modUser = 'REGULAR_USER';

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
