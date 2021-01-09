import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  public loginInvalid: boolean;
  private formSubmitAttempt: boolean;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private authService: AuthService) {
  }

  async ngOnInit(): Promise<void> {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    // if (await this.authService.checkAuthenticated()) {
    //   await this.router.navigate([this.returnUrl]);
    // }
  }

  onSubmit() {
    this.loginInvalid = false;
    this.formSubmitAttempt = false;
    this.router.navigateByUrl('home/personnel/handling').then(r => {
    });
    // if (this.form.valid) {
    //   try {
    //     const username = this.form.get('username').value;
    //     const password = this.form.get('password').value;
    //     // await this.authService.login(username, password);
    //   } catch (err) {
    //     this.loginInvalid = true;
    //   }
    // } else {
    //   this.formSubmitAttempt = true;
    // }
  }

}
