import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PagesComponent} from './pages.component';
import {RouterModule} from '@angular/router';
import {SharedModule} from '../shared/shared.module';
import {MatSidenavModule} from '@angular/material/sidenav';
import {ClientComponent} from './client/client.component';
import {PersonnelComponent} from './personnel/personnel.component';
import {ReportsComponent} from './reports/reports.component';
import {LoginComponent} from './personnel/login/login.component';
import {ReactiveFormsModule} from '@angular/forms';
import {MatCardModule} from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {ReportingComponent} from './client/reporting/reporting.component';
import {MatOptionModule} from '@angular/material/core';
import {MatSelectModule} from '@angular/material/select';
import {MatTableModule} from '@angular/material/table';
import {MatSortModule} from '@angular/material/sort';
import {HttpClientModule} from '@angular/common/http';
import {MatRadioModule} from '@angular/material/radio';
import { authInterceptorProviders } from '../_helpers/auth.interceptor';


@NgModule({
  declarations: [
    PagesComponent,
    ClientComponent,
    PersonnelComponent,
    ReportsComponent,
    LoginComponent,
    ReportingComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule,
    SharedModule,
    MatSidenavModule,
    SharedModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    MatTableModule,
    MatSortModule,
    MatRadioModule,
  ],
  providers: [authInterceptorProviders]
})
export class PagesModule {
}
