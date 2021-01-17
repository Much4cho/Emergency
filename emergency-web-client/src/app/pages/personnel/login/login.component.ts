import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { TokenStorageService } from 'src/app/_services/token-storage.service';
import {AuthService} from '../../../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  isLoggedIn = false;
  errorMessage = '';
  public loginInvalid: boolean;

  constructor(private fb: FormBuilder,
              private router: Router,
              private tokenStorage: TokenStorageService,
              private authService: AuthService) {
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
      this.router.navigate(['/home/personnel']);
    }
  }

  onSubmit(): void {
    this.authService.login(this.form.value.username, this.form.value.password).subscribe(
      (data) => {
        this.tokenStorage.saveToken(data.accessToken);
        this.tokenStorage.saveUser(data);

        this.loginInvalid = false;
        this.isLoggedIn = true;
        this.reloadPage();
      },
      (err) => {
        if (err.status === 200) {
          this.tokenStorage.saveToken(err.error.text);
          this.loginInvalid = false;
          this.isLoggedIn = true;
          this.reloadPage();
        } else {
          this.errorMessage = err.error.message;
          this.loginInvalid = true;
        }
      }
    );
  }

  reloadPage(): void {
    window.location.reload();
  }
}
